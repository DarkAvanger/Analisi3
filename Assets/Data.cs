using Gamekit3D;
using Gamekit3D.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;
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
    int rangedInt = 0;
    public string serverUrl;
    public GameObject Ellen;
    public GameObject[] Enemies;
    Vector3 EllenPos;
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
                    //EllenPos = dam.transform.position;
                    //Debug.Log("Ellen Damaged at: " + EllenPos + " by: "+ damMsg.damager.name);
                    //Debug.Log( "Melee or ranged: " + damMsg.throwing);
                    //Debug.Log("From: " + damMsg.direction);
                    //Debug.Log("From: " + damMsg.damageSource);
                    //Debug.Log("Amount: " + damMsg.amount);
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

                    StartCoroutine(Upload(jsonData));

                    rangedInt = 0;

                }
                break;
            case MessageType.DEAD:
                {
                   // EllenPos = dam.transform.position;
                    //Debug.Log("Ellen Dead at: " + EllenPos + " by: " + damMsg.damager.name);
                    //Debug.Log("Melee or ranged: " + damMsg.throwing);
                    //Debug.Log("From: " + damMsg.direction);
                    //Debug.Log("From: " + damMsg.damageSource);
                    //Debug.Log("Amount: " + damMsg.amount);
                    EllenData user = new EllenData
                    {
                        positionX = dam.transform.position.x,
                        positionY = dam.transform.position.y,
                        positionZ = dam.transform.position.z,
                        ranged = rangedInt,
                        enemy = damMsg.damager.name,
                    };

                    string jsonData = JsonUtility.ToJson(user);
                }
                break;
        }
    }

    void Update()
    {
        EllenMov = Ellen.transform.position;
        //Debug.Log(EllenMov);
    }

    IEnumerator Upload(string jsonData)
    {
        WWWForm form = new WWWForm();
        form.AddField("jsonData", jsonData);


        UnityWebRequest www = UnityWebRequest.Post(serverUrl, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log("Form upload complete!");
            //Debug.Log("Data uploaded successfully!");
            Debug.Log(www.downloadHandler.text);
            Debug.Log(jsonData);
        }


    }

}
