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
        private PlayerInput input;

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

            input = new PlayerInput();
            input.Player.Rotate.performed += ctx => rotationInput = ctx.ReadValue<float>();
            input.Player.Rotate.canceled += _ => rotationInput = 0f;

            input.Player.Jump.performed += ctx =>
            {
                if (!jumping && jumpHeight > 0f)
                {
                    StartCoroutine(Jump());
                }
            };
        }

        private void OnEnable()
        {
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
        }

        private void Update()
        {
            Vector3 motion = (tr.forward * forwardMoveSpeed * DreamSimulation.difficulty.playerSpeedMultiplier + (jumping ? Vector3.zero : Physics.gravity)) * Time.deltaTime;
            cc.Move(motion);
            tr.Rotate(tr.up, rotationSpeed * rotationInput * DreamSimulation.difficulty.playerSpeedMultiplier * Time.deltaTime);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            DreamSimulation.onPlayerHit.Invoke(this, hit);
        }

        private IEnumerator Jump()
        {
            jumping = true;

            float startHeight = tr.position.y;

            AudioManager.instance.Play("jump");

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

        public void InstantMove(Vector3 position)
        {
            cc.enabled = false;
            tr.position = position;
            cc.enabled = true;
        }
    }
}
