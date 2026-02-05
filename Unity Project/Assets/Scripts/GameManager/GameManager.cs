using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player References")]
    public GameObject player;
    public ItemController itemController;

    [Header("Global Timer")] 
    public float timeRemaining;

    public float maxGameTime = 60.0f;
    
    
    // Create singleton instance for this object
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        // If player reference is not manually set, find player object
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        // Get player item controller
        itemController = player.GetComponent<ItemController>();
        
        // Set game timer to max
        timeRemaining = maxGameTime;
        

    }

    void Update()
    {
        HandleGameTimer();
    }

    void HandleGameTimer()
    {
        if (timeRemaining >= 0 )
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            // TBD --- PLACE HOLDER
            Debug.Log("GAME OVER");
        }
    }
}
