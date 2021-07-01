using UnityEngine;

namespace Game
{
    /// <summary>
    /// Настройки спавнера объектов.
    /// </summary>
    [CreateAssetMenu(fileName = "Object Spawner Settings", menuName = "Dream/Object Spawner")]
    public class ObjectSpawnerSettings : ScriptableObject
    {
        public GameObject[] prefabs;

        public float minDistance;
        public float maxDistance;
        public float height;

        public float frequency;
    }
}
