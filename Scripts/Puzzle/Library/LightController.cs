using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private Animator myAnimator;
    private const string LIGHT_ANIM = "Light_On_OFF";

    private bool isStay;
    private bool isOn;

    private void Start()
    {
        isStay = isOn = false;
        myAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SubPlayer")
        {
            SubPlayerController subPlayerController = collision.GetComponent<SubPlayerController>();
            if (subPlayerController != null)
            {
                if (subPlayerController.isSubPlayerAct)
                    isStay = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "SubPlayer")
        {
            SubPlayerController subPlayerController = collision.GetComponent<SubPlayerController>();
            if (subPlayerController != null)
            {
                if (subPlayerController.isSubPlayerAct)
                    isStay = false;
            }
        }
    }

    private void Update()
    {
        if (isStay && !isOn)
        {
            if (Managers.Eyetracker.IsBlink())
            {
                isOn = true;
                myAnimator.SetTrigger(LIGHT_ANIM);
            }
        }
    }
}