using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LibraryListPuzzle : MonoBehaviour
{
    public GameObject eff_Success1;
    public GameObject eff_Success2;
    private GameObject keyInput;

    public InputField field; // 값 입력 받는 InputField
    public TMP_Text text; // 하단 대사 출력
    public SceneController sceneController;

    private Coroutine runningCoroutine;

    private void Start()
    {
        keyInput = transform.Find("KeyInput").gameObject;
        eff_Success1.SetActive(false);
        eff_Success2.SetActive(false);

        field.ActivateInputField(); //Field 활성화
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) //키보드 엔터
        {
            Answer();
        }
    }

    public void Answer() // Ok 버튼
    {
        if (field.text != "")
        {
            CoroutineCheck();
            keyInput.SetActive(true);
            field.enabled = false;
            runningCoroutine = StartCoroutine(ResultCoroutine());
        }
        else
        {
            field.ActivateInputField();
        }
    }

    private void CoroutineCheck()
    {
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
    }

    private IEnumerator ResultCoroutine()
    {
        yield return new WaitForSeconds(0.1f);

        if (field.text == "J113" || field.text == "j113")
        {
            text.text = "정답";
            eff_Success1.SetActive(true);
            eff_Success2.SetActive(true);

            // 성공 저장
            PlayerData.isListDone = true;

            // 도서관으로 씬 전환
            sceneController.TimeChangedScene();
        }
        else
        {
            text.text = "다시 생각해보자";

            yield return new WaitForSeconds(1.3f);
            field.enabled = true;
            field.text = "";
            text.text = "";
            field.ActivateInputField();
            keyInput.SetActive(false);
        }
    }
}