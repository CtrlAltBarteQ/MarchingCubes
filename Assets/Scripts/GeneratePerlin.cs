using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noise;

public class GeneratePerlin : MonoBehaviour
{
    public Vector3 size;
    [Range(0f, 1f)]
    public float minValue = 0;

    public MarchCubes mc;

    public GameObject voxelPrefab;
    public Color color;
    public bool useSimplex = true;

    public float noiseScale = .05f;

    public bool sphere = false;
    private List<Mesh> meshes = new List<Mesh>();

    public Vector3 offset;

    public OpenSimplex2S openSimplex2S = new OpenSimplex2S(2432321);

    bool[,,] Generate3D(Vector3 s, Vector3 off, float minVal)
    {
        bool[,,] pointsFilled = new bool[(int)s.x, (int)s.y, (int)s.z];

        for(int x = 0; x < s.x; x++)
        {
            for(int y = 0; y < s.y; y++)
            {
                for(int z = 0; z < s.z; z++)
                {
                    float value = Perlin3D((x + off.x) * noiseScale, (y + off.y) * noiseScale, (z + off.z) * noiseScale);
                    if(value >= minVal)
                    {
                        pointsFilled[x, y, z] = true;
                    }
                }
            }
        }
        return pointsFilled;
    }

    int[,] GenerateHeightmap(Vector3 s, Vector3 off)
    {
        int[,] values = new int[(int)s.x, (int)s.z];

        for (int x = 0; x < s.x; x++) { 
            for(int z = 0; z < s.z; z++)
            {
                values[x, z] = (int)Mathf.Round(Mathf.Clamp(Mathf.PerlinNoise((x + off.x) * noiseScale, (z + off.z) * noiseScale), 0f, 1f) * s.y);
            }
        }
        return values;
    } 



    void Start()
    {
        if (useSimplex)
        {
            bool[,,] points = new bool[(int)size.x, (int)size.y, (int)size.z];
            int[,] heightmap = GenerateHeightmap(size, offset);

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int z = 0; z < size.z; z++) 
                    {
                       double value = openSimplex2S.Noise3_XZBeforeY((x + offset.x) * noiseScale, (y + offset.y) * noiseScale, (z + offset.z) * noiseScale);
                       if(value > minValue)
                        {
                            points[x, y, z] = true;
                        }
                    }
                }
            }

            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    //Instantiate(voxelPrefab, new Vector3(x, heightmap[x, z], z), Quaternion.identity, transform);
                    points[x, heightmap[x, z], z] = true;
                    for (int i = heightmap[x, z] + 1; i < size.y; i++)
                    {
                        points[x, i, z] = false;
                    }
                }
            }

            #region Instanciate
            
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int z = 0; z < size.z; z++)
                    {
                        if (points[x, y, z])
                        {
                            //Debug.Log(x + "+" + y + "+" + z);
                            //GameObject obj = Instantiate(voxelPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
                            //obj.GetComponent<MeshRenderer>().material.color = color;
                        }
                    }
                }
            }



            #endregion

            //Debug.Log(points[0, 10, 14]);

            mc.Generate(points, size);
        }
        else
        {
            bool[,,] points = Generate3D(size, offset, minValue);

            int[,] heightmap = GenerateHeightmap(size, offset);

            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    //Instantiate(voxelPrefab, new Vector3(x, heightmap[x, z], z), Quaternion.identity, transform);
                    for (int i = heightmap[x, z]; i < size.y; i++)
                    {
                        points[x, i, z] = false;
                    }
                }
            }

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int z = 0; z < size.z; z++)
                    {
                        if (points[x, y, z])
                        {
                            //GameObject obj = Instantiate(voxelPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
                            //obj.GetComponent<MeshRenderer>().material.color = color;
                        }
                    }
                }
            }
        }
    }

    public static float Perlin3D(float x, float y, float z)
    {
        float ab = Mathf.PerlinNoise(x, y);
        float bc = Mathf.PerlinNoise(y, z);
        float ac = Mathf.PerlinNoise(x, z);

        float ba = Mathf.PerlinNoise(y, x);
        float cb = Mathf.PerlinNoise(z, y);
        float ca = Mathf.PerlinNoise(z, x);

        float abc = ab + bc + ac + ba + cb + ca;
        return abc / 6f;
    }
}
