using UnityEngine;

// Persistent singleton that holds cross-scene game state flags.
// Self-creates on first access - no scene setup needed.
public class GameState : MonoBehaviour
{
    private static GameState _instance;

    public static GameState Instance
    {
        get
        {
            // If no instance exists yet, create one automatically
            if (_instance == null)
            {
                GameObject go = new GameObject("GameState");
                _instance = go.AddComponent<GameState>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    // True when the player lost via 5 alert stars, false when time ran out normally
    public bool didPlayerLose = false;

    // Short description of why the session ended - set by GameManager before loading GameOver
    public string endReason = "";

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
