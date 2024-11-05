using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCamera : MonoBehaviour
{

    public Camera[] cameras;
    [SerializeField] private int currentIndex;

    public void SwapCameraInList(float next)
    {
        if (currentIndex > 0 && currentIndex < cameras.Length)
        {
            currentIndex += (int)next;
        }
        else if (currentIndex <= 0)
        {
            currentIndex = cameras.Length - 1;
        }
        else if (currentIndex >= cameras.Length)
        {
            currentIndex = 0;
        }
            SwitchCamera(currentIndex);

    }

    public void SwitchCamera(int index)
    {
        foreach (var cam in cameras)
        {
            cam.enabled = false;
        }
        cameras[index].enabled = true;
    }
}
