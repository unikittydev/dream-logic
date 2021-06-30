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

        private Transform environment;
        private List<FloorTile> floorTiles;

        private GameObject player;

        [SerializeField]
        private FloorSpawnerSettings settings;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag(GameTags.player);
            environment = GameObject.FindGameObjectWithTag(GameTags.environment).transform;
            floorTiles = new List<FloorTile>(FindObjectsOfType<FloorTile>());
        }

        private void Update()
        {
            for (int i = -settings.tileRadius; i <= settings.tileRadius; i++)
                for (int j = -settings.tileRadius; j <= settings.tileRadius; j++)
                {
                    Vector3Int playerTilePos = Vector3Int.RoundToInt(player.transform.position / defaultTileSize);
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
                        FloorTile newTile = Instantiate(settings.floorPrefab, desiredTilePos, Quaternion.identity, environment);
                        newTile.Spawn(settings.endHeight + Random.Range(-settings.heightOffset, settings.heightOffset), settings.smoothTime);
                        floorTiles.Add(newTile);
                    }
                }
            for (int k = 0; k < floorTiles.Count; k++)
            {
                Vector3Int playerTilePos = Vector3Int.RoundToInt(player.transform.position / defaultTileSize);
                Vector3Int floorTilePos = Vector3Int.RoundToInt(floorTiles[k].transform.position / defaultTileSize);
                if (Mathf.Abs(playerTilePos.x - floorTilePos.x) > settings.tileRadius || Mathf.Abs(playerTilePos.z - floorTilePos.z) > settings.tileRadius)
                {
                    floorTiles[k].Despawn(settings.startHeight, settings.smoothTime);
                    floorTiles.RemoveAt(k);
                }
            }
        }
    }
}
