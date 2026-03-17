using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Dev cheat: hold Roar + Jump + Debug simultaneously to reload to Splash.
// Stays DontDestroyOnLoad so the cheat works from any scene.
public class DEBUG : MonoBehaviour
{
    public static DEBUG Instance;

    public InputActionReference roarAction;
    public InputActionReference jumpAction;
    public InputActionReference debugAction;

    private bool isRoar = false;
    private bool isJump = false;
    private bool isDebug = false;

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

    void Start()
    {
        roarAction.action.started += ctx => isRoar = true;
        jumpAction.action.started += ctx => isJump = true;
        debugAction.action.started += ctx => isDebug = true;

        roarAction.action.canceled += ctx => isRoar = false;
        jumpAction.action.canceled += ctx => isJump = false;
        debugAction.action.canceled += ctx => isDebug = false;
    }

    void Update()
    {
        if (isRoar && isJump && isDebug)
        {
            SceneManager.LoadScene("Splash");
        }
    }
}
