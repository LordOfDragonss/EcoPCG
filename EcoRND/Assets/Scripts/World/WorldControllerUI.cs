using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldControllerUI : MonoBehaviour
{
    public WorldController worldController;
    [Header("Text boxes")]
    [SerializeField] TextBoxList textBoxes;

    private void OnEnable()
    {
        SetupDefaultSettings();
    }

    public void SetupDefaultSettings()
    {
        textBoxes.HungerDecay.text = worldController.settings.HungerDecay.ToString();
        textBoxes.TimeTillCreatureDies.text = worldController.settings.TimeTillCreatureDeath.ToString();
    }

    public void UpdateSettings()
    {
        worldController.settings.HungerDecay = float.Parse(textBoxes.HungerDecay.text);
        worldController.settings.TimeTillCreatureDeath = float.Parse(textBoxes.TimeTillCreatureDies.text);
    }
}

[System.Serializable]
public class TextBoxList
{
    public TMP_InputField HungerDecay;
    public TMP_InputField TimeTillCreatureDies;
}
