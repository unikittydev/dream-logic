using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform tr;
    private Rigidbody rb;

    [SerializeField]
    private float forwardMoveSpeed;
    [SerializeField]
    private float rotationSpeed;

    private float rotationInput;

    private void Awake()
    {
        tr = transform;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rotationInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        rb.AddForce(tr.forward * forwardMoveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        rb.AddTorque(tr.up * rotationSpeed * rotationInput * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}
