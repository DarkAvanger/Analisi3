/*using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DataReceiver2 : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetDataFromServer());
    }

    IEnumerator GetDataFromServer()
    {
        string url = "https://citmalumnes.upc.es/~laiapp4/UnityPHPMove.php";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            // Parse the JSON data in Unity and use it as needed
            Debug.Log(json);
        }
        else
        {
            Debug.LogError("Error fetching data: " + request.error);
        }
    }
}*/

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DataReceiver2 : MonoBehaviour
{
    public Material heatmapMaterial;
    public GameObject plane; // Reference to your plane object

    void Start()
    {
        StartCoroutine(GetDataFromServer());
    }

    IEnumerator GetDataFromServer()
    {
        string url = "https://citmalumnes.upc.es/~laiapp4/UnityPHP.php";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            ProcessDataAndVisualize(json);
        }
        else
        {
            Debug.LogError("Error fetching data: " + request.error);
        }
    }

    void ProcessDataAndVisualize(string json)
    {
        try
{
    DataContainer dataContainer = JsonUtility.FromJson<DataContainer>(json);
    float[,] intensityMap = dataContainer.intensityMap;
    // Rest of your code...
}
catch (System.Exception e)
{
    Debug.LogError("Error parsing JSON: " + e.Message);
}

    }

    void ApplyIntensityMap(float[,] intensityMap)
    {
        // Get the plane's mesh renderer
        MeshRenderer planeRenderer = plane.GetComponent<MeshRenderer>();

        // Set the intensity map to the shader
        heatmapMaterial.SetFloat("_Intensity", 1.0f); // Adjust as needed
        heatmapMaterial.SetTexture("_MainTex", GenerateTexture(intensityMap));

        // Assign the material to the plane's renderer
        planeRenderer.material = heatmapMaterial;
    }

    Texture2D GenerateTexture(float[,] intensityMap)
    {
        // Create a texture based on the intensity map
        int width = intensityMap.GetLength(0);
        int height = intensityMap.GetLength(1);
        Texture2D texture = new Texture2D(width, height);

        // Set colors based on intensity values
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = Color.Lerp(Color.blue, Color.red, intensityMap[x, y]);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply(); // Apply changes

        return texture;
    }

    [System.Serializable]
    public class DataContainer
    {
        public float[,] intensityMap;
    }
}

