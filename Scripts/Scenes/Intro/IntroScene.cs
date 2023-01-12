using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : BaseScene
{
    public SceneController sceneController;
    public float IntroSec;

    private void Start()
    {
        StartCoroutine("WaitForIntro");
    }

    private IEnumerator WaitForIntro()
    {
        yield return new WaitForSeconds(IntroSec);
        sceneController.TimeChangedScene();
    }

    public override void Clear()
    {
    }
}