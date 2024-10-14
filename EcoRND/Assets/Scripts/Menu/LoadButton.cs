using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static Enums;

public class LoadButton : MonoBehaviour
{
    public CreatureSettings creatureSettings;
    public CreatureSpawner creatureSpawner;

    public void LoadSettingsIntoSpawner()
    {
        creatureSpawner.FillTextboxesWithSettings(creatureSettings);
        creatureSpawner.EnableCanvas();
        creatureSpawner.loadList.SetActive(false);
    }
}
