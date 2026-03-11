using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DEBUG : MonoBehaviour
{
    public static DEBUG Instance;
    
    public InputActionReference roarAction;
    public InputActionReference jumpAction;
    public InputActionReference debugAction;
    public InputActionReference pauseAction;

    private bool isRoar = false;
    private bool isJump = false;
    private bool isDebug = false;

    private bool isGamePaused = false;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        roarAction.action.started += ctx => isRoar = true;
        jumpAction.action.started += ctx => isJump = true;
        debugAction.action.started += ctx => isDebug = true;
        
        roarAction.action.canceled += ctx => isRoar = false;
        jumpAction.action.canceled += ctx => isJump = false;
        debugAction.action.canceled += ctx => isDebug = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (isGamePaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        
        
        if (pauseAction.action.WasPressedThisFrame())
        {
            isGamePaused = !isGamePaused;
        }
        
        
        if (isRoar && isJump && isDebug)
        {
            SceneManager.LoadScene("Splash");
        }
    }
}
