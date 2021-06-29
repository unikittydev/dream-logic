using UnityEngine;

public class DreamSimulation : MonoBehaviour
{
    [SerializeField]
    private DreamTheme theme;

    [SerializeField]
    private FloorSpawner floorSpawner;
    private SimpleSpawner[] enemySpawners;

    private void OnValidate()
    {
        floorSpawner.floorPrefab = theme.floorPrefab;
    }
}
