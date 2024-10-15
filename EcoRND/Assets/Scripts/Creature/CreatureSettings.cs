using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

[CreateAssetMenu]
public class CreatureSettings : ScriptableObject
{
    public float Size;
    public float Speed;
    public float VisionRadius;
    public float WalkRange;
    public Color color;
    public float Stamina;
    public float maxHunger;


    public Diet diet;
    public HuntType huntType;
}
