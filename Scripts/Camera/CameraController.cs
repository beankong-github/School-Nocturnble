using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    // == 전역 변수 ==============================
    private static CameraController camera;

    public GameObject _target;
    public BoxCollider2D _bound;
    private Camera _theCamera;

    [SerializeField]
    private float _camSpeed = 3.0f;

    [SerializeField]
    private bool FixYAxis = false;

    [NonSerialized]
    public Vector3 _minBound; // 박스 컬라이더 영역의 최소 xyz값 저장

    [NonSerialized]
    public Vector3 _maxBound; // 박스 컬라이더 영역의 최대 xyz값 저장

    [NonSerialized]
    public float _halfWidth;  // 카메라의 반너비

    [NonSerialized]
    public float _halfHeight; // 카메라의 반높이

    private void Start()
    {
        // 카메라 싱글턴
        if (camera == null)
        {
            camera = this;
        }
        else
            Destroy(this.gameObject);

        // Scene이 바뀌는 이벤트 구독+
        SceneManager.activeSceneChanged -= ChangedActiveScene;
        SceneManager.activeSceneChanged += ChangedActiveScene;

        _theCamera = this.GetComponent<Camera>();
        _halfHeight = _theCamera.orthographicSize;
        _halfWidth = _halfHeight * Screen.width / Screen.height;

        SetBound();
    }

    // Scene이 바뀌면 호출
    private void ChangedActiveScene(Scene current, Scene next)
    {
        Define.SceneType newSceneType = GameObject.Find("@Scene").GetComponent<SceneController>().sceneType;

        if (newSceneType == Define.SceneType.Game)
            SetBound();
    }

    private void SetBound()
    {
        if (_bound == null)
        {
            _bound = GameObject.FindGameObjectWithTag("CamBound").GetComponent<BoxCollider2D>();
        }

        if (_bound != null)
        {
            _minBound = _bound.bounds.min;
            _maxBound = _bound.bounds.max;
        }
    }

    private void Update()
    {
        if (_target == null)
        {
            Debug.Log("Failed to load player character");
            return;
        }

        SetCameraPos();
        OnKeyboard();
    }

    private void SetCameraPos()
    {
        if (!FixYAxis)
        {    // 플레이어 따라 카메라 움직임
            float clampedX = Mathf.Clamp(_target.transform.position.x, _minBound.x + _halfWidth, _maxBound.x - _halfWidth);
            float clampedY = Mathf.Clamp(_target.transform.position.y + 4f, _minBound.y + _halfHeight, _maxBound.y - _halfHeight);
            Vector3 targetPos = new Vector3(clampedX, clampedY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, _camSpeed * Time.deltaTime);
        }
        else
        {
            // x축만 플레이어를 따라 움직임
            float clampedX = Mathf.Clamp(_target.transform.position.x, _minBound.x + _halfWidth, _maxBound.x - _halfWidth);
            Vector3 targetPos = new Vector3(clampedX, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, _camSpeed * Time.deltaTime);
        }
    }

    private void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            float targetPos_up = Mathf.Clamp(transform.position.y + 0.8f, _minBound.y + _halfHeight, _maxBound.y - _halfHeight);
            Vector3 targetPos = new Vector3(transform.position.x, targetPos_up, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _camSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            float targetPos_down = Mathf.Clamp(transform.position.y - 0.8f, _minBound.y + _halfHeight, _maxBound.y - _halfHeight);
            Vector3 targetPos = new Vector3(transform.position.x, targetPos_down, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _camSpeed);
        }
    }
}