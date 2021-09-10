using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameLoadingScreen : MonoBehaviour
    {
        [SerializeField]
        private Image _background;
        public Image background => _background;
    }
}

