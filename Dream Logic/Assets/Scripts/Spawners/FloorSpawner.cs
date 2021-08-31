using Game.Dream;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Спавнер плиток пола.
    /// </summary>
    public class FloorSpawner : MonoBehaviour
    {
        private const float defaultTileSize = 8f;

        private Transform tr;
        private List<FloorTile> floorTiles;

        [SerializeField]
        private FloorSpawnerSettings settings;

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
            int tileRadius = Mathf.RoundToInt(settings.tileRadius * DreamSimulation.difficulty.playerSpeedMultiplier);

            SpawnCloseTiles(tileRadius);
            DespawnFarTiles(tileRadius);
        }

        private void SpawnCloseTiles(int tileRadius)
        {
            for (int i = -tileRadius; i <= tileRadius; i++)
                for (int j = -tileRadius; j <= tileRadius; j++)
                {
                    Vector3Int playerTilePos = Vector3Int.RoundToInt(DreamSimulation.player.transform.position / defaultTileSize);
                    Vector3 desiredTilePos = new Vector3(i + playerTilePos.x, 0f, j + playerTilePos.z) * defaultTileSize;

                    if (!TryFindTile(desiredTilePos))
                    {
                        var newTile = CreateTile(desiredTilePos);
                        desiredTilePos.y = settings.startHeight;
                        newTile.transform.position = desiredTilePos;
                        floorTiles.Add(newTile);
                    }
                }
        }

        private bool TryFindTile(Vector3 position)
        {
            for (int k = 0; k < floorTiles.Count; k++)
            {
                Vector3 planePos = floorTiles[k].transform.position;
                planePos.y = 0f;
                if (Mathf.Approximately((position - planePos).sqrMagnitude, 0f))
                {
                    return true;
                }
            }
            return false;
        }

        private void DespawnFarTiles(int tileRadius)
        {
            for (int k = 0; k < floorTiles.Count; k++)
            {
                Vector3Int playerTilePos = Vector3Int.RoundToInt(DreamSimulation.player.transform.position / defaultTileSize);
                Vector3Int floorTilePos = Vector3Int.RoundToInt(floorTiles[k].transform.position / defaultTileSize);
                if (Mathf.Abs(playerTilePos.x - floorTilePos.x) > tileRadius || Mathf.Abs(playerTilePos.z - floorTilePos.z) > tileRadius)
                {
                    floorTiles[k].Despawn(settings.startHeight, settings.smoothTime);
                    floorTiles.RemoveAt(k);
                }
            }
        }

        public void Refresh(FloorSpawnerSettings newSettings)
        {
            settings = newSettings;
            SetupWeights();
            for (int k = 0; k < floorTiles.Count; k++)
            {
                Vector3 position = floorTiles[k].transform.position;

                floorTiles[k].Despawn(settings.startHeight, settings.smoothTime);
                var newTile = CreateTile(position);
                
                position.y += (floorTiles[k].tileHeight - newTile.tileHeight) * .5f;
                newTile.transform.position = position;
                floorTiles[k] = newTile;
            }
        }

        public void ReplaceRandomTile(FloorTile prefab, bool includePlayerTile = false)
        {
            Vector3Int playerTilePos = Vector3Int.RoundToInt(DreamSimulation.player.transform.position / defaultTileSize);
            playerTilePos.y = 0;
            Vector3Int floorTilePos;
            int k;

            do
            {
                k = Random.Range(0, floorTiles.Count);
                floorTilePos = Vector3Int.RoundToInt(floorTiles[k].transform.position / defaultTileSize);
            }
            while (!includePlayerTile && floorTilePos == playerTilePos);

            Vector3 position = floorTiles[k].transform.position;

            floorTiles[k].Despawn(settings.startHeight, settings.smoothTime);
            var newTile = CreateTile(position, prefab);
            position.y = floorTiles[k].transform.position.y + (floorTiles[k].tileHeight - newTile.tileHeight) * .5f;
            newTile.transform.position = position;
            floorTiles[k] = newTile;

            AudioManager.instance.Play("fall");
        }

        private FloorTile CreateTile(Vector3 position)
        {
            var floorPrefab = GetRandomPrefab();
            return CreateTile(position, floorPrefab);
        }

        private FloorTile CreateTile(Vector3 position, FloorTile prefab)
        {
            FloorTile newTile = Instantiate(prefab, position, Quaternion.identity, tr);
            newTile.Spawn(Random.Range(-settings.heightOffset, settings.heightOffset), settings.smoothTime);

            return newTile;
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
