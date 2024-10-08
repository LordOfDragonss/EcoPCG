using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using static CreatureSettings;

public class CreatureSpawner : MonoBehaviour
{
    [SerializeField] Creature creatureToSpawn;
    public GameObject creatureBase;

    [Header("Text Boxes")]
    [SerializeField] TMP_InputField Speed;
    [SerializeField] TMP_InputField Size;
    [SerializeField] TMP_InputField VisionRadius;
    [SerializeField] TMP_InputField WalkRange;
    [SerializeField] TMP_Dropdown Gender;
    [SerializeField] TMP_Dropdown HuntType;

    private void Update()
    {
        SetupStats();
    }

    public void FillUpDropDowns()
    {

    }

    public void SetupStats()
    {
        if(Speed.text != "")
        creatureToSpawn.speed = float.Parse(Speed.text);
        if (Size.text != "")
            creatureToSpawn.size = float.Parse(Size.text);
        if (VisionRadius.text != "")
            creatureToSpawn.VisionRadius = float.Parse(VisionRadius.text);
        if (WalkRange.text != "")
            creatureToSpawn.WalkRange = float.Parse(WalkRange.text);
        creatureToSpawn.gender = (CreatureSettings.Gender)Gender.value;
        creatureToSpawn.huntType = (CreatureSettings.HuntType)HuntType.value;
    }

    public void SpawnCreatureAtLocation(Vector3 location)
    {
        SetupStats();
        var newCreature = Instantiate(creatureBase, location,creatureBase.transform.rotation).GetComponent<CreatureController>();
        Creature creature = newCreature.InitiateCreature(creatureToSpawn.size, creatureToSpawn.speed, creatureToSpawn.VisionRadius, creatureToSpawn.WalkRange, creatureToSpawn.gender, creatureToSpawn.huntType);
        newCreature.Creature = creature;
    }
}
