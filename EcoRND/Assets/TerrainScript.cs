using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)),RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshCollider))]
public class TerrainScript : MonoBehaviour
{
    Mesh mesh;
    private int MESH_SCALE = 500;
    public AnimationCurve heightCurve;
    private Vector3[] vertices;
    private int[] triangles;

    public int xSize;
    public int zSize;

    public float scale;
    public int octaves;
    public float lacunarity;

    public int seed;
    private System.Random prng;
    private Vector2[] octaveOffsets;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateNewMap();
    }

    private void CreateNewMap()
    {
        CreateMeshShape();
        CreateTriangles();
        UpdateMesh();
    }

    private void CreateMeshShape()
    {
        Vector2[] octaveOffsets = GetOffsetSeed();

        if (scale <= 0)
            scale = 0.0001f;

        vertices = new Vector3[(xSize+1) * (zSize + 1)];

        for (int i = 0,z = 0; z<= zSize; z++)
        {
            for(int x = 0; x<= xSize; x++)
            {
                float noiseHeight = GenerateNoiseHeight(z, x, octaveOffsets);
                vertices[i] = new Vector3(x, noiseHeight, z);
                i++;
            }
        }
    }

    private Vector2[] GetOffsetSeed()
    {
        seed = UnityEngine.Random.Range(0, 1000);

        System.Random prng = new System.Random();
        Vector2[] octaveOffsets = new Vector2[octaves];

        for(int o = 0;  o < octaves; o++)
        {
            float offsetX = prng.Next(-10000, 10000);
            float offsetY = prng.Next(-10000, 10000);
            octaveOffsets[o] = new Vector2(offsetX, offsetY);
        }
        return octaveOffsets;
    }

    private float GenerateNoiseHeight(int z, int x, Vector2[] octaveOffsets)
    {
        float amplitude = 12;
        float frequency = 1;
        float persistence = 0.5f;
        float noiseHeight = 0;

        for(int y = 0; y< octaves; y++)
        {
            float mapZ = z / scale * frequency + octaveOffsets[y].y;
            float mapX = x / scale * frequency + octaveOffsets[y].x;


            float perlinValue = (Mathf.PerlinNoise(mapZ, mapX)) * 2 - 1;
            noiseHeight += heightCurve.Evaluate(perlinValue) * amplitude;
            frequency *= lacunarity;
            amplitude *= persistence;
        }
        return noiseHeight;
    }

    private void CreateTriangles()
    {
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 2] = triangles[tris+ 3] = vert +1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        GetComponent<MeshCollider>().sharedMesh = mesh;

        gameObject.transform.localScale = new Vector3(MESH_SCALE, MESH_SCALE, MESH_SCALE);
    }


}


