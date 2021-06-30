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

        private void Update()
        {
            for (int i = -settings.tileRadius; i <= settings.tileRadius; i++)
                for (int j = -settings.tileRadius; j <= settings.tileRadius; j++)
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
                        desiredTilePos.y = settings.startHeight;
                        FloorTile newTile = Instantiate(settings.floorPrefab, desiredTilePos, Quaternion.identity, tr);
                        newTile.Spawn(settings.endHeight + Random.Range(-settings.heightOffset, settings.heightOffset), settings.smoothTime);
                        floorTiles.Add(newTile);
                    }
                }
            for (int k = 0; k < floorTiles.Count; k++)
            {
                Vector3Int playerTilePos = Vector3Int.RoundToInt(DreamSimulation.player.transform.position / defaultTileSize);
                Vector3Int floorTilePos = Vector3Int.RoundToInt(floorTiles[k].transform.position / defaultTileSize);
                if (Mathf.Abs(playerTilePos.x - floorTilePos.x) > settings.tileRadius || Mathf.Abs(playerTilePos.z - floorTilePos.z) > settings.tileRadius)
                {
                    floorTiles[k].Despawn(settings.startHeight, settings.smoothTime);
                    floorTiles.RemoveAt(k);
                }
            }
        }

        public void Refresh(FloorSpawnerSettings newSettings)
        {
            settings = newSettings;
            for (int k = 0; k < floorTiles.Count; k++)
            {
                Vector3 position = floorTiles[k].transform.position;
                position.y = settings.endHeight;

                FloorTile newTile = Instantiate(settings.floorPrefab, position, Quaternion.identity, tr);
                floorTiles[k].Despawn(settings.startHeight, settings.smoothTime);
                floorTiles[k] = newTile;
            }
        }
    }
}
