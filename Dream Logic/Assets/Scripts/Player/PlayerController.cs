using Game.Dream;
using System.Collections;
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

        [Header("Jumping")]
        [SerializeField]
        private float jumpStayTime;
        [SerializeField]
        private float jumpHeight;

        private bool jumping;

        private void Awake()
        {
            _tr = transform;
            _cc = GetComponent<CharacterController>();
        }

        private void Update()
        {
            rotationInput = Input.GetAxisRaw("Horizontal");

            Vector3 motion = (tr.forward * forwardMoveSpeed * DreamSimulation.difficulty.playerSpeedMultiplier + (jumping ? Vector3.zero : Physics.gravity)) * Time.deltaTime;
            cc.Move(motion);

            tr.Rotate(tr.up, rotationSpeed * rotationInput * DreamSimulation.difficulty.playerSpeedMultiplier * Time.deltaTime);

            if (!jumping && cc.isGrounded && Input.GetKeyDown(KeyCode.Space) && jumpHeight > 0f)
            {
                StartCoroutine(Jump());
            }
        }

        private IEnumerator Jump()
        {
            jumping = true;

            float startHeight = tr.position.y;

            // Rise
            while (tr.position.y < startHeight + jumpHeight)
            {
                cc.Move(-Physics.gravity * Time.deltaTime);
                yield return null;
            }
            // Stay
            yield return new WaitForSeconds(jumpStayTime);
            // Fall
            jumping = false;
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
