using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    public Transform orientation;
    float horizontalInput, verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    [SerializeField] private float _jumpPower;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // calc move direction (vec3): walk in the direction the player is looking
        moveDirection = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

        rb.AddForce(moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
    }

    private void GetInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
}
