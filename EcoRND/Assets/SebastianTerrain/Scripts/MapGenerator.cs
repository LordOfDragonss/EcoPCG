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
    [Min(1)] public int mapWidth;
    [Min(1)] public int mapHeight;
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
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunatiry, offset);
        Color[] ColourMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight =  noiseMap[x, y];
                for(int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        ColourMap[y * mapWidth + x] = regions[i].color;
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
            display.DrawTexture(TextureGenerator.textureFromColourMap(ColourMap, mapWidth, mapHeight));
        }else if(drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, MeshHeightMultiplier,meshHeightCurve), TextureGenerator.textureFromColourMap(ColourMap, mapWidth, mapHeight));
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
