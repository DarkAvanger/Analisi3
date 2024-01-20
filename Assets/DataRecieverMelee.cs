using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataReceiver3Melee : MonoBehaviour
{
    public GameObject cubeHeat;
    private List<Vector3> lastPos = new List<Vector3>();
    int count = 0;

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

    public void StartDataFetching()
    {
        
        StartCoroutine(GetDataFromServer());

    }
    public void ClearObjects()
    {
        foreach (Vector3 pos in lastPos)
        {
            GameObject obj = FindObjectAtPosition(pos);
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        lastPos.Clear();
    }

    IEnumerator GetDataFromServer()
    {
        string url = "https://citmalumnes.upc.es/~laiapp4/UnityPHPDead.php";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;

            DataObject dataList = JsonUtility.FromJson<DataObject>("{\"Data\":" + json + "}");

            foreach (PositionData data in dataList.Data)
            {
                Vector3 posicionSpawn = new Vector3(Mathf.Round(data.positionX), Mathf.Round(data.positionY), Mathf.Round(data.positionZ));

                if (data.ranged == 0)
                {
                    if (lastPos.Contains(posicionSpawn))
                    {
                        ChangeColor(posicionSpawn);
                    }
                    else
                    {
                        GameObject nuevoObjeto = Instantiate(cubeHeat, posicionSpawn, Quaternion.identity);
                        lastPos.Add(posicionSpawn);
                    }
                }
            }

        }
        else
        {
            Debug.LogError("Error fetching data: " + request.error);
        }
    }

    int CountRepetitions(Vector3 posicion)
    {
        // Contamos cuántas veces se ha repetido la posición

        foreach (Vector3 pos in lastPos)
        {
            if (pos == posicion)
            {
                count++;
            }
        }
        return count;
    }

    void ChangeColor(Vector3 posicion)
    {
        GameObject gameObject = FindObjectAtPosition(posicion);

        if (gameObject != null)
        {
            Debug.Log("Objeto encontrado en la posición: " + posicion);

            ColorChanger changeColor = gameObject.GetComponentInChildren<ColorChanger>();

            if (changeColor != null)
            {
                int repeticiones = CountRepetitions(posicion);
                Debug.Log(repeticiones);
                if (repeticiones > 30)
                {
                    changeColor.colorChange(Color.red);
                }
                else if (repeticiones > 1)
                {
                    changeColor.colorChange(Color.yellow);
                }
            }
            else
            {
                Debug.LogError("El objeto en la posición " + posicion + " no tiene un componente ColorChanger.");
            }
        }
        else
        {
            Debug.LogError("No se pudo encontrar el objeto en la posición: " + posicion);
        }
    }



    GameObject FindObjectAtPosition(Vector3 posicion)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("YourCubeTag"))
        {
            if (Vector3.Distance(obj.transform.position, posicion) < 0.1f)
            {
                return obj;
            }
        }
        return null;
    }
}

