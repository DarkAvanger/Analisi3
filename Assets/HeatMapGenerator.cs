using UnityEngine;
using System.Collections.Generic;

public class HeatmapGenerator : MonoBehaviour
{
    public Material heatmapMaterial;
    private List<Vector3> positions = new List<Vector3>();

    public void CreateHeatMap()
    {
        foreach(Vector3 pos in positions)
        {
            Debug.Log(pos.x + " " + pos.y + " " + pos.z);
        }
        // Aquí, llama a CreateHeatmapTexture con tus datos de posición
        Texture2D heatmapTexture = CreateHeatmapTexture(positions, 256, 256);
        heatmapMaterial.mainTexture = heatmapTexture;
    }

    public void AddPosition(Vector3 position)
    {
        positions.Add(position);
        // Opcionalmente, puedes actualizar la textura aquí si quieres que se actualice en tiempo real
    }

    Texture2D CreateHeatmapTexture(List<Vector3> positions, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);

        // Initialize texture
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }

        // Map positions to texture
        foreach (var position in positions)
        {
            // Convert 3D position to 2D
            int x = (int)(position.x * width); // Adjust these calculations based on your world's scale
            int y = (int)(position.z * height); // Using X and Z for a top-down view

            Color currentColor = texture.GetPixel(x, y);
            currentColor += new Color(1, 0, 0, 0.1f); // Increase red channel, adjust alpha for intensity
            texture.SetPixel(x, y, currentColor);
        }

        texture.Apply();
        return texture;
    }
}

