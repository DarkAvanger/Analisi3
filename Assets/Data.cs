using Gamekit3D;
using Gamekit3D.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using static Gamekit3D.Damageable;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;


public class EllenData
{
    public float positionX;
    public float positionY;
    public float positionZ;
    public int ranged;
    public string enemy;
}
public class Data : MonoBehaviour, IMessageReceiver
{
    float timer;
    int rangedInt = 0;
    public string PHPDamage;
    public string PHPDeath;
    public string PHPMove;
    public GameObject Ellen;
    public GameObject[] Enemies;
    Vector3 EllenMov;


    private void OnEnable()
    {
        Ellen.GetComponent<Damageable>().onDamageMessageReceivers.Add(this);
    }
    public void OnReceiveMessage(MessageType type, object sender, object msg)
    {
        Debug.Log("Received message: " + type);

        Damageable dam = (Damageable)sender;
        DamageMessage damMsg = (DamageMessage)msg;

        

        switch (type)
        {
            case MessageType.DAMAGED:
                {
                    if (damMsg.throwing == true)
                    {
                        rangedInt = 1;
                    }

                    EllenData user = new EllenData
                    {
                        positionX = dam.transform.position.x,
                        positionY = dam.transform.position.y,
                        positionZ = dam.transform.position.z,
                        ranged = rangedInt,
                        enemy = damMsg.damager.name,
                    };

                    string jsonData = JsonUtility.ToJson(user);

                    StartCoroutine(UploadDamage(jsonData));

                    rangedInt = 0;

                }
                break;
            case MessageType.DEAD:
                {
                    EllenData user = new EllenData
                    {
                        positionX = dam.transform.position.x,
                        positionY = dam.transform.position.y,
                        positionZ = dam.transform.position.z,
                        ranged = rangedInt,
                        enemy = damMsg.damager.name,
                    };

                    string jsonData = JsonUtility.ToJson(user);

                    StartCoroutine(UploadDeath(jsonData));
                }
                break;
        }
    }

    void Update()
    {
        // Assuming you have a timer to control the delay
        if (timer >= 1.0f)
        {
            EllenMov = Ellen.transform.position;
            EllenData user = new EllenData
            {
                positionX = EllenMov.x,
                positionY = EllenMov.y,
                positionZ = EllenMov.z,
            };

            string jsonData = JsonUtility.ToJson(user);

            //StartCoroutine(UploadMove(jsonData));

            // Reset the timer after sending the data
            timer = 0.0f;
        }
        else
        {
            // Increment the timer in each frame
            timer += Time.deltaTime;
        }
    }

    IEnumerator UploadDamage(string jsonData)
    {
        WWWForm form = new WWWForm();
        form.AddField("jsonData", jsonData);


        UnityWebRequest www = UnityWebRequest.Post(PHPDamage, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            Debug.Log(jsonData);
        }


    }

    IEnumerator UploadMove(string jsonData)
    {
        WWWForm form = new WWWForm();
        form.AddField("jsonData", jsonData);


        UnityWebRequest www = UnityWebRequest.Post(PHPMove, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            Debug.Log(jsonData);
        }


    }

    IEnumerator UploadDeath(string jsonData)
    {
        WWWForm form = new WWWForm();
        form.AddField("jsonData", jsonData);


        UnityWebRequest www = UnityWebRequest.Post(PHPDeath, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            Debug.Log(jsonData);
        }


    }

}
