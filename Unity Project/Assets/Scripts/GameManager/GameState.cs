using UnityEngine;

// Persistent singleton that holds cross-scene game state flags.
// Attach to the DEBUG prefab (or any DontDestroyOnLoad object).
public class GameState : MonoBehaviour
{
    public static GameState Instance;

    // True when the player lost via 5 alert stars, false when time ran out normally
    public bool didPlayerLose = false;

    // Short description of why the session ended - set by GameManager before loading GameOver
    public string endReason = "";

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
}
