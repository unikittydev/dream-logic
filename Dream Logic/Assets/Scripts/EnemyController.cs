using System.Collections;
using UnityEngine;

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

    private void OnEnable()
    {
        dashCoroutine = StartCoroutine(DashMove());
    }

    private void OnDisable()
    {
        StopCoroutine(dashCoroutine);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(tr.position + tr.forward * forwardMoveSpeed * forwardInput * Time.fixedDeltaTime);
    }

    private void Update()
    {
        tr.Rotate(tr.up, rotationSpeed * rotationInput * Time.deltaTime);
    }

    private IEnumerator DashMove()
    {
        while (true)
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
