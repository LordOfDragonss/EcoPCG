using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.AssetImporters;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap,ColourMap,Mesh
    }

    public DrawMode drawMode;

    public const int mapChunkSize = 241;
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
    Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();

    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData();

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.textureFromColourMap(mapData.colorMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, MeshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.textureFromColourMap(mapData.colorMap, mapChunkSize, mapChunkSize));
        }
    }

    public void RequestMapData(Action<MapData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(callback);
        };

        new Thread(threadStart).Start();
    }

    void MapDataThread(Action<MapData> callback)
    {
        MapData mapData = GenerateMapData();
        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
        }
    }


    private void Update()
    {
        if (mapDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < mapDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    MapData GenerateMapData()
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
        return new MapData(noiseMap, ColourMap);
    }

    struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
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

public struct MapData
{
    public readonly float[,] heightMap;
    public readonly Color[] colorMap;
    
    public MapData(float[,] heightMap, Color[] colorMap)
    {
        this.heightMap = heightMap;
        this.colorMap = colorMap;
    }
}