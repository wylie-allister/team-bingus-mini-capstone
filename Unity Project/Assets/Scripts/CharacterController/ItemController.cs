using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemController : MonoBehaviour
{
    [Header("Input System")]
    public InputActionAsset InputActions;
    private InputAction m_throwAction;
    
    [Header("Current Throwable")]
    public ThrowableObject currentThrowable;
    public Transform throwTarget;

    [Header("Throw Settings")]
    private float throwForce = 0.0f;
    public float throwForceMax = 5.0f;
    public float forceIncrease = 0.2f;
    private bool isThrowing = false;

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
        m_throwAction = InputActions.FindAction("Throw");
        
        InputActions.FindAction("Throw").started += ctx => isThrowing = true;
        InputActions.FindAction("Throw").canceled += ctx => isThrowing = false;
    }
    

    void Update()
    {
        // Don't attempt throw if no throwable object exists
        if (currentThrowable.data.tag == ThrowableObjectTag.UNTAGGED)
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
        Debug.Log($"ITEM THROWN WITH FORCE: {throwForce}");
        
        // Throw item, reset force, disable isThrowing
        throwForce = 0.0f;
        isThrowing = false;
    }

    void ChargeThrow()
    {
        Debug.Log($"THROWCHARGE: {throwForce}");
        
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
}
