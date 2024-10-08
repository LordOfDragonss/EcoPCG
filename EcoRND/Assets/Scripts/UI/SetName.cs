using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

[ExecuteInEditMode]
public class SetName : MonoBehaviour
{
    public GameObject parent;

    private void Update()
    {
        GetComponent<TextMeshProUGUI>().text = parent.name;
    }
}
