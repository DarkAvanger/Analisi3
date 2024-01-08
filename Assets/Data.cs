using Gamekit3D;
using Gamekit3D.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Gamekit3D.Damageable;

public class Data : MonoBehaviour, IMessageReceiver
{
    public GameObject Ellen;
    public GameObject[] Enemies;
    Vector3 EllenPos;

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
                    EllenPos = dam.transform.position;
                    Debug.Log("Ellen Damaged at: " + EllenPos + " by: "+ damMsg.damager.name);
                }
                break;
            case MessageType.DEAD:
                {
                    EllenPos = Ellen.transform.position;
                    Debug.Log("Ellen Dead at: " + EllenPos);
                }
                break;
        }
    }


}
