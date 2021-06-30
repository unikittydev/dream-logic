using UnityEngine;

namespace Game
{
    /// <summary>
    /// Настройки спавнера противников.
    /// </summary>
    [CreateAssetMenu(fileName = "Enemy Spawner Settings", menuName = "Dream/Enemy Spawner")]
    public class EnemySpawnerSettings : ScriptableObject
    {
        public GameObject[] prefabs;

        public float minDistance;
        public float maxDistance;
        public float height;

        public float frequency;
    }
}
