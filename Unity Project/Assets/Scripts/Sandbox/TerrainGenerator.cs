using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Model References")]
    [SerializeField] private GameObject[] treePrefabs;

    [Header("Settings")] 
    public Transform treeParent;
    public int numberOfTrees;

    public Vector2 treeSpawnRadius;
    public List<GameObject> trees = new List<GameObject>();
    public float treeSpacingOffset = 2.0f;
    
    [Header("Camps")] 
    public GameObject campPrefab;
    List<GameObject> camps = new List<GameObject>();
    public GameObject campParent;
    public int campCount = 5;


    void Awake()
    {
        HandleTreeRandomization();
    }
    
    void Start()
    {
        SpawnCamps();
    }

    // Update is called once per frame
    void Update()
    {
        /*/ TBD - DEBUG
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < numberOfTrees; i++)
            {
                RandomizeTreePosition(trees[i]);
            }
        }
        */
    }

    public void HandleTreeRandomization()
    {
        // Create new list of trees with an individual random position
        for (int treeIndex = 0; treeIndex < numberOfTrees; treeIndex++)
        {
            GameObject newTree = Instantiate(treePrefabs[Random.Range(0, treePrefabs.Length)]);

            RandomizeTreePosition(newTree);
            newTree.transform.parent = treeParent;
            trees.Add(newTree);
        }
    }

    public void RandomizeTreePosition(GameObject tree)
    {
        // Get new random position within the spawn radius
        Vector3 randomPosition = new Vector3(Random.Range(-treeSpawnRadius.x, treeSpawnRadius.x), 0, Random.Range(-treeSpawnRadius.y, treeSpawnRadius.y));
        
        // Set tree position to new random position
        tree.transform.position = randomPosition;
    }

    public void SpawnCamps()
    {
        // Camp spawn radius lies within the tree spawn radius
        Vector2 campSpawnRadius = new Vector2(treeSpawnRadius.x- 5.0f, treeSpawnRadius.y - 5.0f);
        
        // Create new camps and append to list
        for (int i = 0; i < camps.Count; i++)
        {
            GameObject newCamp = Instantiate(campPrefab);
            newCamp.transform.position = new Vector3(Random.Range(-campSpawnRadius.x, campSpawnRadius.y), 0, Random.Range(-campSpawnRadius.y, campSpawnRadius.y));
            //newCamp.transform.parent = campParent.transform;
            
            camps.Add(newCamp);
        }
    }
}
