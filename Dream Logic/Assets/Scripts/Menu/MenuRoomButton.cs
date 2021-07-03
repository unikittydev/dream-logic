using UnityEngine;
using UnityEngine.Events;
namespace Game
{
    /// <summary>
    /// Ёлемент главного меню
    /// </summary>
    public class MenuRoomButton : MonoBehaviour
    {
        private const float twinkSpeed = .25f;
        private const string twinkKeyword = "Twink_Speed";

        private Material material;

        public string test;

        [SerializeField]
        private UnityEvent onClick;

        private void Awake()
        {
            material = GetComponentInChildren<Renderer>().material;
        }

        private void OnMouseOver()
        {
            material.SetFloat(twinkKeyword, twinkSpeed);
        }

        private void OnMouseExit()
        {
            material.SetFloat(twinkKeyword, 0f);
        }

        private void OnMouseDown()
        {
            onClick.Invoke();
        }
    }
}
