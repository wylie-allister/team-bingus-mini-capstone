using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemController : MonoBehaviour
{
    [Header("Input System")]
    public InputActionAsset InputActions;
    private InputAction m_interactAction;
    
    [Header("Current Throwable")]
    public ThrowableObject currentThrowable;
    public Transform throwTarget;

    [Header("Throw Settings")]
    public float throwForce = 0.0f;
    public float throwForceMax = 5.0f;
    public float forceIncrease = 0.2f;
    public Transform throwHolder;
    
    public bool isThrowing
    {
        get;
        private set;
    }

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

    // Get reference to throw action on script awake
    // Bind throw context to isThrowing bool
    private void Awake()
    {
        // Create thro
        InputActions.FindAction("Throw").started += ctx => isThrowing = true;
        InputActions.FindAction("Throw").canceled += ctx => isThrowing = false;
        m_interactAction = InputActions.FindAction("Interact");
    }

    private void Start()
    {
        isThrowing = false;
    }
    
    void Update()
    {
        // Don't attempt throw if no throwable object exists
        if (currentThrowable == null)
        {
            return;
        }
        
        HandleThrow();
    }

    void HandleThrow()
    {
        // If player is holding throw, charge throw
        // Else if player lets go of throw mid charge, throw object
        if (isThrowing)
        {
            ChargeThrow();
        }
        else
        {
            if (throwForce != 0.0f)
            {
                Throw();
            }
        }
    }
    
    void Throw()
    {
        // Remove throwable parent, enable mesh rendering
        currentThrowable.transform.parent = null;
        currentThrowable.EnableMesh();
        
        // Get the rigidbody of the throwable and add throwforce impule
        currentThrowable.GetComponent<Rigidbody>().AddForce(this.transform.forward * (throwForce * 750 * Time.deltaTime), ForceMode.Impulse);
        currentThrowable.hasBeenThrown = true;
        currentThrowable = null;
        
        // Reset throw force and set is throwing to true
        throwForce = 0.0f;
        isThrowing = false;
    }

    void ChargeThrow()
    {
        // If throw force is not at max, increase throw force
        if (throwForce < throwForceMax)
        {
            throwForce += forceIncrease * Time.deltaTime;
        }
        else
        {
            // If above max, set to max and throw
            throwForce = throwForceMax;
            Throw();
        }
    }

    // If player with this script enters a throwable object trigger, checkpickup()
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Throwable"))
        {
            CheckPickup(other.gameObject);
        }
    }

    private void CheckPickup(GameObject item)
    {
        // If the player presses interact
        if (m_interactAction.WasPressedThisFrame())
        {
            // If there is a current throwable
            if (currentThrowable != null)
            {
                // Set current throwable parent to nothing
                currentThrowable.transform.parent = null;
                
                // Enable the mesh
                currentThrowable.EnableMesh();
                
                // Set throwable to null
                currentThrowable = null;
            }
            
            // If there is now no throwable, get the throwable object
            // Set position to throwholder position
            // Disable visable rendering of the mesh
            // Set parent to throwholder
            currentThrowable = item.transform.parent.GetComponent<ThrowableObject>();
            currentThrowable.transform.position = throwHolder.transform.position;
            currentThrowable.DisableMesh();
            currentThrowable.transform.SetParent(throwHolder);
        }
    }
}
