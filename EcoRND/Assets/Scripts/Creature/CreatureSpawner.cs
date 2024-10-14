using JetBrains.Annotations;
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
    public List<CreatureSettings> creatureSettings;
    public GameObject loadList;
    public GameObject LoadButtonPreset;

    [Header("Text Boxes")]
    [SerializeField] TMP_InputField Name;
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

    public void DisableCanvas()
    {
        gameObject.SetActive(false);
    }

    public void EnableCanvas()
    {
        gameObject.SetActive(true);
    }

    public void FillUpDropDowns()
    {

    }

    public void FillTextboxesWithSettings(CreatureSettings settings)
    {
        Name.text = settings.name;
        Speed.text = settings.Speed.ToString();
        Size.text = settings.Size.ToString();
        VisionRadius.text = settings.VisionRadius.ToString();
        WalkRange.text = settings.WalkRange.ToString();
        Diet.value = (int)settings.diet;
        HuntType.value = (int)settings.huntType;
    }

    public void SaveCreature()
    {
        var newCreatureSettings = ScriptableObject.CreateInstance<CreatureSettings>();
        newCreatureSettings.Speed = creatureToSpawn.speed;
        newCreatureSettings.Size = creatureToSpawn.size;
        newCreatureSettings.VisionRadius = creatureToSpawn.VisionRadius;
        newCreatureSettings.WalkRange = creatureToSpawn.WalkRange;
        newCreatureSettings.color = creatureToSpawn.color;
        newCreatureSettings.diet = creatureToSpawn.diet;
        newCreatureSettings.huntType = creatureToSpawn.huntType;
        newCreatureSettings.name = Name.text;
        creatureSettings.Add(newCreatureSettings);
    }

    public void LoadCreature()
    {
        foreach (var settings in creatureSettings)
        {
            LoadButton loadButton = Instantiate(LoadButtonPreset, loadList.transform).GetComponent<LoadButton>();
            loadButton.gameObject.name = settings.name;
            loadButton.creatureSettings = settings;
            loadButton.creatureSpawner = this;
        }
        DisableCanvas();
        loadList.SetActive(true);
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
        newCreature.gameObject.name = Name.text;
        Creature creature = newCreature.InitiateCreature(creatureToSpawn.size, creatureToSpawn.speed, creatureToSpawn.VisionRadius, creatureToSpawn.WalkRange, creatureToSpawn.color, creatureToSpawn.diet, creatureToSpawn.huntType);
        newCreature.creature = creature;
    }

    public void SpawnFoodAtLocation(Vector3 location)
    {
        var newFood = Instantiate(foodBase, location, creatureBase.transform.rotation, parent.transform).GetComponent<CreatureController>();
    }
}


