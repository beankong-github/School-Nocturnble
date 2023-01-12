using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnswerWheel : MonoBehaviour
{
    public GameObject eff_Success1;
    public GameObject eff_Success2;

    private GameObject[] wheelArray;
    private GameObject keyInput;

    public TMP_Text text; // 하단 대사 출력

    private bool correct = false;

    void Start()
    {
        eff_Success1.SetActive(false);
        eff_Success2.SetActive(false);
        keyInput = transform.GetChild(0).gameObject;

        GameObject Wheel = GameObject.Find("gearWheel");
        int wheelCount = Wheel.transform.childCount;

        wheelArray = new GameObject[wheelCount];

        for (int i = 0; i < wheelCount; i++)
        {
            wheelArray[i] = Wheel.transform.GetChild(i).gameObject;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AnswerCheck();
        }
    }

    public void AnswerCheck()
    {
        keyInput.SetActive(true);

        for (int i = 0; i < wheelArray.Length; i++)
        {
            if (wheelArray[i].transform.eulerAngles.z < 1)
            {
                correct = true;
            }
            else
            {
                //Debug.Log("오답:" + wheelArray[i].name + "번 Wheel:" + wheelArray[i].transform.eulerAngles);
                correct = false;
                StartCoroutine(ResultCoroutine());
                return;
            }
        }
        StartCoroutine(ResultCoroutine());
    }

    private IEnumerator ResultCoroutine()
    {
        keyInput.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        if (correct)
        {
            text.text = "정답";
            eff_Success1.SetActive(true);
            eff_Success2.SetActive(true);

            // 기숙사로 씬 전환
        }
        else
        {
            text.text = "다시 생각해보자";
            yield return new WaitForSeconds(1f);

            text.text = "";
            keyInput.SetActive(false);
        }
    }
}
