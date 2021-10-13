using UnityEngine;
using UnityEngine.UI;

namespace Game.Menu
{
    public class MenuNavigationSwitcher : MonoBehaviour
    {
        [SerializeField]
        private Graphic[] navigationElements;

        [SerializeField]
        private float _switchTime;
        public float switchTime => _switchTime;

        public void SwitchNavigationUI(bool enable)
        {
            foreach (var element in navigationElements)
                GameUI.FadeUI(element, enable, 1f, switchTime);
        }
    }
}
