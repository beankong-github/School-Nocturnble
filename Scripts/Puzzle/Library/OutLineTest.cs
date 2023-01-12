using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineTest : MonoBehaviour
{
    private Material outLine;
    private Material defaultMat;

    private void Start()
    {
        outLine = Resources.Load("Materials/object/Book_outline", typeof(Material)) as Material;
        defaultMat = Resources.Load("Materials/object/BookDefault", typeof(Material)) as Material;
    }

    private void OnMouseEnter()
    {
        GetComponent<Renderer>().material = outLine;
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material = defaultMat;
    }
}
