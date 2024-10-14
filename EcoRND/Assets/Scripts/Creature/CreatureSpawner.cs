using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using static Enums;

public class CreatureSpawner : MonoBehaviour
{
    [SerializeField] Creature creatureToSpawn;
    public GameObject creatureBase;
    public GameObject foodBase;
    public GameObject parent;

    [Header("Text Boxes")]
    [SerializeField] TMP_InputField Speed;
    [SerializeField] TMP_InputField Size;
    [SerializeField] TMP_InputField VisionRadius;
    [SerializeField] TMP_InputField WalkRange;
    [SerializeField] TMP_Dropdown Diet;
    [SerializeField] TMP_Dropdown HuntType;

    [Header("Color")]
    [SerializeField] FlexibleColorPicker colorPicker;


    private void Update()
    {
        UpdateStats();
    }

    public void FillUpDropDowns()
    {

    }

    public void UpdateStats()
    {
        if (Speed.text != "")
            creatureToSpawn.speed = float.Parse(Speed.text);
        if (Size.text != "")
            creatureToSpawn.size = float.Parse(Size.text);
        if (VisionRadius.text != "")
            creatureToSpawn.VisionRadius = float.Parse(VisionRadius.text);
        if (WalkRange.text != "")
            creatureToSpawn.WalkRange = float.Parse(WalkRange.text);
        creatureToSpawn.color = colorPicker.color;
        creatureToSpawn.diet = (Diet)Diet.value;
        creatureToSpawn.huntType = (HuntType)HuntType.value;
    }

    public void SpawnCreatureAtLocation(Vector3 location)
    {
        UpdateStats();
        var newCreature = Instantiate(creatureBase, location, creatureBase.transform.rotation, parent.transform).GetComponent<CreatureController>();
        Creature creature = newCreature.InitiateCreature(creatureToSpawn.size, creatureToSpawn.speed, creatureToSpawn.VisionRadius, creatureToSpawn.WalkRange,creatureToSpawn.color, creatureToSpawn.diet, creatureToSpawn.huntType);
        newCreature.creature = creature;
    }

    public void SpawnFoodAtLocation(Vector3 location)
    {
        var newFood = Instantiate(foodBase, location, creatureBase.transform.rotation, parent.transform).GetComponent<CreatureController>();
    }
}
