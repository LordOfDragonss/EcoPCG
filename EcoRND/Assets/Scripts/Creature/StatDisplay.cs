using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{
    public Canvas canvas;
    public Creature creature;
    public bool isVisible;

    [Header("Text Boxes")]
    public TextMeshProUGUI DyingNotification;
    public TextMeshProUGUI Speed;
    public TextMeshProUGUI Size;
    public TextMeshProUGUI VisionRadius;
    public TextMeshProUGUI WalkRange;
    public TextMeshProUGUI Diet;
    public TextMeshProUGUI HuntType;
    public TextMeshProUGUI Hunger;

    public void EnableCanvas()
    {
        canvas.enabled = true;
        isVisible = true;
        SetupStats();
    }
    private void Update()
    {
        Hunger.text = "Hunger: " + creature.Hunger.ToString();
        if (creature.isDying)
        {
            DyingNotification.enabled = true;
            DyingNotification.text = "Creature is Dying in " + creature.dyingTimer;
            DyingNotification.color = Color.red;
        }
        else
        {
            DyingNotification.enabled = false;
        }
    }

    private void LateUpdate()
    {
        canvas.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public void SetupStats()
    {
        Speed.text = "Speed: " + creature.speed.ToString();
        Size.text = "Size: " + creature.size.ToString();
        VisionRadius.text = "Vision Radius: " + creature.VisionRadius.ToString();
        WalkRange.text = "Walk Range: " + creature.WalkRange.ToString();
        Diet.text = "Diet: " + creature.diet.ToString();
        HuntType.text = "Hunt Type: " + creature.huntType.ToString();
    }

    public void DisableCanvas()
    {
        canvas.enabled = false;
        isVisible = false;
    }
}
