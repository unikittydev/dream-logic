using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game
{
    /// <summary>
    /// ������� �������� ����
    /// </summary>
    public class MenuRoomButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private UnityEvent onClick;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!enabled)
                return;

            onClick.Invoke();
        }
    }
}
