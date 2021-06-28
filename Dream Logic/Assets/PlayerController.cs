using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform tr;
    private CharacterController cc;

    [SerializeField]
    private float forwardMoveSpeed;
    [SerializeField]
    private float rotationSpeed;

    private float rotationInput;

    private void Awake()
    {
        tr = transform;
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        rotationInput = Input.GetAxisRaw("Horizontal");

        cc.Move(tr.forward * forwardMoveSpeed * Time.deltaTime);
        tr.Rotate(tr.up, rotationSpeed * rotationInput * Time.deltaTime);
    }
}
