using System;
using UnityEngine;

public enum ThrowableObjectTag { UNTAGGED, ROCK, STICK, }

[Serializable]
public struct ObjectData
{
    public ThrowableObjectTag tag;
    public GameObject prefabModel;
    public float objectWeight;
    public float soundRadius;
}