using UnityEngine;
using UnityEngine.InputSystem;

// This script is to be placed on the Player gameobject
public class PlayerMovement : MonoBehaviour
{

    public InputActionAsset InputActions;
    private InputAction m_moveAction;
    private InputAction m_sprintAction;
    
    private Rigidbody m_rb;

    private Vector2 m_moveAmt;
    private Vector3 movement;

    public float walkSpeed = 5.0f;
    public float sprintSpeed = 11.0f;   // bumped from 9 for a more satisfying sprint
    private float currentSpeed;
    public float strafeSpeed = 4.5f;    // bumped slightly from 4
    public float walkSpeedOffset = 0.5f;
    public bool isSprinting = false;
    public bool canSprint = true;

    public float currentStamina { get; private set; }
    public float maxStamina = 30.0f;
    public float sprintStaminaTax = 5.0f;   // lowered from 7.5 so sprint lasts ~6s instead of 4s
    public float sprintRegenRate = 5.0f;

    public AudioClip pantSound;
    private bool isPanting = false;
    public AudioClip walkSound;
    public AudioClip runSound;

    private bool isPlayingFootstep = false;
    public float footstepInterval = 0.45f;
    public float sprintFootstepInterval = 0.28f;
    private float footstepTimer = 0.0f;

    private bool isGrounded;
    public LayerMask groundLayer;

    public Transform cam;

    public GameObject sprintHighlight;

    // Enable Input Actions Map
    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    // Disable Input Actions Map
    private void OnDisable()
    {
        //InputActions.FindActionMap("Player").Disable();
    }

    // Get reference to move action and rigidbody on script awake
    private void Awake()
    {
        m_moveAction = InputActions.FindAction("Move");

        InputActions.FindAction("Sprint").started += ctx => isSprinting = true;
        InputActions.FindAction("Sprint").canceled += ctx => isSprinting = false;
        
        m_rb = GetComponent<Rigidbody>();
        currentSpeed = walkSpeed;
        currentStamina = maxStamina;
        
    }

    private void Update()
    {
        // Read and store value of movement input action
        m_moveAmt = m_moveAction.ReadValue<Vector2>();
        if (m_moveAmt == Vector2.zero)
        {
            this.m_rb.angularVelocity = Vector3.zero;
        }
        // Check for ground collision -- Usually required for jump mechanics, we may or may not use this -bc
        isGrounded = GroundCheck();
    }

    private void FixedUpdate()
    {
        HandleSprint();
        HandleMovement();
        HandleFootsteps();
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

    private void HandleSprint()
    {
        if (!canSprint && isPanting)
        {
            AudioController.Instance.PlaySoundClip(pantSound, 0.2f, 1);
            isPanting = false;
        }
        
        //if (!isSprinting)
        //{
            if (currentStamina < maxStamina)
            {
                currentStamina += sprintRegenRate * Time.deltaTime;
            sprintHighlight.SetActive(false);
            }
            else
            {
                currentStamina = maxStamina;
                canSprint = true;
            sprintHighlight.SetActive(true);
            }
            
       // }
        
        if (isSprinting && canSprint)
        {
            currentStamina -= sprintStaminaTax * Time.deltaTime;
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
        
        if (currentStamina <= 0.0f && canSprint)
        {
            isPanting = true;
            canSprint = false;
        }
    }
    
    
    private void HandleFootsteps()
    {
        // No footsteps if not moving or not on the ground
        if (m_moveAmt == Vector2.zero || !isGrounded)
        {
            footstepTimer = 0.0f;
            return;
        }

        footstepTimer += Time.deltaTime;

        float interval = isSprinting && canSprint ? sprintFootstepInterval : footstepInterval;

        if (footstepTimer >= interval)
        {
            AudioClip clip = isSprinting && canSprint ? runSound : walkSound;
            if (AudioController.Instance != null)
                AudioController.Instance.PlaySoundClip(clip, 0.3f, 1);

            footstepTimer = 0.0f;
        }
    }

    private void HandleMovement()
    {
        float h = m_moveAmt.x;
        
        // Clamp backwards movement for slower backwards speed, adjust as necessary -bc
        float v = Mathf.Clamp(m_moveAmt.y, -0.5f, 1.0f);
        
        
        Vector3 movement = cam.transform.right * h * strafeSpeed + cam.transform.forward * v * currentSpeed;
        movement.y = 0.0f;

        if (movement.magnitude != 0f)
        {
            transform.Rotate(Vector3.up * h * cam.GetComponent<CameraScript>().sensitivity * Time.deltaTime);
            Quaternion camRot = cam.rotation;
            camRot.x = 0.0f;
            camRot.z = 0.0f;

            transform.rotation = Quaternion.Lerp(transform.rotation, camRot, 0.2f);
        }
        
        // Translate rigidbody by given position - We will probably want to swap to velocity based, but this is fine
        // for now -bc
        m_rb.MovePosition(transform.position + movement * Time.deltaTime);
    }
}
