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

    [Header("Roar Mechanic")] 
    public RoarController roarController;

    [Header("Trees")]
    public List<GameObject> trees;
    
    [Header("Terrain Dev")]
    public TerrainGenerator terrainGenerator;

    public GameObject throwableCanPrefab;
    public GameObject throwableParent;
    public int throwableCount = 50;
    private List<GameObject> throwables =  new List<GameObject>();

    [Header("Alert Mechanic")] 
    public int activeAlertStars = 0;


    
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
        roarController = player.GetComponent<RoarController>();
        
        // Set game timer to max
        timeRemaining = maxGameTime;

        // Start day ambience
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayDayAmbience();

        // Create new throwables based on throwable count
        for (int i = 0; i < throwableCount; i++)
        {
            // Instantiate and set position to random position within -30x30 radius
            GameObject newCan = Instantiate(throwableCanPrefab);
            newCan.transform.position = new Vector3(Random.Range(-30, 30), 0, Random.Range(-30, 30));
            
            // Append to throwables
            throwables.Add(newCan);
            newCan.transform.SetParent(throwableParent.transform);
        }
    }

    void Update()
    {
        HandleGameTimer();
        HandleAlertMechanic();
    }

    void HandleGameTimer()
    {
        // Countdown game timer
        if (timeRemaining >= 0 )
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            // TBD --- PLACE HOLDER
            Debug.Log("GAME OVER");
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayDayEnd();
            //SceneController.GameOver();
        }
    }

    void HandleAlertMechanic()
    {
        if (activeAlertStars == 5)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayLoseMusic();
            //SceneController.GameOver();
        }
    }

    // Adds an alert start to active alert stars
    public void AddAlertStar()
    {
        if (activeAlertStars < 5)
        {
            activeAlertStars++;
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayAlertTone();
        }
    }

    // Removes an alert start to active alert stars
    public void RemoveAlertStar()
    {
        if (activeAlertStars > 0)
            activeAlertStars--;
    }
}
