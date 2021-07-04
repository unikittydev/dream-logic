using UnityEngine;

namespace Game
{
    /// <summary>
    /// Настройки для спавнера плиток пола.
    /// </summary>
    [CreateAssetMenu(fileName = "Floor Spawner Settings", menuName = "Dream/Floor Spawner")]
    public class FloorSpawnerSettings : ScriptableObject
    {
        public FloorTile[] floorPrefab;
        public float[] weights;
        [HideInInspector]
        public float[] cumWeights;

        [Space]
        public int tileRadius;
        public float heightOffset;
        public float startHeight;
        public float smoothTime;
    }
}
