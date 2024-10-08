using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuSettings : MonoBehaviour
{
    public GameObject cursorImage;
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
        cursorImage.SetActive(false);
        canvas.enabled = true;
        playerCamera.creatureSpawnMode = false;
        playerCamera.canMove = false;
    }
    public void DisableMenu()
    {
        menuEnabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        cursorImage.SetActive(true);
        canvas.enabled = false;
        playerCamera.canMove = true;
    }

    public void EnableCreatureSpawner()
    {
        DisableMenu();
        playerCamera.creatureSpawnMode = true;
    }
}
