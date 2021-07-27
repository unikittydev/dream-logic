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
        private const float angleEpsilon = 5f;
        private const float posEpsilon = 1f;

        private Transform _tr;
        public Transform tr => _tr;

        private Rigidbody _rb;
        public Rigidbody rb => _rb;

        private Coroutine dashCoroutine;

        [SerializeField]
        private float forwardMoveSpeed;
        private float forwardInput;

        [SerializeField]
        private float rotationSpeed;
        private float rotationInput;

        [SerializeField]
        private float maxDashDistance;

        private void Awake()
        {
            _tr = transform;
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            dashCoroutine = StartCoroutine(DashMove());
        }

        private void OnDestroy()
        {
            StopCoroutine(dashCoroutine);
        }

        private void FixedUpdate()
        {
            rb.MovePosition(tr.position + tr.forward * forwardMoveSpeed * forwardInput * DreamSimulation.difficulty.objectSpeedMultiplier * Time.fixedDeltaTime);
            rb.MoveRotation(tr.rotation * Quaternion.AngleAxis(rotationSpeed * rotationInput * DreamSimulation.difficulty.objectSpeedMultiplier * Time.fixedDeltaTime, tr.up));
        }

        private IEnumerator DashMove()
        {
            while (enabled)
            {
                float desiredAngle = Random.Range(-180f, 180f);
                rotationInput = Mathf.Sign(Mathf.DeltaAngle(tr.rotation.eulerAngles.y, desiredAngle));

                while (Mathf.Abs(Mathf.DeltaAngle(tr.rotation.eulerAngles.y, desiredAngle)) > angleEpsilon)
                {
                    yield return null;
                }
                rotationInput = 0f;

                Vector3 desiredPosition = tr.position + tr.forward * Random.Range(0f, maxDashDistance);
                forwardInput = 1f;

                while ((desiredPosition - tr.position).sqrMagnitude > posEpsilon)
                {
                    yield return null;
                }
                forwardInput = 0f;
            }
        }
    }
}
