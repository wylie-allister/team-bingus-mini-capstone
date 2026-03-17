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
    
    [Header("Alert Mechanic")] 
    public int activeAlertStars = 0;

    [Header("Marked Trees")] 
    public int marksWiped = 0;

    public float timeBetweenStars = 2.0f;
    private float starTimer = 0.0f;
    private bool canAddActiveStar = true;

    [Header("Trees")]
    public List<GameObject> trees;
    
    [Header("Terrain Dev")]
    public TerrainGenerator terrainGenerator;

    public GameObject[] throwablePrefabs;
    public GameObject throwableParent;
    public int throwableCount = 50;
    private List<GameObject> throwables =  new List<GameObject>();

    public GameObject[] cloudPrefabs;
    public Transform cloudParent;
    private List<GameObject> clouds = new List<GameObject>();
    public float cloudSpeed = 10.0f;

    public int cloudCount = 10;

    public Animator canvasAnimator;
    private float canvasAnimationTimer = 0.0f;
    private bool activeAnimationTimer = true;
    private float endGameAnimationTimer = 0.0f;
    private bool activeEndGameTimer = false;
    
    public bool gameOver = false;

    
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
            //DontDestroyOnLoad(this.gameObject);
        }
        
        PlayerPrefs.SetInt("TreesSaved", 0);
        PlayerPrefs.Save();
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

        // Create new throwables based on throwable count
        CreateThrowables();

        CreateClouds();
        
    }

    void CreateThrowables()
    {
        for (int i = 0; i < throwableCount; i++)
        {
            // Instantiate and set position to random position within -30x30 radius
            GameObject newThrowable = Instantiate(throwablePrefabs[Random.Range(0, throwablePrefabs.Length)]);
            newThrowable.transform.position = new Vector3(Random.Range(-30, 30), 3, Random.Range(-30, 30));
            
            // Append to throwables
            throwables.Add(newThrowable);
            newThrowable.transform.SetParent(throwableParent.transform);
        }
    }
    void CreateClouds()
    {
        for (int i = 0; i < cloudCount; i++)
        {
            GameObject newCloud =  Instantiate(cloudPrefabs[Random.Range(0, cloudPrefabs.Length)]);
            newCloud.transform.position = new Vector3(Random.Range(-800, 800), Random.Range(170, 190), Random.Range(-400, 400));
            newCloud.transform.SetParent(cloudParent);
            clouds.Add(newCloud);
        }
    }
    
    void Update()
    {
        if (canvasAnimationTimer >= 1.0f)
        {
            canvasAnimator.SetBool("StartAnimation", true);
            activeAnimationTimer = false;
            canvasAnimationTimer = 0.0f;
        }
        
        if (activeAnimationTimer)
        {
            canvasAnimationTimer += Time.deltaTime;
            return;
        }
        
        
        HandleGameTimer();
        HandleAlertMechanic();

        UpdateClouds();

        HandleEndGame();
    }

    void UpdateClouds()
    {
        if (gameOver)
        {
            return;
        }
        foreach (GameObject cloud in clouds)
        {
            if (cloud.transform.position.x > 1000)
            {
                cloud.transform.position = new Vector3(-1000, cloud.transform.position.y, cloud.transform.position.z);
            }
            
            cloud.transform.position = new Vector3(cloud.transform.position.x + (cloudSpeed * Time.deltaTime), cloud.transform.position.y, cloud.transform.position.z);
        }
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
            gameOver = true;
            activeEndGameTimer = true;

        }
    }

    void HandleAlertMechanic()
    {
        if (starTimer < timeBetweenStars)
        {
            starTimer += Time.deltaTime;
        }
        else
        {
            canAddActiveStar = true;
            starTimer = 0.0f;
        }
        
        if (activeAlertStars == 5)
        {
            activeEndGameTimer = true;
        }
    }

    public void HandleEndGame()
    {
        if (activeEndGameTimer)
        {
            endGameAnimationTimer += Time.deltaTime;
            canvasAnimator.SetBool("EndScene", true);
        }

        if (endGameAnimationTimer >= 1.7f)
        {
            bool lost = activeAlertStars == 5;

            // Guard: GameState may not be in the scene yet if prefab hasn't been updated
            if (GameState.Instance != null)
            {
                GameState.Instance.didPlayerLose = lost;
                GameState.Instance.endReason = lost
                    ? "You were spotted too many times!"
                    : "Time's up! The logging crew has been driven off.";
            }

            SceneController.Instance.GoToGameOver();
        }
    }
    
    // Adds an alert start to active alert stars
    public void AddAlertStar()
    {
        if (activeAlertStars < 5 && canAddActiveStar)
        {
            activeAlertStars++;
            canAddActiveStar = false;
        }
    }

    // Removes an alert start to active alert stars
    public void RemoveAlertStar()
    {
        if (activeAlertStars > 0)
            activeAlertStars--;
    }
}
