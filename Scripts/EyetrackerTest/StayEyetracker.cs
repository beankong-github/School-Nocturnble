using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StayEyetracker : MonoBehaviour
{
    public Animator animator;

    public bool isInteracterble = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInteracterble)
            if (collision.tag == "Cursor")
            {
                StartCoroutine(OnEyeEnter());
            }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isInteracterble)
            if (collision.tag == "Cursor")
            {
                StopAllCoroutines();
                transform.Find("RingBack").gameObject.SetActive(false);
                gameObject.GetComponent<Image>().color = Color.white;
                animator.enabled = false;
            }
    }

    private IEnumerator OnEyeEnter()
    {
        // 카운터 동작
        transform.Find("RingBack").gameObject.SetActive(true);
        animator.enabled = true;

        yield return new WaitForSeconds(3f);

        transform.Find("EnterImage").gameObject.SetActive(true);
        transform.Find("RingBack").gameObject.SetActive(false);

        isInteracterble = false;

        transform.parent.gameObject.GetComponent<EyetrackerTest_stay>().CheckAllStarDone();
    }
}