using Game.Dream;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Бегущий вперёд игрок.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        private Transform _tr;
        public Transform tr => _tr;

        private CharacterController _cc;
        public CharacterController cc => _cc;

        [SerializeField]
        private float forwardMoveSpeed;
        public float speed => forwardMoveSpeed;

        [SerializeField]
        private float rotationSpeed;

        private float rotationInput;

        private void Awake()
        {
            _tr = transform;
            _cc = GetComponent<CharacterController>();
        }

        private void Update()
        {
            rotationInput = Input.GetAxisRaw("Horizontal");

            cc.Move((tr.forward * forwardMoveSpeed + Physics.gravity) * Time.deltaTime);
            tr.Rotate(tr.up, rotationSpeed * rotationInput * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(GameTags.enemy))
            {
                DreamSimulation.WakeUp();
            }
        }
    }
}
