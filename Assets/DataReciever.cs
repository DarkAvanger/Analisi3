using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class PositionData
{
    public float positionX;
    public float positionY;
    public float positionZ;
    public PositionData[] data;
}


public class DataReceiver : MonoBehaviour
{
    public HeatmapGenerator heatmapGenerator;

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
            PositionData dataList = JsonUtility.FromJson<PositionData>("{\"data\":" + json + "}");
            foreach (var data in dataList.data)
            {
                heatmapGenerator.AddPosition(new Vector3(data.positionX, data.positionY, data.positionZ));
            }
        }
        else
        {
            Debug.LogError("Error fetching data: " + request.error);
        }
    }
}