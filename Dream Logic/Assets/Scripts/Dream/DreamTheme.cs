using UnityEngine;

[CreateAssetMenu(fileName = "New Dream Theme", menuName = "Dream/Theme")]
public class DreamTheme : ScriptableObject
{
    public FloorTile floorPrefab;
    public Color skyColor;

    public PlayerController player;
    public GameObject[] enemies;
}
