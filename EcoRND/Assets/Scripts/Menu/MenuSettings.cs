using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class MenuSettings : MonoBehaviour
{
    public Image cursorImage;
    public PlayerCamera playerCamera;
    public bool menuEnabled;
    Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        DisableMenu();
    }

    public void EnableMenu()
    {
        menuEnabled = true;
        Cursor.lockState = CursorLockMode.None;
        cursorImage.enabled = false;
        canvas.enabled = true;
        playerCamera.isInSpawnMode = false;
        playerCamera.creatureDeleteMode = false;
        playerCamera.canMove = false;
    }
    public void DisableMenu()
    {
        menuEnabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        cursorImage.enabled = true;
        canvas.enabled = false;
        playerCamera.canMove = true;
        cursorImage.color = Color.black;
    }

    public void EnableCreatureSpawner()
    {
        DisableMenu();
        playerCamera.isInSpawnMode = true;
        playerCamera.spawnMode = SpawnMode.Creature;
        cursorImage.color = Color.green;
    }

    public void EnableFoodSpawner()
    {
        DisableMenu();
        playerCamera.isInSpawnMode = true;
        playerCamera.spawnMode = SpawnMode.Food;
        cursorImage.color = Color.green;
    }

    public void EnableCreatureDeleter()
    {
        DisableMenu();
        playerCamera.creatureDeleteMode = true;
        cursorImage.color = Color.red;
    }
}
