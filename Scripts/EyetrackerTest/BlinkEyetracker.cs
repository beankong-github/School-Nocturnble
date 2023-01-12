using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkEyetracker : MonoBehaviour
{
    public Animator animator;

    public bool isInteracterble = true;
    private bool isReady = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInteracterble)
            if (collision.tag == "Cursor")
            {
                isReady = true;
                gameObject.GetComponent<Image>().color = Color.yellow;
            }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isInteracterble)
            if (collision.tag == "Cursor")
            {
                isReady = false;
                gameObject.GetComponent<Image>().color = Color.white;
            }
    }

    private void Update()
    {
        if (isInteracterble && isReady)
            if (Managers.Eyetracker.IsBlink())
            {
                transform.Find("EnterImage").gameObject.SetActive(true);
                isInteracterble = false;

                transform.parent.gameObject.GetComponent<EyetrackerTest_blink>().CheckAllStarDone();
            }
    }
}