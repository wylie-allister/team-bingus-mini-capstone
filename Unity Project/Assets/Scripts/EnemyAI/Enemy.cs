using UnityEngine;
using UnityEngine.AI;

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
    private float sasSpotTimerMax = 1.5f;   // bumped from 1s so player has a bit more time to duck out
    public bool shouldRunAway = false;

    private GameObject lastThrowable = null;
    private bool startDistractTimer = false;
    private float distractionTimer = 0.0f;
    public float distractTimeDelay = 0.8f;  // lowered from 1.1s for snappier enemy reaction to throws

    // how long exclaim/question markers stay visible after trigger clears
    private float exclaimLingerTimer = 0.0f;
    private float questionLingerTimer = 0.0f;
    private float markerLingerTime = 1.2f;

    // if agent stops moving for too long, reset it
    private float stuckTimer = 0.0f;
    private float stuckTimeLimit = 3.0f;
    private Vector3 lastPosition;

    void Start()
    {
        // Get the NavMeshAgent component attached to this GameObject
        // Set spawnpoint to current position
        agent = GetComponent<NavMeshAgent>();
        spawnPoint = transform.position;
        lastPosition = spawnPoint;

        // Set guard point to a random area near spawn
        guardPoint = GetNewGuardPoint();

        // Set nav agent destination to guardpoint
        agent.SetDestination(guardPoint);
        exclaimMarkerObject.SetActive(false);
        questionMarkerObject.SetActive(false);
    }

    void Update()
    {
        HandleMarkerDisplay();
        HandleDistractTimer();
        HandleGuarding();
        HandleDistraction();
        HandleVision();
        HandleStuckDetection();
    }

    // keep markers visible for a moment after trigger clears
    void HandleMarkerDisplay()
    {
        if (hasSpottedSas)
        {
            exclaimLingerTimer = markerLingerTime;
        }
        else if (exclaimLingerTimer > 0.0f)
        {
            exclaimLingerTimer -= Time.deltaTime;
        }

        if (isDistracted)
        {
            questionLingerTimer = markerLingerTime;
        }
        else if (questionLingerTimer > 0.0f)
        {
            questionLingerTimer -= Time.deltaTime;
        }

        exclaimMarkerObject.SetActive(exclaimLingerTimer > 0.0f);
        questionMarkerObject.SetActive(questionLingerTimer > 0.0f);
    }

    void HandleVision()
    {
        Vector3 origin = transform.position;
        origin.y = 1.5f;
        Vector3 dir = transform.forward;
        float maxDist = 10.0f;
        RaycastHit hit;

        // Uncomment to visualize enemy vision in scene view
        //Debug.DrawRay(origin, dir * maxDist);

        if (Physics.Raycast(origin, dir, out hit, maxDist))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                hasSpottedSas = true;
            }
        }
        else
        {
            if (!hasSpottedSas)
                sasSpotTimer = 0.0f;
        }

        HandleSasSpot();
    }

    void HandleSasSpot()
    {
        // If the enemy hasn't spotted sas, break from logic
        if (!hasSpottedSas)
            return;

        // If enemy has spotted sas, run timer
        sasSpotTimer += Time.deltaTime;

        // If timer is greater than timer max, add alert and stop spotting
        if (sasSpotTimer >= sasSpotTimerMax)
        {
            GameManager.Instance.AddAlertStar();
            shouldTriggerHuh = true;
            hasSpottedSas = false;
            sasSpotTimer = 0.0f;
        }
    }

    void HandleDistraction()
    {
        // If not distracted, break from logic
        if (!isDistracted)
            return;

        // If distracted, set idle to false
        isIdle = false;

        // Get a new destination when we first become distracted
        if (obtainNewDistractPoint)
        {
            finalDistractionPoint = GetDirectionXZ(distractionSourcePosition, shouldRunAway);
            finalDistractionPoint = Vector3.Normalize(finalDistractionPoint) * 15;
            finalDistractionPoint += this.transform.position;

            agent.SetDestination(finalDistractionPoint);
            obtainNewDistractPoint = false;
        }

        // If we are within range of the distraction point, return to spawn
        if (IsAtPointWithinRange(finalDistractionPoint, 2.0f))
        {
            isDistracted = false;
            shouldRunAway = false;
            obtainNewDistractPoint = true;
            agent.SetDestination(spawnPoint);
        }
    }

    // Guards between enemy spawn point and a given guard point
    void HandleGuarding()
    {
        // If idling or distracting, break from logic
        if (isIdle || isDistracted)
            return;

        // Set navagent dest to guard point if at spawn point
        if (IsAtPointWithinRange(spawnPoint, 1f))
        {
            agent.SetDestination(guardPoint);
        }
        // Set navagent dest to spawn point if at guard point
        else if (IsAtPointWithinRange(guardPoint, 1f))
        {
            guardPoint = GetNewGuardPoint();
            agent.SetDestination(spawnPoint);
        }
    }

    void HandleStuckDetection()
    {
        if (isDistracted)
        {
            stuckTimer = 0.0f;
            lastPosition = transform.position;
            return;
        }

        if (Vector3.Distance(transform.position, lastPosition) < 0.1f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer >= stuckTimeLimit)
            {
                guardPoint = GetNewGuardPoint();
                agent.SetDestination(guardPoint);
                stuckTimer = 0.0f;
            }
        }
        else
        {
            stuckTimer = 0.0f;
            lastPosition = transform.position;
        }
    }

    Vector3 GetNewGuardPoint()
    {
        Vector3 candidate = spawnPoint + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

        if (NavMesh.SamplePosition(candidate, out NavMeshHit navHit, 5f, NavMesh.AllAreas))
            return navHit.position;

        return spawnPoint;
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
            return new Vector3(this.transform.position.x - point.x, 0,
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
        if (!startDistractTimer)
            return;

        distractionTimer += Time.deltaTime;

        if (distractionTimer >= distractTimeDelay)
        {
            distractionSourcePosition = lastThrowable.gameObject.transform.position;
            shouldTriggerHuh = true;
            isDistracted = true;
            obtainNewDistractPoint = true;
            distractionTimer = 0.0f;
            startDistractTimer = false;
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

        // If enemy collides with roar collider - run away from player
        if (collision.gameObject.CompareTag("RoarCollider"))
        {
            shouldRunAway = true;
            distractionSourcePosition = collision.gameObject.transform.position;
            shouldTriggerHuh = true;
            isDistracted = true;
            obtainNewDistractPoint = true;
        }
    }
}
