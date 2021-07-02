using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// Параметры сна.
    /// </summary>
    [CreateAssetMenu(fileName = "New Dream Difficulty", menuName = "Dream/Difficulty")]
    public class DreamDifficultySettings : ScriptableObject
    {
        public Vector2 playerSpeedRise;
        public Vector2 objectSpawnFrequencyRise;
        public Vector2 objectSpeedRise;
        public Vector2 dreamDurationRise;
    }
}
