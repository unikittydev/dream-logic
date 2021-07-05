using UnityEngine;

namespace Game
{
    /// <summary>
    /// Камера главного меню.
    /// </summary>
    public class MenuRoomCamera : MonoBehaviour
    {
        private Quaternion startRotation;

        [SerializeField]
        private float maxOffset;

        private void Awake()
        {
            startRotation = transform.rotation;
        }

        private void Update()
        {
            Vector2 viewport = GetViewportPosition();
            Quaternion rotation = Quaternion.Euler(-viewport.y * maxOffset, viewport.x * maxOffset, 0f);
            transform.rotation = startRotation * rotation;
        }

        private Vector2 GetViewportPosition()
        {
            return Vector2.ClampMagnitude(new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height) - Vector2.one * .5f, 1f);
        }
    }
}
