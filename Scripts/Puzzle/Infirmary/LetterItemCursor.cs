using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterItemCursor : MonoBehaviour
{
    public Material outLineOn;
    public Material outLineOff;

    private void OnMouseEnter()
    {
        GetComponent<Renderer>().material = outLineOn;
    }

    private void OnMouseDown()
    {
        GetComponent<Renderer>().material = outLineOff;
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material = outLineOff;
    }
}
