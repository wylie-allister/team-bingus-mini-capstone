using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    public ObjectData data;
    public bool hasBeenThrown = false;
    
    void Start()
    {
        // If object is untagged, throw error - preventative
        if (data.tag == ThrowableObjectTag.UNTAGGED || data.prefabModel == null)
        {
            Debug.LogWarning("OBJECT DATA NOT FOUND");
        }
    }
}
