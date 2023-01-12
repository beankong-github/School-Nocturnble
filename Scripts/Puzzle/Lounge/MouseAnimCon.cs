using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAnimCon : MonoBehaviour
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
            animator.Play("MousePuzzleAnim");
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            AnimEdit();
            Destroy(gameObject);
        }
    }

    private void AnimEdit()
    {
        PillAnimCon mouseAnimCon = GameObject.Find("Anim2").GetComponent<PillAnimCon>();
        mouseAnimCon.animCheck = true;
    }
}
