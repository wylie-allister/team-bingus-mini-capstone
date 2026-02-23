using System;
using UnityEngine;

public enum ThrowableObjectTag { UNTAGGED, ROCK, STICK, CAN, }

[Serializable]
public struct ObjectData
{
    // Tag for item delineation
    public ThrowableObjectTag tag;
    
    // Stretch goal
    public float objectWeight;
    public float soundRadius;
}