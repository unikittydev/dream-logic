using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// ������ ���.
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
