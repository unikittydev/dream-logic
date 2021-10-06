using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Rendering;

namespace Game.Dream
{
    /// <summary>
    /// ���� ���.
    /// </summary>
    [CreateAssetMenu(fileName = "New Dream Theme", menuName = "Dream/Theme")]
    public class DreamTheme : ScriptableObject
    {
        public FloorSpawnerSettings floorSpawnerSettings;

        public Color skyColor;

        public float cameraAngle;
        public float cameraDistance;

        public PlayerController playerPrefab;

        public ObjectSpawnerSettings[] objectSpawnerSettings;

        public VolumeProfile postprocessing;

        public DreamModeFlag allowedModes;

        public LocalizedString description;

        public Sound themeSound;

        public Vector2Int minMaxEnemyCost;
    }
}
