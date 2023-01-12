using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTrigger : MonoBehaviour
{
    private Animator myAnimator;
    private const string TEXT_ANIM = "Text_Trigger";
    public SceneController SceneController;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    public void OnMouseEnter()
    {
        myAnimator.SetTrigger(TEXT_ANIM);

        if (SceneController != null)
            Invoke("Scenetransition", 2.0f);
    }

    private void Scenetransition()
    {
        SceneController.TimeChangedScene();
    }
}