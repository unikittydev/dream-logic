using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private const float angleEpsilon = 5f;
    private const float posEpsilon = 1f;

    private Transform _tr;
    public Transform tr => _tr;

    private CharacterController _cc;
    public CharacterController cc => _cc;

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
        _cc = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        dashCoroutine = StartCoroutine(DashMove());
    }

    private void OnDisable()
    {
        StopCoroutine(dashCoroutine);
    }

    private void Update()
    {
        cc.Move((tr.forward * forwardMoveSpeed * forwardInput + Physics.gravity) * Time.deltaTime);
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
