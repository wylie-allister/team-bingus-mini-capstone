using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoarController : MonoBehaviour
{
    [Header("Input")] 
    public InputActionAsset InputActions;
    private InputAction m_roarAction;
    
    [Header("Roar Mechanic")] 
    private bool canRoar = false;
    private bool hasAttemptedRoar = false;
    public float roarCharge = 0.0f;
    public float roarChargeRate = 1.0f;
    public float roarMaxCharge = 30.0f;
    private float roarRange = 10.0f;

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
        m_roarAction = InputActions.FindAction("Roar");
        
        InputActions.FindAction("Roar").started += ctx => hasAttemptedRoar = true;
        InputActions.FindAction("Roar").canceled += ctx => hasAttemptedRoar = false;
    }

    void Update()
    {
        // If roar is charged, player can roar
        if (roarCharge >= roarMaxCharge)
        {
            canRoar = true;
            roarCharge = roarMaxCharge;
        }
        
        // If player cannot roar, charge roar
        if (!canRoar)
        {
            roarCharge += roarChargeRate * Time.deltaTime;
        }

        // If player can roar, and attempts to roar, roar
        if (canRoar && hasAttemptedRoar)
        {
            // TBD -------- 
            Debug.Log("PLAYER ROARED");
            roarCharge = 0;
            canRoar = false;
        }
        
        
    }
}
