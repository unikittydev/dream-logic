using UnityEngine;
using UnityEngine.Localization;

namespace Game.Dream
{
    /// <summary>
    /// Режим игры.
    /// </summary>
    public class DefaultDreamMode : MonoBehaviour
    {
        [SerializeField]
        private DreamModeFlag _mode;
        public DreamModeFlag mode => _mode;

        [SerializeField]
        private LocalizedString _description;
        public LocalizedString description => _description;
    }
}

