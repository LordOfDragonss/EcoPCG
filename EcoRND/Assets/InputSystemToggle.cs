using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InputSystemToggle : MonoBehaviour
{

    public void OnTextFieldSelect(string text)
    {
        InputSystem.DisableDevice(Keyboard.current);
    }

    public void OnTextFieldDeselect(string text)
    {
        InputSystem.EnableDevice(Keyboard.current);
    }
}
