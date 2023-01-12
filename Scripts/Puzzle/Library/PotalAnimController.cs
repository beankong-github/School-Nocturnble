using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalAnimController : MonoBehaviour
{
    public GameObject on_Effect;
    public GameObject off_Effect;

    void Start()
    {
        on_Effect.SetActive(false);
        off_Effect.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            on_Effect.SetActive(true);
            off_Effect.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            on_Effect.SetActive(false);
            off_Effect.SetActive(true);
        }
    }
}
