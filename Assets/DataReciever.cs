using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class DataReceiver : MonoBehaviour
{
    [Serializable]
    public class PositionData
    {
        public float positionX;
        public float positionY;
        public float positionZ;
        public float ranged;
        public string enemy;
    }

    [Serializable]
    public class DataObject
    {
        public PositionData[] Data;
    }

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

            DataObject dataList = JsonUtility.FromJson<DataObject>("{\"Data\":" + json + "}");
            for(int i = 0; i < dataList.Data.Length; i++)
            foreach (PositionData data in dataList.Data)
            {
                    //Debug.Log("X: " + data.positionX + " Y: " + data.positionY + " Z: " + data.positionZ);
                    heatmapGenerator.AddPosition(new Vector3(data.positionX, data.positionY, data.positionZ));
            }

            heatmapGenerator.CreateHeatMap();
        }
        else
        {
            Debug.LogError("Error fetching data: " + request.error);
        }
    }
}