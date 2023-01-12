using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairCollisionControl : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
}