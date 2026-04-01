using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private InputActionReference jumpAction;
    public VideoPlayer videoPlayer;
    public Animator canvasAnimator;
    public string nextSceneName = "";
    public float cutsceneDuration = 5.0f;
    private float timer = 0.0f;
    private float tempTimer = 0.0f;

    private bool _hasPlayerSkipped = false;

    private void Start()
    {
    }

    private void Update()
    {
        // Increment timer
        timer += Time.deltaTime;
        
        // If player skips, set has skipped to true
        if (jumpAction.action.WasPressedThisFrame())
        {
            _hasPlayerSkipped = true;
        }

        // If player has skipped, handle skip and break from logic
        if (_hasPlayerSkipped)
        {
            HandleSkip();
            return;
        }
        
        // Buffer time for circle wipe
        if (timer > 0.35f)
        {
            canvasAnimator.SetBool("StartGameplay", true);
        }
        
        // Exit circle wipe
        if (timer > cutsceneDuration)
        {
            canvasAnimator.SetBool("StartEndScene", true);
        }
        
        // Buffer time for exit circle wipe
        if (timer > cutsceneDuration + 1.5f)
        {
            SwapScene();
        }
    }

    private void HandleSkip()
    {
        // New timer ticks up, start end circle wipe
        tempTimer += Time.deltaTime;
        canvasAnimator.SetBool("StartEndScene", true);
        
        // Buffer time for end circle wipe
        if (tempTimer > 1.5f)
        {
            SwapScene();
        }
    }

    private void SwapScene()
    {
        //SceneController.Instance.GoTo
        SceneManager.LoadScene(nextSceneName);
    }
}
