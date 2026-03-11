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
    public bool shouldTriggerHuh = false;
    private bool obtainNewDistractPoint = true;

    public GameObject exclaimMarkerObject;
    public GameObject questionMarkerObject;

    private bool hasSpottedSas = false;
    private float sasSpotTimer = 0.0f;
    private float sasSpotTimerMax = 1f;
    public bool shouldRunAway = false;

    private GameObject lastThrowable = null;
    private bool startDistractTimer = false;
    private float distractionTimer = 0.0f;
    public float distractTimeDelay = 1.10f;

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
        questionMarkerObject.SetActive(false);


    }

    
    void Update()
    {
        exclaimMarkerObject.SetActive(hasSpottedSas);
        questionMarkerObject.SetActive(isDistracted);
        
        RaycastHit hit;
        
        // Cast ray from base of object upwards, if an item/collider/object/model is present, move guardpoint
        if (Physics.SphereCast(guardPoint, 2.0f, Vector3.up, out hit, 3.0f))
        {
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                guardPoint += new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            }
        }
        
        HandleDistractTimer();
        HandleGuarding();
        HandleDistraction();
        HandleVision();
    }

    void HandleVision()
    {
        Vector3 origin = transform.position;
        origin.y = 1.5f;
        Vector3 dir = transform.forward;
        float maxDist = 10.0f;
        RaycastHit hit;
        
        // Scene debug ray call to visualize the distance of the enemy vision
        //Debug.DrawRay(origin, dir * maxDist);

        if (Physics.Raycast(origin, dir, out hit, maxDist))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player Seen!!!");
                hasSpottedSas = true;
            }
        }

        HandleSasSpot();
    }

    void HandleSasSpot()
    {
		// If the neemy hasnt spotted sas, break from logic and set timer to 0
        if (!hasSpottedSas)
        {
            sasSpotTimer = 0.0f;
            return;
        }
        
        // If enemy has spotted sas, run timer
        sasSpotTimer += Time.deltaTime;
        
        // If timer is greater than timer max, stop spotting sas
        if (sasSpotTimer >= sasSpotTimerMax)
        {
            GameManager.Instance.AddAlertStar();
            shouldTriggerHuh = true;
            hasSpottedSas = false;
        }
    }
    
    void HandleDistraction()
    {
        // If not distracted, break from logic
        if (!isDistracted)
            return;

        // If distracted, set idle to false
        isIdle = false;
        
        // If we need a new distraction point
        if (distractionSourcePosition != null && agent.destination != finalDistractionPoint && obtainNewDistractPoint)
        {
            // Get direction between where the distraction source is from, normalize, scale, and apply transform
            finalDistractionPoint =  GetDirectionXZ(distractionSourcePosition, shouldRunAway);
            finalDistractionPoint = Vector3.Normalize(finalDistractionPoint) * 15 ;
            finalDistractionPoint += this.transform.position;
            
            // Set nav agent destination to new position
            agent.SetDestination(finalDistractionPoint);
            
            // Prevent update of new distraction point
            obtainNewDistractPoint = false;
        }

        // If we are within a given range of the distraction point
        if (IsAtPointWithinRange(finalDistractionPoint, 2.0f))
        {
            // Not distracted, can obtain new distraction point, move to spawn
            isDistracted = false;
            obtainNewDistractPoint = true;
            agent.SetDestination(spawnPoint);
        }
    }

    // Guards between enemy spawn point and a given guard point
    void HandleGuarding()
    {
        // If idling or distracting, break from logic
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
    
    // Return whether a point is within a given range of this object, or not
    bool IsAtPointWithinRange(Vector3 point, float range)
    {
        if (this.transform.position.x >= point.x - range && this.transform.position.x <= point.x + range)
        {
            if (this.transform.position.z >= point.z - range && this.transform.position.z <= point.z + range)
                return true;
        }

        return false;
    }

    // Returns the direction from this object to a given point, swappable
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

    private void HandleDistractTimer()
    {
        if (startDistractTimer)
        {
            distractionTimer += Time.deltaTime;

            if (distractionTimer >= distractTimeDelay)
            {
                shouldRunAway = true;
                shouldRunAway = false;
                distractionSourcePosition = lastThrowable.gameObject.transform.position;
                shouldTriggerHuh = true;
                isDistracted = true;
                distractionTimer = 0.0f;
                startDistractTimer = false;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // If enemy collides with a distraction point
        if (collision.gameObject.CompareTag("DistractionPoint"))
        {
            // If object is throwable, set to distracted
            if (collision.transform.GetComponentInParent<ThrowableObject>().hasBeenThrown)
            {
                lastThrowable = collision.gameObject;
                startDistractTimer = true;
            }
        }

        // If enemy collides with roarcollider
        if (collision.gameObject.CompareTag("RoarCollider"))
        {
            // Move away from player
            shouldRunAway = true;
            distractionSourcePosition = collision.gameObject.transform.position;
            shouldTriggerHuh = true;
            isDistracted = true;
        }
    }
}