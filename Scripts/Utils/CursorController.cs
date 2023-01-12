using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    public delegate void change();

    public static event change CursorModeChanged; // 캐릭터 생성을 알리는 event

    public delegate void Alert();

    public static event Alert BlinkAlert;         // 눈 깜빡임을 알리는 event

    public static CursorController MyInstance;

    public enum CursorMode
    {
        Mouse,           // 마우스 사용
        Eyetracker,      // 아이트래커 사용
        SubPlayer,       // 보조 캐릭터 조종(아이트래커)
    }

    [NonSerialized]
    public CursorMode CurCursorMode;

    public CursorMode DefualtCursorMode;

    public float speed;

    public bool isSubPlayerExistence;
    public bool isGameObjectMode;
    private bool canChangeCursorMode;

    private Animator animator;
    private Image UI_Eye;
    private GameObject GO_Eye;

    private void Start()
    {
        if (MyInstance == null)
        {
            MyInstance = this;
        }
        else
        {
            if (MyInstance != this)
                Destroy(this.gameObject);
        }

        GO_Eye = transform.Find("GO_Eye").gameObject;
        UI_Eye = this.GetComponentInChildren<Image>();
        animator = this.GetComponentInChildren<Animator>();

        canChangeCursorMode = true;
        CurCursorMode = DefualtCursorMode;
    }

    private void OnEnable()
    {
        Managers.Event.SubPlayerGenerated -= OnSubPlayerGenerated;
        Managers.Event.SubPlayerGenerated += OnSubPlayerGenerated;
    }

    private void Update()
    {
        if (canChangeCursorMode && Input.anyKey)
            OnKeyboard();

        switch (CurCursorMode)
        {
            case CursorMode.Mouse:
                // 눈 모양 스프라이트 끄기
                UI_Eye.gameObject.SetActive(false);
                GO_Eye.SetActive(false);
                // 커서 보이기
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;

            case CursorMode.Eyetracker:
                // 눈 모양 스프라이트 켜기
                if (isGameObjectMode)
                {
                    GO_Eye.SetActive(true);
                    UI_Eye.gameObject.SetActive(false);
                    Managers.Eyetracker.MoveGOwithEyetracker(GO_Eye, speed);
                }
                else
                {
                    GO_Eye.SetActive(false);
                    UI_Eye.gameObject.SetActive(true);
                    Managers.Eyetracker.MoveUIwithEyetracker(UI_Eye.gameObject, speed);
                }

                // 커서 끄기
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case CursorMode.SubPlayer:
                // 커서 끄기
                UI_Eye.gameObject.SetActive(false);
                GO_Eye.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }

        StartCoroutine(CheckBlinkPerSce());
    }

    private IEnumerator CheckBlinkPerSce()
    {
        if (Managers.Eyetracker.IsBlink())
        {
            animator.SetTrigger("Blink");
            Debug.Log("Blink!");
            if (BlinkAlert != null)
                BlinkAlert();
        }

        yield return new WaitForSeconds(1.0f);
    }

    // 키보드 입력 감지
    public void OnKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CurCursorMode++;

            if ((int)CurCursorMode > 2)
                CurCursorMode = 0;

            if (CurCursorMode == CursorMode.SubPlayer && !isSubPlayerExistence)
                CurCursorMode = 0;

            if (CursorModeChanged != null)
                CursorModeChanged();

            Debug.Log($"CurEyetrackerMode : {CurCursorMode.ToString()}");
            return;
        }
    }

    public void FixCursorMode(CursorMode _cursorMode)
    {
        canChangeCursorMode = false;
        CurCursorMode = _cursorMode;
    }

    public void UnFixCursorMode()
    {
        canChangeCursorMode = true;
    }

    private void OnSubPlayerGenerated()
    {
        isSubPlayerExistence = true;
    }
}