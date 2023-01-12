using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyetrackerTest_blink : MonoBehaviour
{
    private List<BlinkEyetracker> childs_blinkEyetracker;
    private SceneController sceneController;

    private void Start()
    {
        childs_blinkEyetracker = new List<BlinkEyetracker>();
        sceneController = FindObjectOfType<SceneController>();

        for (int i = 0; i < transform.childCount; i++)
        {
            BlinkEyetracker tmp;
            transform.GetChild(i).TryGetComponent(out tmp);

            if (tmp != null)
                childs_blinkEyetracker.Add(tmp);
        }
    }

    public void CheckAllStarDone()
    {
        for (int i = 0; i < childs_blinkEyetracker.Count; i++)
        {
            if (childs_blinkEyetracker[i].isInteracterble)
                return;
        }

        Destroy(gameObject, 0.2f);
        ChangeScene();
    }

    private void ChangeScene()
    {
        FindObjectOfType<CursorController>().UnFixCursorMode();
        sceneController.TimeChangedScene();
    }
}