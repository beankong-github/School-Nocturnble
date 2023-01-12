using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillAnimCon : MonoBehaviour
{
    private bool animPlay = false;
    public bool animCheck = false;

    Animator animator;

    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        animator.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!animPlay && animCheck)
        {
            animPlay = true;
            animator.gameObject.SetActive(true);

            animator.Rebind();
            animator.Play("PillAnim");
        }

        if (animator != null && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            Destroy(animator.gameObject);
        }
    }
}
