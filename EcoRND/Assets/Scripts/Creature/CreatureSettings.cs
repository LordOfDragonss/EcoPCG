using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CreatureSettings : ScriptableObject
{
    public float Size;
    public float Speed;
    public float VisionRadius;
    public float WalkRange;
    public Material material;
    public enum Gender
    {
        Male,Female
    }

    public Gender gender;
    public enum HuntType
    {
        Predator,Prey
    }
    public HuntType huntType;
}
