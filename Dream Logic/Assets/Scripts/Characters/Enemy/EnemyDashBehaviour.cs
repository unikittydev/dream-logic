using System.Collections;
using UnityEngine;

namespace Game
{
    public class EnemyDashBehaviour : EnemyBehaviour
    {
        private Transform tr;

        private const float angleEpsilon = 5f;
        private const float posEpsilon = 1f;

        private Coroutine dash;

        [SerializeField]
        private float maxDashDistance;

        protected override void Awake()
        {
            base.Awake();

            tr = transform;
        }

        private void OnEnable()
        {
            dash = StartCoroutine(DashMove());
        }

        private void OnDisable()
        {
            StopCoroutine(dash);
        }

        private IEnumerator DashMove()
        {
            while (enabled)
            {
                float targetAngle = Random.Range(-180f, 180f);
                ec.rotationInput = Mathf.Sign(Mathf.DeltaAngle(tr.rotation.eulerAngles.y, targetAngle));

                while (Mathf.Abs(Mathf.DeltaAngle(tr.rotation.eulerAngles.y, targetAngle)) > angleEpsilon)
                {
                    yield return null;
                }
                ec.rotationInput = 0f;

                Vector3 targetPosition = tr.position + tr.forward * Random.Range(0f, maxDashDistance);
                ec.forwardInput = 1f;

                while ((targetPosition - tr.position).sqrMagnitude > posEpsilon)
                {
                    yield return null;
                }
                ec.forwardInput = 0f;
            }
        }
    }
}
