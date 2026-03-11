using System;
using UnityEngine;

public enum ThrowableObjectTag { UNTAGGED, ROCK, STICK, CAN, PINECONE, }

[Serializable]
public struct ObjectData
{
    // Tag for item delineation
    public ThrowableObjectTag tag;
    
}