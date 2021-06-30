using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// Логика сна.
    /// </summary>
    public abstract class DreamBehaviour : MonoBehaviour
    {
        [SerializeField]
        private DreamSettings _settings;
        public DreamSettings settings => _settings;

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }
    }
}
