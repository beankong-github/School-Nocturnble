using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOutline : MonoBehaviour
{
    private Material outLine;
    private Material defaultMat;

    private void Start()
    {
        outLine = Resources.Load("Materials/object/SpriteOutline", typeof(Material)) as Material;
        defaultMat = GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetComponent<Renderer>().material = outLine;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetComponent<Renderer>().material = defaultMat;
        }
    }

    /*private void OnMouseEnter()
    {
        GetComponent<Renderer>().material = outLine;
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material = defaultMat;
    }*/
}
