using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game
{
    /// <summary>
    /// Ёлемент главного меню
    /// </summary>
    public class MenuRoomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        private const float twinkSpeed = .25f;
        private static int twinkID = Shader.PropertyToID("Twink_Speed");

        private Material material;

        [SerializeField]
        private UnityEvent onClick;

        private void Awake()
        {
            material = GetComponentInChildren<Renderer>().material;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!enabled)
                return;

            material.SetFloat(twinkID, twinkSpeed);

            AudioManager.instance.Play("menu.over");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!enabled)
                return;

            material.SetFloat(twinkID, 0f);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!enabled)
                return;

            AudioManager.instance.Play("menu.press");
            onClick.Invoke();
        }
    }
}
