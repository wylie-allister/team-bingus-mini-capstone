using UnityEngine;
using UnityEngine.InputSystem;

// Handles pausing, time scale, and cursor lock.
// Attach to any scene object in the Gameplay scene - does not need to persist.
public class PauseController : MonoBehaviour
{
    public static PauseController Instance;

    public InputActionReference pauseAction;

    public bool isGamePaused { get; private set; } = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (pauseAction.action.WasPressedThisFrame())
        {
            isGamePaused = !isGamePaused;
        }

        if (isGamePaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            if (Cursor.lockState != CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.Locked;

            Time.timeScale = 1f;
        }

        // Keep pause panel in sync with pause state
        if (UIController.Instance != null)
            UIController.Instance.pausePanel.SetActive(isGamePaused);
    }
}
