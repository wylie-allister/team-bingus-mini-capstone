using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private Rigidbody rigidbody;
    public ObjectData data;
    public bool hasBeenThrown = false;
    private float thrownTimer = 0;
    
    void Start()
    {
        // If object is untagged, throw error - preventative
        if (data.tag == ThrowableObjectTag.UNTAGGED)
        {
            Debug.LogWarning("OBJECT DATA NOT FOUND");
        }
        meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        meshCollider = this.gameObject.GetComponent<MeshCollider>();
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
        //Instantiate(data.prefabModel, this.transform);
    }

    public void Update()
    {
        if (hasBeenThrown)
        {
            thrownTimer += Time.deltaTime;
        }

        if (thrownTimer >= 2.5f)
        {
            thrownTimer = 0;
            hasBeenThrown = false;
        }
    }

    public void DisableMesh()
    {
        // Disable meshrenderer and mesh collider, disable rigidbody sim
        meshRenderer.enabled = false;
        meshCollider.enabled = false;
        rigidbody.isKinematic = true;
    }
    
    public void  EnableMesh()
    {
        // Enable meshrenderer and mesh collider, enable rigidbody sim
        meshRenderer.enabled = true;
        meshCollider.enabled = true;
        rigidbody.isKinematic = false;
    }

}
