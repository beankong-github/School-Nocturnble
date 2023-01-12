using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using System;
using UnityEngine.SceneManagement;

public class SubPlayerController : MonoBehaviour
{
    // == 전역 변수 ==========================================
    private static SubPlayerController instance;

    public BoxCollider2D bound;

    private GameObject player;
    private PlayerController playerCtrl;

    public float SubPlayerSpeed = 100.0f;
    public float VisualizationDistance = 10f;

    [NonSerialized]
    public float targetDir;    // PlayerMain에서 사용자 입력에 따라 보조 캐릭터의 회전값 결정

    [NonSerialized]
    public bool isSubPlayerAct;     // PlayerMain

    private Vector3 minBound; // 박스 컬라이더 영역의 최소 xyz값 저장
    private Vector3 maxBound; // 박스 컬라이더 영역의 최대 xyz값 저장

    public CursorController cursor;
    private CursorController.CursorMode EyetrackerMode;

    public static SubPlayerController MyInstance { get => instance; }

    // == Unity 함수 ===============================================

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }

        SceneController sc = GameObject.Find("@Scene").GetComponent<SceneController>();
        if (sc.sceneType != Define.SceneType.Game)
        {
            Destroy(gameObject);
            return;
        }
        bound = GameObject.FindGameObjectWithTag("CamBound").GetComponent<BoxCollider2D>();
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;

        CursorController.CursorModeChanged -= OnCursorChanged;
        CursorController.CursorModeChanged += OnCursorChanged;

        SetPlayer();

        Managers.Event.SubPlayerGenerated();

        isSubPlayerAct = false;

        if (PlayerData.isDark)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    private void OnCursorChanged()
    {
        EyetrackerMode = CursorController.MyInstance.CurCursorMode;
        OnOffSubPlayer();
    }

    private void Update()
    {
        // subPlayer가 deact면 플레이어를 쫒아다님.
        if (!isSubPlayerAct)
        {
            if (player != null)
                FollowPlayer(targetDir);
        }
        else
            Managers.Eyetracker.MoveGOwithEyetracker(this.gameObject, SubPlayerSpeed);
    }

    // == 기능 함수 ================================================
    public void OnOffSubPlayer()
    {
        if (EyetrackerMode == CursorController.CursorMode.Mouse || EyetrackerMode == CursorController.CursorMode.Eyetracker)
            isSubPlayerAct = false;
        else if (EyetrackerMode == CursorController.CursorMode.SubPlayer)
            isSubPlayerAct = true;
    }

    // SetPlayer <- Start : Player를 찾는다.
    private void SetPlayer()
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerCtrl = player.GetComponent<PlayerController>();
        }
    }

    // FollowPlayer <- Update : Player 따라가는 함수. 아이트래커로 보조 캐릭터를 조종하지 않을 때 사용한다.
    private void FollowPlayer(float dir)
    {
        Vector3 _subPlayerPos = gameObject.transform.position;
        Vector3 _targetPos = player.transform.position + new Vector3(0.0f, 2.0f, 0.0f);

        if (dir == 0.0f)
        {
            _targetPos += new Vector3(1.0f, 0.0f, 0.0f);
            gameObject.transform.position = Vector3.Lerp(_subPlayerPos, _targetPos, Time.deltaTime * SubPlayerSpeed);
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            Transform effect = gameObject.transform.Find("Effect");
            effect.localScale = new Vector3(1f, effect.localScale.y, effect.localScale.z);
        }
        else if (dir == 180.0f)
        {
            _targetPos += new Vector3(-1.0f, 0.0f, 0.0f);
            gameObject.transform.position = Vector3.Lerp(_subPlayerPos, _targetPos, Time.deltaTime * SubPlayerSpeed);
            gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
            Transform effect = gameObject.transform.Find("Effect");
            effect.localScale = new Vector3(-1f, effect.localScale.y, effect.localScale.z);
        }
    }
}