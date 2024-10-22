using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public WorldSettings settings;
    public static WorldController instance;

    private void Awake()
    {
        instance = this;
    }

    public WorldController(WorldSettings settings)
    {
        settings = this.settings;
    }
}
