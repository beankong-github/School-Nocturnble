using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimTrigger : MonoBehaviour
{
    public GameObject targetText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cursor")
        {
            if (targetText == null)
                return;

            targetText.GetComponent<TempTrigger>().OnMouseEnter();
        }
    }
}