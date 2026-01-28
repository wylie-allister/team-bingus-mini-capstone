using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaypoint : MonoBehaviour
{

    public Transform externalTargetTransform;
    private Transform originTransform;

    public bool movingToTarget = false;
    public float moveSpeed = 1.0f;

    void Start()
    {
        originTransform.position = transform.position;
    }


    void Update()
    {
        HandleTarget();
    }

    void HandleTarget(Transform newTargetTransform = null)
    {
        Transform targetTransform;

        if (newTargetTransform == null)
        {
            targetTransform = externalTargetTransform;
        }
        else
        {
            
            targetTransform = newTargetTransform;
        }


        if (movingToTarget)
        {

        }
        
    }
}
