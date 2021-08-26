using Game.Dream;
using System.Collections;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Контроллер противника.
    /// </summary>
    public class EnemyController : MonoBehaviour
    {
        private Transform tr;
        private Rigidbody rb;

        [SerializeField]
        private float forwardMoveSpeed;
        public float forwardInput { get; set; }

        [SerializeField]
        private float rotationSpeed;
        public float rotationInput { get; set; }

        private void Awake()
        {
            tr = transform;
            rb = GetComponent<Rigidbody>();
        }


        private void FixedUpdate()
        {
            rb.MovePosition(tr.position + tr.forward * forwardMoveSpeed * forwardInput * DreamSimulation.difficulty.objectSpeedMultiplier * Time.fixedDeltaTime);
            rb.MoveRotation(tr.rotation * Quaternion.AngleAxis(rotationSpeed * rotationInput * DreamSimulation.difficulty.objectSpeedMultiplier * Time.fixedDeltaTime, tr.up));
        }
    }
}
