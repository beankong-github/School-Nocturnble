using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEngine.SceneManagement;

public class InfirmaryLockPuzzle : MonoBehaviour
{
    private List<WheelMoveUpdate> wheels;

    public int correct = 1733; // 정답
    private int result; // 사용자가 도출해낸 값
    private int wheelCount = 0;

    public TMP_Text text; // 하단 대사 출력
    public GameObject eff_Success1;
    public GameObject eff_Success2;
    public GameObject keyInput;

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
            Debug.Log(wheels[i].name);
        }
        //ConnectedScene = "Library";
    }

    public void Answer() // Ok버튼
    {
        foreach (var wheel in wheels)
        {
            if (wheel.timer != 0)
            {
                Debug.Log("Wheel 이동 중");
                return;
            }
        }
        Debug.Log("정답확인");
        keyInput.SetActive(true);
        StartCoroutine(ResultCoroutine());
    }

    void AnswerCheck() 
    {
        var tempResult = "";
        foreach (var wheel in wheels)
        {
            tempResult += wheel.num;
        }
        result = int.Parse(tempResult);
        Debug.Log("도출한 답:" + result + "정답:" + correct);
    }

    private IEnumerator ResultCoroutine()
    {
        AnswerCheck();

        if (correct == result)
        {
            text.text = "정답";
            eff_Success1.SetActive(true);
            eff_Success2.SetActive(true);

            // 보건실로 씬 전환
            yield return new WaitForSeconds(1.0f);
            //SceneManager.LoadScene(ConnectedScene);
        }
        else
        {
            text.text = "다시 생각해보자";
        }

        yield return new WaitForSeconds(0.7f);
        text.text = "";
        keyInput.SetActive(false);
    }
}