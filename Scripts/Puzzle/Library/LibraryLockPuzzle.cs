using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LibraryLockPuzzle : BaseScene
{
    private List<WheelMoveUpdate> wheels;

    public int correct = 29862;
    private int result; 
    private int wheelCount = 0;

    public TMP_Text text; 
    public GameObject eff_Success1;
    public GameObject eff_Success2;
    public GameObject keyInput;
    public SceneController sceneController;

    public Define.SceneName ConnectedScene;

    private void Start()
    {
        eff_Success1.SetActive(false);
        eff_Success2.SetActive(false);

        wheels = new List<WheelMoveUpdate>();
        wheelCount = transform.childCount;

        for (int i = 0; i < wheelCount; i++)
        {
            var wheel = transform.GetChild(i);
            var idf = wheel.GetComponent<WheelMoveUpdate>();

            wheels.Add(idf);
        }
    }

    public void Answer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.timer != 0)
            {
                return;
            }
        }
        keyInput.SetActive(true);
        StartCoroutine(ResultCoroutine());
    }

    private void AnswerCheck()
    {
        var tempResult = "";
        foreach (var wheel in wheels)
        {
            tempResult += wheel.num;
        }
        result = int.Parse(tempResult);
    }

    private IEnumerator ResultCoroutine()
    {
        AnswerCheck();

        if (correct == result)
        {
            text.text = "정답";
            eff_Success1.SetActive(true);
            eff_Success2.SetActive(true);

            PlayerData.isSafeUnlock = true;
            yield return new WaitForSeconds(1.0f);
            sceneController.nextScene = ConnectedScene;
            sceneController.TimeChangedScene();
        }
        else
        {
            text.text = "다시 생각해보자";
        }

        yield return new WaitForSeconds(0.7f);
        text.text = "";
        keyInput.SetActive(false);
    }

    public override void Clear()
    {
    }
}