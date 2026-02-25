using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveDrag;

    [Header("Jumping")]
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] [Range(0.0f, 1.0f)] private float _airMultiplier;
    private bool _readyToJump;

    [Header("Ground Check")]
    [SerializeField] private float _playerHeight;
    [SerializeField] LayerMask _groundMask;
    private bool _isGrounded;
    public Transform orientation;
    private float horizontalInput, verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        _readyToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        GetInputs();
        ClampSpeed();

        rb.drag = (_isGrounded) ? _moveDrag : 0.0f;
    }

    void FixedUpdate()
    {
        Move();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        
        // grounding ray
        Gizmos.DrawRay(transform.position, Vector3.down * (_playerHeight * 0.5f + 0.2f));
    }

    private void Move()
    {
        // calc move direction (vec3): walk in the direction the player is looking
        moveDirection = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

        if(_isGrounded)
        {
            rb.AddForce(moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);        
        } else
        {
            rb.AddForce(moveDirection.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);        
        }
    }

    private void GetInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(KeyCode.Space) && _readyToJump && _isGrounded)
        {
            _readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), _jumpCooldown);
        }

    }

    private void GroundCheck()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.3f, _groundMask);
    }

    private void ClampSpeed()
    {
        Vector3 actualSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if(actualSpeed.magnitude > _moveSpeed) // clamp
        {
            Vector3 clampedSpeed = actualSpeed.normalized * _moveSpeed;
            rb.velocity = new Vector3(clampedSpeed.x, rb.velocity.y, clampedSpeed.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * _jumpPower, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _readyToJump = true;
    }
}
