using UnityEngine;
using UnityEngine.InputSystem;

// This script is to be placed on the Main Camera underneath the Player gameobject
public class PlayerCamera : MonoBehaviour
{
        public InputActionAsset InputActions;
        private InputAction m_lookAction;
        private Vector2 m_lookAmt;
        private Vector2 mouseLook;
        private Vector2 smoothMovement;
        
        private GameObject player;
        
        public float lookSensitivity = 5.0f;
        public float smoothing = 1.5f;

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

        // Get reference to player object (parent of camera)
        private void Start()
        {
                player = transform.parent.gameObject;
        }

        private void Update()
        {
                // Lock Cursor
                Cursor.lockState = CursorLockMode.Locked;
                
                // Read and store value of look input action
                m_lookAmt = m_lookAction.ReadValue<Vector2>();
                
                // Create new mouseDirection vector -------- This can probably be deleted -bc
                Vector2 mouseDirection = new Vector2(m_lookAmt.x, m_lookAmt.y);

                // Multiply the mouseDirection vector by sensitivity and smoothing amount
                mouseDirection.x *= lookSensitivity * smoothing;
                mouseDirection.y  *= lookSensitivity * smoothing;
                
                // Lerp smooth movement vector and mouse direction vector by smoothing amount
                smoothMovement.x = Mathf.Lerp(smoothMovement.x, mouseDirection.x, 1f / smoothing);
                smoothMovement.y = Mathf.Lerp(smoothMovement.y, mouseDirection.y, 1f / smoothing);

                // Add smoothing vector to mouse look vector
                mouseLook += smoothMovement;
                
                // Clamp vertical camera movement
                mouseLook.y = Mathf.Clamp(mouseLook.y, -80, 90f);

                // Apply rotation to camera transform
                transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);

                // Rotate player to match camera rotation
                player.transform.rotation = Quaternion.AngleAxis(mouseLook.x, player.transform.up);
        }
}
