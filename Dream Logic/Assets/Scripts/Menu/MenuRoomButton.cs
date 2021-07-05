using Game.Dream;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    /// <summary>
    /// Ёлемент главного меню
    /// </summary>
    public class MenuRoomButton : MonoBehaviour
    {
        private static MenuRoomUI roomUI;
        
        private const float twinkSpeed = .25f;
        private const string twinkKeyword = "Twink_Speed";

        public bool buttonEnabled { get; set; } = true;

        private Material material;

        [SerializeField]
        private UnityEvent onClick;
        [SerializeField]
        private string hintText;

        private void Awake()
        {
            if (roomUI == null)
                roomUI = FindObjectOfType<MenuRoomUI>();

            material = GetComponentInChildren<Renderer>().material;
        }

        private void OnMouseEnter()
        {
            if (!buttonEnabled)
                return;

            roomUI.ShowNavigationText(hintText);
            material.SetFloat(twinkKeyword, twinkSpeed);
        }

        private void OnMouseExit()
        {
            if (!buttonEnabled)
                return;

            roomUI.navigationText.gameObject.SetActive(false);
            material.SetFloat(twinkKeyword, 0f);
        }

        private void OnMouseDown()
        {
            if (!buttonEnabled)
                return;

            onClick.Invoke();
        }
    }
}
