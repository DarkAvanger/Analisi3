using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public void colorChange(Color nuevoColor)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = nuevoColor;
        }
        else
        {
            renderer.material.color = Color.green;
        }
    }
}
