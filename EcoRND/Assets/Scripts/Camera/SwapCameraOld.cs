using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCameraOld : MonoBehaviour
{
    public Camera[] cameras;

    private void Start()
    {
        foreach (var camera in cameras)
        {
            camera.enabled = false;
        }
        cameras[0].enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            SwitchCamera(0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            SwitchCamera(1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            SwitchCamera(2);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            SwitchCamera(3);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            SwitchCamera(4);
        }
    }

    private void SwitchCamera(int index)
    {
        foreach (var cam in cameras)
        {
            cam.enabled = false;
        }
        cameras[index].enabled = true;
    }
}
