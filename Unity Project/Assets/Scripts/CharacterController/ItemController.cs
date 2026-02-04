using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemController : MonoBehaviour
{
    public InputActionAsset InputActions;
    private InputAction m_throwAction;
    
    public ThrowableObject currentThrowable;

    public Transform throwTarget;

    private float throwForce = 0.0f;
    public float throwForceMax = 5.0f;
    public float forceIncrease = 0.2f;
    private bool isThrowing = false;

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        m_throwAction = InputActions.FindAction("Throw");
        
        InputActions.FindAction("Throw").started += ctx => isThrowing = true;
        InputActions.FindAction("Throw").canceled += ctx => isThrowing = false;
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        if (isThrowing)
        {
            ChargeThrow();
        }
    }

    void Throw()
    {
        
    }

    void ChargeThrow()
    {
        if (throwForce > throwForceMax)
        {
            throwForce += forceIncrease * Time.deltaTime;
        }
        
        
        
    }
}
