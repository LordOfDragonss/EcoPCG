using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuSettings : MonoBehaviour
{
    public Image cursorImage;
    public PlayerCamera playerCamera;
    public bool menuEnabled;
    Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    public void EnableMenu()
    {
        menuEnabled = true;
        Cursor.lockState = CursorLockMode.Confined;
        cursorImage.enabled = false;
        canvas.enabled = true;
        playerCamera.creatureSpawnMode = false;
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
        playerCamera.creatureSpawnMode = true;
        cursorImage.color = Color.green;
    }

    public void EnableCreatureDeleter()
    {
        DisableMenu();
        playerCamera.creatureDeleteMode = true;
        cursorImage.color = Color.red;
    }
}
