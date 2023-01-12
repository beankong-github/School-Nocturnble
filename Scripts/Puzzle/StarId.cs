using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarId: MonoBehaviour
{
    public StarPattern star;

    public int id;
    public bool isInteractable;

    private Animator animator;

    private void Start()
    {
        isInteractable = true;
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cursor")
        {
            if (isInteractable)
            {
                StartCoroutine(OnEyeEnter());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isInteractable)
        {
            if (collision.tag == "Cursor")
            {
                StopAllCoroutines();
                transform.Find("RingBack").gameObject.SetActive(false);
                gameObject.GetComponent<Image>().color = Color.white;
                animator.enabled = false;
            }
        }
    }

    private IEnumerator OnEyeEnter()
    {
        // 카운터 동작
        transform.Find("RingBack").gameObject.SetActive(true);
        animator.enabled = true;

        yield return new WaitForSeconds(2f);

        Debug.Log(id + "번째 별 잠금 해제!");
        transform.Find("EnterImage").gameObject.SetActive(true);
        transform.Find("RingBack").gameObject.SetActive(false);

        isInteractable = false;
        star.StaySuccess(this);
    }
}