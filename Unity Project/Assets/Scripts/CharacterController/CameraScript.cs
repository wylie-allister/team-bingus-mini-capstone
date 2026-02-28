using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    public InputActionAsset InputActions;
    private InputAction m_lookAction;
    private Vector2 m_lookAmt;
    
    public Transform target;
    public float distance = 10.0f;
    public float sensitivity = 1.0f;

    private float currentX = 0;
    private float currentY = 0;
    
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

    // Get reference to look action on script awake
    private void Awake()
    {
        m_lookAction = InputActions.FindAction("Look");
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        m_lookAmt = m_lookAction.ReadValue<Vector2>();

        currentX += m_lookAmt.x;
        currentY -= m_lookAmt.y;
        currentY = Mathf.Clamp(currentY, -24, 65);


    }
    
    void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rot = Quaternion.Euler(currentY, currentX, 0);
        
        transform.position = target.position + rot * dir;
        transform.LookAt(target.position);
    }
}
