using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Dream
{
    /// <summary>
    /// Тема сна.
    /// </summary>
    [CreateAssetMenu(fileName = "New Dream Theme", menuName = "Dream/Theme")]
    public class DreamTheme : ScriptableObject
    {
        public DreamStyle style;

        public FloorSpawnerSettings floorSpawnerSettings;

        public Color skyColor;

        public float cameraAngle;
        public float cameraDistance;

        public PlayerController playerPrefab;

        public ObjectSpawnerSettings[] objectSpawnerSettings;

        public VolumeProfile postprocessing;

        public DreamModeFlag allowedModes;
    }
}
