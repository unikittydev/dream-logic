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

            for (int i = -tileRadius; i <= tileRadius; i++)
                for (int j = -tileRadius; j <= tileRadius; j++)
                {
                    Vector3Int playerTilePos = Vector3Int.RoundToInt(DreamSimulation.player.transform.position / defaultTileSize);
                    Vector3 desiredTilePos = new Vector3(i + playerTilePos.x, 0f, j + playerTilePos.z) * defaultTileSize;

                    bool foundTile = false;
                    for (int k = 0; k < floorTiles.Count; k++)
                    {
                        Vector3 planePos = floorTiles[k].transform.position;
                        planePos.y = 0f;
                        if (Mathf.Approximately((desiredTilePos - planePos).sqrMagnitude, 0f))
                        {
                            foundTile = true;
                            break;
                        }
                    }
                    if (!foundTile)
                    {
                        var newTile = CreateTile(desiredTilePos);
                        desiredTilePos.y = settings.startHeight;
                        newTile.transform.position = desiredTilePos;
                        floorTiles.Add(newTile);
                    }
                }
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
                position.y += (floorTiles[k].transform.lossyScale.y - newTile.transform.lossyScale.y) * .5f;
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
            while (!includePlayerTile && Mathf.Approximately((floorTilePos - playerTilePos).sqrMagnitude, 0f));

            Vector3 position = floorTiles[k].transform.position;

            floorTiles[k].Despawn(settings.startHeight, settings.smoothTime);
            var newTile = CreateTile(position, prefab);
            position.y = floorTiles[k].transform.position.y + (floorTiles[k].transform.lossyScale.y - newTile.transform.lossyScale.y) * .5f;
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
            newTile.Spawn(prefab.transform.lossyScale.y * -.5f + Random.Range(-settings.heightOffset, settings.heightOffset), settings.smoothTime);

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
