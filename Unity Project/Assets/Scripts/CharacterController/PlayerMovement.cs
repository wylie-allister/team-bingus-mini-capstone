using UnityEngine;
using UnityEngine.InputSystem;

// This script is to be placed on the Player gameobject
public class PlayerMovement : MonoBehaviour
{

    public InputActionAsset InputActions;
    private InputAction m_moveAction;
    private Rigidbody m_rb;

    private Vector2 m_moveAmt;
    private Vector3 movement;

    public float walkSpeed = 5.0f;
    public float strafeSpeed = 4.0f;
    public float walkSpeedOffset = 0.5f;

    private bool isGrounded;
    public LayerMask groundLayer;

    // Enable Input Actions Map
    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    // Disable Input Actions Map
    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    // Get reference to move action on script awake
    private void Awake()
    {
        m_moveAction = InputActions.FindAction("Move");
        m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Read and store value of movement input action
        m_moveAmt = m_moveAction.ReadValue<Vector2>();

        // Check for ground collision -- Usually required for jump mechanics, we may or may not use this -bc
        isGrounded = GroundCheck();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    // Cast ray below player position to check for ground beneath player
    private bool GroundCheck()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, Vector3.down, out raycastHit, 2.0f, groundLayer))
        {
            return true;
        }

        return false;
    }
    
    private void HandleMovement()
    {
        
        float h = m_moveAmt.x;
        
        // Clamp backwards movement for slower backwards speed, adjust as necessary -bc
        float v = Mathf.Clamp(m_moveAmt.y, -0.5f, 1.0f);
        Vector3.Normalize(movement);

        // Apply movement input vector to movement var
        movement = (transform.forward * v * walkSpeed * walkSpeedOffset) +
                   (transform.right * h * strafeSpeed * walkSpeedOffset);
        
        // Translate rigidbody by given position - We will probably want to swap to velocity based, but this is fine
        // for now -bc
        m_rb.MovePosition(transform.position + movement * Time.deltaTime);
    }
    
}
