using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// Сложность сна.
    /// </summary>
    public class DreamDifficulty : MonoBehaviour
    {
        private float _playerSpeedMultiplier = 1f;
        public float playerSpeedMultiplier => _playerSpeedMultiplier;

        private float _objectSpawnFrequencyMultiplier = 1f;
        public float objectSpawnFrequencyMultiplier => _objectSpawnFrequencyMultiplier;

        private float _objectSpeedMultiplier = 1f;
        public float objectSpeedMultiplier => _objectSpeedMultiplier;

        private float _dreamDurationMultiplier = 1f;
        public float dreamDurationMultiplier => _dreamDurationMultiplier;

        [SerializeField]
        private DreamDifficultySettings settings;

        public void RaiseDifficulty()
        {
            _playerSpeedMultiplier = Mathf.Max(1f, _playerSpeedMultiplier + Random.Range(settings.playerSpeedRise.x, settings.playerSpeedRise.y));
            _objectSpawnFrequencyMultiplier = Mathf.Max(1f, _objectSpawnFrequencyMultiplier + Random.Range(settings.objectSpawnFrequencyRise.x, settings.objectSpawnFrequencyRise.y));
            _objectSpeedMultiplier = Mathf.Max(1f, _objectSpeedMultiplier + Random.Range(settings.objectSpeedRise.x, settings.objectSpeedRise.y));
            _dreamDurationMultiplier = Mathf.Max(1f, _dreamDurationMultiplier + Random.Range(settings.dreamDurationRise.x, settings.dreamDurationRise.y));
        }

        public void ResetDifficulty()
        {
            _playerSpeedMultiplier = _objectSpawnFrequencyMultiplier = _objectSpeedMultiplier = _dreamDurationMultiplier = 1f;
        }
    }
}
