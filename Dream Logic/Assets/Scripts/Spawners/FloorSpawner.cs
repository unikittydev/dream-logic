using Game.Dream;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Спавнер плиток пола.
    /// </summary>
    public class FloorSpawner : MonoBehaviour
    {
        public const float defaultTileSize = 8f;

        private Transform tr;
        private List<FloorTile> floorTiles;

        [SerializeField]
        private FloorSpawnerSettings settings;

        private static ProfilerMarker spawn = new ProfilerMarker("SpawnCloseTiles");
        private static ProfilerMarker despawn = new ProfilerMarker("DespawnFarTiles");

        private void Awake()
        {
            tr = transform;
            floorTiles = new List<FloorTile>(FindObjectsOfType<FloorTile>());
        }

        private void SetupWeights()
        {
            settings.cumWeights = new float[settings.weights.Length];

            float sum = 0f, cumSum = 0f;
            for (int i = 0; i < settings.weights.Length; i++)
            {
                sum += settings.weights[i];
            }
            for (int i = 0; i < settings.weights.Length; i++)
            {
                settings.weights[i] /= sum;
                cumSum += settings.weights[i];
                settings.cumWeights[i] = cumSum;
            }
        }

        private void Update()
        {
            int tileRadius = Mathf.RoundToInt(settings.tileRadius * DreamGame.difficulty.playerSpeedMultiplier);

            SpawnCloseTiles(tileRadius);
            DespawnFarTiles(tileRadius);
        }

        private void SpawnCloseTiles(int tileRadius)
        {
            spawn.Begin();
            Vector3Int playerTilePos = Vector3Int.RoundToInt(DreamGame.player.tr.position / defaultTileSize);
            playerTilePos.y = 0;

            for (int i = -tileRadius; i <= tileRadius; i++)
                for (int j = -tileRadius; j <= tileRadius; j++)
                {
                    Vector3Int desiredTilePos = playerTilePos + new Vector3Int(i, 0, j);

                    if (!TryFindTile(desiredTilePos))
                    {
                        var newTile = CreateTile(desiredTilePos);
                        floorTiles.Add(newTile);
                        Debug.DrawLine(DreamGame.player.tr.position, newTile.transform.position, Color.red, 2f);
                    }
                }
            spawn.End();
        }

        private bool TryFindTile(Vector3Int position)
        {
            for (int k = 0; k < floorTiles.Count; k++)
                if (floorTiles[k].tilePosition == position)
                    return true;
            return false;
        }

        private void DespawnFarTiles(int tileRadius)
        {
            despawn.Begin();
            Vector3Int playerTilePos = Vector3Int.RoundToInt(DreamGame.player.tr.position / defaultTileSize);
            playerTilePos.y = 0;

            for (int k = 0; k < floorTiles.Count; k++)
            {
                Vector3Int floorTilePos = floorTiles[k].tilePosition;
                if (Mathf.Abs(playerTilePos.x - floorTilePos.x) > tileRadius || Mathf.Abs(playerTilePos.z - floorTilePos.z) > tileRadius)
                {
                    floorTiles[k].Despawn(settings.startHeight, settings.smoothTime);
                    floorTiles.RemoveAt(k);
                }
            }
            despawn.End();
        }

        public void Refresh(FloorSpawnerSettings newSettings)
        {
            settings = newSettings;
            SetupWeights();
            for (int k = 0; k < floorTiles.Count; k++)
            {
                ReplaceTile(k);
            }
        }

        public void ReplaceRandomTile(FloorTile prefab, bool includePlayerTile = false)
        {
            Vector3Int playerTilePos = Vector3Int.RoundToInt(DreamGame.player.tr.position / defaultTileSize);
            playerTilePos.y = 0;
            Vector3Int floorTilePos;
            int k;

            do
            {
                k = Random.Range(0, floorTiles.Count);
                floorTilePos = floorTiles[k].tilePosition;
            }
            while (!includePlayerTile && floorTilePos == playerTilePos);

            ReplaceTile(k, prefab);

            AudioManager.instance.Play("fall");
        }

        private FloorTile CreateTile(Vector3Int position)
        {
            var floorPrefab = GetRandomPrefab();
            return CreateTile(position, floorPrefab);
        }

        private FloorTile CreateTile(Vector3Int position, FloorTile prefab)
        {
            FloorTile newTile = Instantiate(prefab, new Vector3(defaultTileSize * position.x, settings.startHeight, defaultTileSize * position.z), Quaternion.identity, tr);
            newTile.Spawn(Random.Range(-settings.heightOffset, settings.heightOffset), settings.smoothTime);

            return newTile;
        }

        private void ReplaceTile(int oldIndex)
        {
            var floorPrefab = GetRandomPrefab();
            ReplaceTile(oldIndex, floorPrefab);
        }

        private void ReplaceTile(int oldIndex, FloorTile newPrefab)
        {
            floorTiles[oldIndex].Despawn(settings.startHeight, settings.smoothTime);
            var newTile = CreateTile(floorTiles[oldIndex].tilePosition, newPrefab);

            Vector3 position = floorTiles[oldIndex].transform.position;
            position.y += (floorTiles[oldIndex].tileHeight - newTile.tileHeight) * .5f;
            newTile.transform.position = position;

            floorTiles[oldIndex] = newTile;
        }

        private FloorTile GetRandomPrefab()
        {
            float value = Random.value;
            int index;
            for (index = 0; index < settings.cumWeights.Length && value > settings.cumWeights[index]; index++) { }

            return settings.floorPrefab[index];
        }
    }
}
