using UnityEngine;
using UnityEngine.AI; // Required for NavMesh classes

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    private Vector3 spawnPoint;
    private Vector3 guardPoint;

    void Start()
    {
        // Get the NavMeshAgent component attached to this GameObject
        // Set spawnpoint to current position
        agent = GetComponent<NavMeshAgent>();
        spawnPoint = transform.position;
        
        // Defaultly set spawn point to a random area
        guardPoint = spawnPoint;
        guardPoint += new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        
        // Set nav agent destination to guardpoint
        agent.SetDestination(guardPoint);
    }

    void Update()
    {
        RaycastHit hit;
        
        // Cast ray from base of object upwards, if an item is present, move guardpoint
        if (Physics.SphereCast(guardPoint, 2.0f, Vector3.up, out hit, 3.0f))
        {
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                guardPoint += new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            }
        }
        
        // Set navagent dest to guard point if at spawn point
        if (this.transform.position.x == spawnPoint.x && this.transform.position.z == spawnPoint.z)
        {
            agent.SetDestination(guardPoint);
        }
        // Set navagent dest to spawn point if at guard point
        else if (this.transform.position.x == guardPoint.x && this.transform.position.z == guardPoint.z)
        {
            agent.SetDestination(spawnPoint);
        }
    }
}