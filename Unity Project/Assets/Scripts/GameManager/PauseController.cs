using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance;

    public InputActionReference pauseAction;
    public InputActionReference roarAction;
    public Slider menuReturnSlider;

    public bool isGamePaused = false;
    private bool isRoarHeld = false;

    private float radialSliderValue = 0.0f;
    public float returnSpeedScalar = 0.5f;

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
        
        roarAction.action.started += ctx => isRoarHeld = true;
        roarAction.action.canceled += ctx => isRoarHeld = false;
    }

    void Update()
    {
        if (pauseAction.action.WasPressedThisFrame())
        {
            isGamePaused = !isGamePaused;
        }

        if (isGamePaused)
        {
            if (Cursor.lockState != CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.Locked;
            
            Time.timeScale = 0f;
            HandleMenuReturn();
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

    private void HandleMenuReturn()
    {
        if (isRoarHeld)
        {
            radialSliderValue += 0.001666666f * returnSpeedScalar;
            if (radialSliderValue > 1.0f)
            {
                Time.timeScale = 1.0f;
                SceneController.Instance.GoToStartMenu();
            }
        }
        else
        {
            radialSliderValue = 0.0f;
        }
        
        menuReturnSlider.value = radialSliderValue;
    }
}
