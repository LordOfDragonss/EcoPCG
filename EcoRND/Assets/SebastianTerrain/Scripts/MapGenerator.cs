using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap,ColourMap,Mesh
    }

    public DrawMode drawMode;

    const int mapChunkSize = 241;
    [Range(0,6)]
    public int levelOfDetail;
    public float noiseScale;

    [Min(0)] public int octaves;
    [Range(0, 1)]
    public float persistance;
    [Min(1)] public float lacunatiry;

    public int seed;
    public Vector2 offset;

    public float MeshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainType[] regions;
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunatiry, offset);
        Color[] ColourMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight =  noiseMap[x, y];
                for(int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        ColourMap[y * mapChunkSize + x] = regions[i].color;
                        break;
                    }
                }
            }
        }
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if(drawMode == DrawMode.NoiseMap)
        display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if(drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.textureFromColourMap(ColourMap, mapChunkSize, mapChunkSize));
        }else if(drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, MeshHeightMultiplier,meshHeightCurve,levelOfDetail), TextureGenerator.textureFromColourMap(ColourMap, mapChunkSize, mapChunkSize));
        }
    }
}
[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
