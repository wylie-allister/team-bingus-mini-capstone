using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance;

    public InputActionReference pauseAction;

    public bool isGamePaused = false;

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

        // keep pause panel in sync
        if (UIController.Instance != null)
            UIController.Instance.pausePanel.SetActive(isGamePaused);
    }
}
