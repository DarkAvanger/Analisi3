using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderScript : MonoBehaviour
{
    public Material heatmapMaterial;
    public float[,] intensityMap;

    void Start()
    {
        ApplyIntensityMap();
    }

    void ApplyIntensityMap()
    {
        heatmapMaterial.SetFloat("_Intensity", 1.0f); // Adjust as needed
    }
}
