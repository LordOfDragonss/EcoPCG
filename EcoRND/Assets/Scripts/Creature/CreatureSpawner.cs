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
    [SerializeField] TMP_InputField Gender;
    [SerializeField] TMP_InputField HuntType;

    private void Update()
    {
        SetupStats();
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
    }

    public void SpawnCreatureAtLocation(Vector3 location)
    {
        SetupStats();
        var newCreature = Instantiate(creatureBase, location,creatureBase.transform.rotation).GetComponent<CreatureController>();
        newCreature.InitiateCreature(creatureToSpawn.size, creatureToSpawn.speed, creatureToSpawn.VisionRadius, creatureToSpawn.WalkRange, creatureToSpawn.gender, creatureToSpawn.huntType);
        newCreature.Creature = creatureToSpawn;
    }
}
