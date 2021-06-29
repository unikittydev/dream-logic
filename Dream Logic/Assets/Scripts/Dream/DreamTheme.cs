using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Dream
{
    /// <summary>
    /// ���� ���.
    /// </summary>
    [CreateAssetMenu(fileName = "New Dream Theme", menuName = "Dream/Theme")]
    public class DreamTheme : ScriptableObject
    {
        public DreamStyle style;

        public FloorTile floorPrefab;

        public Color skyColor;

        public float cameraAngle;
        public float cameraDistance;

        public PlayerController playerPrefab;

        public GameObject[] enemies;

        public GameObject[] obstacles;

        public VolumeProfile postprocessing;
    }
}
