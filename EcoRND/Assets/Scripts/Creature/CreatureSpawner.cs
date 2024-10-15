using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using static Enums;

public class CreatureSpawner : MonoBehaviour
{
    [SerializeField] CreatureSettings creatureToSpawnSettings;
    public GameObject creatureBase;
    public GameObject foodBase;
    public GameObject parent;
    public List<CreatureSettings> creatureSettings;
    public GameObject loadList;
    public GameObject LoadButtonPreset;
    private List<GameObject> ActiveLoadButtons = new List<GameObject>();

    [Header("Text Boxes")]
    [SerializeField] TMP_InputField Name;
    [SerializeField] TMP_InputField Speed;
    [SerializeField] TMP_InputField Size;
    [SerializeField] TMP_InputField VisionRadius;
    [SerializeField] TMP_InputField WalkRange;
    [SerializeField] TMP_InputField MaxHunger;
    [SerializeField] TMP_Dropdown Diet;
    [SerializeField] TMP_Dropdown HuntType;

    [Header("Color")]
    [SerializeField] FlexibleColorPicker colorPicker;

    private void Start()
    {
        FillDropdownWithEnum<Diet>(Diet);
        FillDropdownWithEnum<HuntType>(HuntType);
    }

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

    void FillDropdownWithEnum<T>(TMP_Dropdown dropdown) where T : System.Enum
    {
        dropdown.ClearOptions(); // Clear existing options if any
        dropdown.AddOptions(System.Enum.GetNames(typeof(T)).ToList()); // Add enum values as options
    }

    public void FillTextboxesWithSettings(CreatureSettings settings)
    {
        Name.text = settings.name;
        Speed.text = settings.Speed.ToString();
        Size.text = settings.Size.ToString();
        VisionRadius.text = settings.VisionRadius.ToString();
        WalkRange.text = settings.WalkRange.ToString();
        MaxHunger.text = settings.maxHunger.ToString();
        colorPicker.color = settings.color;
        Diet.value = (int)settings.diet;
        HuntType.value = (int)settings.huntType;
    }

    public void SaveCreature()
    {
        var newCreatureSettings = ScriptableObject.CreateInstance<CreatureSettings>();
        newCreatureSettings.name = Name.text;
        newCreatureSettings.Speed = creatureToSpawnSettings.Speed;
        newCreatureSettings.Size = creatureToSpawnSettings.Size;
        newCreatureSettings.VisionRadius = creatureToSpawnSettings.VisionRadius;
        newCreatureSettings.WalkRange = creatureToSpawnSettings.WalkRange;
        newCreatureSettings.maxHunger = creatureToSpawnSettings.maxHunger;
        newCreatureSettings.color = creatureToSpawnSettings.color;
        newCreatureSettings.diet = creatureToSpawnSettings.diet;
        newCreatureSettings.huntType = creatureToSpawnSettings.huntType;
        creatureSettings.Add(newCreatureSettings);
    }

    public void LoadCreature()
    {
        // Clear existing buttons safely
        for (int i = ActiveLoadButtons.Count - 1; i >= 0; i--)
        {
            Destroy(ActiveLoadButtons[i]); // Destroy the button
            ActiveLoadButtons.RemoveAt(i); // Remove from the list
        }
        foreach (var settings in creatureSettings)
        {
            LoadButton loadButton = Instantiate(LoadButtonPreset, loadList.transform).GetComponent<LoadButton>();
            loadButton.gameObject.name = settings.name;
            loadButton.creatureSettings = settings;
            loadButton.creatureSpawner = this;
            ActiveLoadButtons.Add(loadButton.gameObject);
        }
        DisableCanvas();
        loadList.SetActive(true);
    }

    public void UpdateStats()
    {
        if (Speed.text != "")
            creatureToSpawnSettings.Speed = float.Parse(Speed.text);
        if (Size.text != "")
            creatureToSpawnSettings.Size = float.Parse(Size.text);
        if (VisionRadius.text != "")
            creatureToSpawnSettings.VisionRadius = float.Parse(VisionRadius.text);
        if (WalkRange.text != "")
            creatureToSpawnSettings.WalkRange = float.Parse(WalkRange.text);
        if (MaxHunger.text != "")
            creatureToSpawnSettings.maxHunger = float.Parse(MaxHunger.text);
        creatureToSpawnSettings.color = colorPicker.color;
        creatureToSpawnSettings.diet = (Diet)Diet.value;
        creatureToSpawnSettings.huntType = (HuntType)HuntType.value;
    }

    public void SpawnCreatureAtLocation(Vector3 location)
    {
        UpdateStats();
        var newCreature = Instantiate(creatureBase, location, creatureBase.transform.rotation, parent.transform).GetComponent<CreatureController>();
        newCreature.gameObject.name = Name.text;
        Creature creature = newCreature.InitiateCreature(creatureToSpawnSettings);
        newCreature.creature = creature;
    }

    public void SpawnFoodAtLocation(Vector3 location)
    {
        var newFood = Instantiate(foodBase, location, creatureBase.transform.rotation, parent.transform).GetComponent<CreatureController>();
    }
}


