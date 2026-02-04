using System;
using UnityEngine;

public enum ThrowableObjectTag { UNTAGGED, ROCK, STICK, }

[Serializable]
public struct ObjectData
{
    // Tag for item delineation
    public ThrowableObjectTag tag;
    public GameObject prefabModel;
    
    // Stretch goal
    public float objectWeight;
    public float soundRadius;
}