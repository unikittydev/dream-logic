using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    private const float defaultTileSize = 8f;

    [SerializeField]
    private FloorTile floorPrefab;
    [SerializeField]
    private Transform environment;
    [SerializeField]
    private List<FloorTile> floorTiles;

    private GameObject player;

    [SerializeField, Header("Spawn Settings")]
    private int tileRadius;
    [SerializeField]
    private float heightOffset;
    [SerializeField]
    private float startHeight;
    [SerializeField]
    private float endHeight;
    [SerializeField]
    private float smoothTime;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        for (int i = -tileRadius; i <= tileRadius; i++)
            for (int j = -tileRadius; j <= tileRadius; j++)
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
                    desiredTilePos.y = startHeight;
                    FloorTile newTile = Instantiate(floorPrefab, desiredTilePos, Quaternion.identity, environment);
                    newTile.Spawn(endHeight + Random.Range(-heightOffset, heightOffset), smoothTime);
                    floorTiles.Add(newTile);
                }
            }
        for (int k = 0; k < floorTiles.Count; k++)
        {
            Vector3Int playerTilePos = Vector3Int.RoundToInt(player.transform.position / defaultTileSize);
            Vector3Int floorTilePos = Vector3Int.RoundToInt(floorTiles[k].transform.position / defaultTileSize);
            if (Mathf.Abs(playerTilePos.x - floorTilePos.x) > tileRadius || Mathf.Abs(playerTilePos.z - floorTilePos.z) > tileRadius)
            {
                floorTiles[k].Despawn(startHeight, smoothTime);
                floorTiles.RemoveAt(k);
            }
        }
    }
}
