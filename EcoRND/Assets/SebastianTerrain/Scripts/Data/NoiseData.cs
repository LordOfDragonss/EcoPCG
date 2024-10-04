using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NoiseData : UpdatableData
{
    public Noise.NormalizeMode normalizeMode;

    public float noiseScale;

    [Min(0)] public int octaves;
    [Range(0, 1)]
    public float persistance;
    [Min(1)] public float lacunarity;

    public int seed;
    public Vector2 offset;
}
