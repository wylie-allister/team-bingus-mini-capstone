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
    private bool isColliderEnabled = false;
    public float roarCharge = 0.0f;
    public float roarChargeRate = 1.0f;
    public float roarMaxCharge = 30.0f;
    private float roarRange = 10.0f;
    public GameObject roarCollider;
    
    [Header("Audio")]
    public AudioClip roarSound;

    private float roarColliderTimer = 0.0f;

    private void OnEnable()
    {
        // Enable input map
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        // Disable input map
        //InputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        // Find roar action
        m_roarAction = InputActions.FindAction("Roar");
        
        // Bind bool to input actions
        InputActions.FindAction("Roar").started += ctx => hasAttemptedRoar = true;
        InputActions.FindAction("Roar").canceled += ctx => hasAttemptedRoar = false;
        
        roarCollider.GetComponent<SphereCollider>().radius = roarRange;
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
            if (AudioController.Instance != null)
            {
                AudioController.Instance.PlaySoundClip(roarSound, 0.6f, 1);
            }
            
            isColliderEnabled = true;
            roarCharge = 0;
            canRoar = false;
        }

        HandleRoarCollider();
    }

    void HandleRoarCollider()
    {
        roarCollider.SetActive(isColliderEnabled);
        
        if (isColliderEnabled)
        {
            roarColliderTimer += Time.deltaTime;

            if (roarColliderTimer >= 0.5f)
            {
                isColliderEnabled = false;
                roarColliderTimer = 0.0f;
            }
        }
    }
}
