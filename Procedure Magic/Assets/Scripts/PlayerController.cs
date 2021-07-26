using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    private Transform tr;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotationSpeed;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        tr = transform;
    }

    private void Update()
    {
        tr.Rotate(tr.up, rotationSpeed * Input.GetAxisRaw("Horizontal") * Time.deltaTime);
        cc.Move(tr.forward * moveSpeed * Input.GetAxisRaw("Vertical") * Time.deltaTime);
    }
}
