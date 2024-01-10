using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DataReceiver : MonoBehaviour
{
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
            // Parse the JSON data in Unity and use it as needed
            Debug.Log(json);
        }
        else
        {
            Debug.LogError("Error fetching data: " + request.error);
        }
    }
}
