using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// ����� ����.
    /// </summary>
    public class DefaultDreamMode : MonoBehaviour
    {
        [SerializeField]
        private DreamModeFlag _mode;
        public DreamModeFlag mode => _mode;

        [SerializeField]
        private string _description;
        public string description => _description;
    }
}

