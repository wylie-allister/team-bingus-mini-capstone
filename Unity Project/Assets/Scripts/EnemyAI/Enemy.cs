using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools; // Required for NavMesh classes

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    private Vector3 spawnPoint;
    private Vector3 guardPoint;
    public Vector3 finalDistractionPoint;

    private bool isIdle = false;
    public Vector3 distractionSourcePosition;

    public bool isDistracted = false;
    private bool obtainNewDistractPoint = true;

    public GameObject exclaimMarkerObject;

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
        exclaimMarkerObject.SetActive(false);
        
    }

    
    void Update()
    {
        exclaimMarkerObject.SetActive(isDistracted);
        
        RaycastHit hit;
        
        // Cast ray from base of object upwards, if an item/collider/object/model is present, move guardpoint
        if (Physics.SphereCast(guardPoint, 2.0f, Vector3.up, out hit, 3.0f))
        {
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                guardPoint += new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            }
        }

        HandleGuarding();
        HandleDistraction();

    }

    void HandleDistraction()
    {
        if (!isDistracted)
            return;

        isIdle = false;
        
        if (distractionSourcePosition != null && agent.destination != finalDistractionPoint && obtainNewDistractPoint)
        {
            finalDistractionPoint =  GetDirectionXZ(distractionSourcePosition, true);
            finalDistractionPoint = Vector3.Normalize(finalDistractionPoint) * 15 ;
            finalDistractionPoint += this.transform.position;
            
            
            agent.SetDestination(finalDistractionPoint);
            obtainNewDistractPoint = false;
        }

        if (IsAtPointWithinRange(finalDistractionPoint, 2.0f))
        {
            isDistracted = false;
            obtainNewDistractPoint = true;
            agent.SetDestination(spawnPoint);
        }
        
    }

    // Guards between enemy spawn point and a given guard point
    void HandleGuarding()
    {
        if (isIdle || isDistracted)
        {
            return;
        }
        
        // Set navagent dest to guard point if at spawn point
        if (IsAtPointWithinRange(spawnPoint, 1f))
        {
            agent.SetDestination(guardPoint);
        }
        // Set navagent dest to spawn point if at guard point
        else if (IsAtPointWithinRange(guardPoint, 1f))
        {
            agent.SetDestination(spawnPoint);
        }
    }

    bool isAtPoint(Vector3 point)
    {
        if (this.transform.position.x == point.x && this.transform.position.z == point.z)
            return true;
        
        return false;
    }

    bool IsAtPointWithinRange(Vector3 point, float range)
    {
        if (this.transform.position.x >= point.x - range && this.transform.position.x <= point.x + range)
        {
            if (this.transform.position.z >= point.z - range && this.transform.position.z <= point.z + range)
                return true;
        }

        return false;
    }

    Vector3 GetDirectionXZ(Vector3 point, bool swapDir = false)
    {
        if (swapDir)
        {
            return new Vector3( this.transform.position.x - point.x, 0,
                this.transform.position.z - point.z);
        }
        else
        {
            return new Vector3(point.x - this.transform.position.x, 0,
                point.z - this.transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("DistractionPoint"))
        {
            Debug.Log("DISTRACTED");
            distractionSourcePosition = collision.gameObject.transform.position;
            isDistracted = true;
        }
    }
}