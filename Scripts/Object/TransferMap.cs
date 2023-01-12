using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public Define.SceneName _NextSceneName;
    public Define.TransferMapType _transferMapType;
    public bool _canTransfer = true;

    private bool _isPlayerEnter = false;

    private SceneController _SceneController;
    private PlayerController _thePlayer;

    private GameObject guide;

    private void Start()
    {
        _SceneController = GameObject.Find("@Scene").GetComponent<SceneController>();
        if (_SceneController == null)
        {
            Debug.Log("SceneController 연결 실패. @Scene 오브젝트에 SceneController 클래스를 추가하세요.");
        }

        _thePlayer = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_thePlayer == null)
        {
            _thePlayer = FindObjectOfType<PlayerController>();
        }

        if (collision.tag == "Player")
        {
            if (_transferMapType == Define.TransferMapType.Smooth)
            {
                _isPlayerEnter = false;
                _thePlayer._prevMapName = _SceneController.sceneName;
                _SceneController.nextScene = _NextSceneName;
                _SceneController.TimeChangedScene();
            }

            if (_transferMapType == Define.TransferMapType.Door && _canTransfer)
            {
                guide = Managers.Resource.Instantiate("Util/E", collision.gameObject.transform);
            }
            _isPlayerEnter = true;
        }
    }

    private void Update()
    {
        if (_transferMapType == Define.TransferMapType.Door)
        {
            if (Input.GetKey(KeyCode.E) && _isPlayerEnter && _canTransfer)
            {
                if (guide == null)
                {
                    GuideEKey e = GameObject.FindObjectOfType<GuideEKey>();
                    if (e != null)
                        guide = e.gameObject;
                }
                else
                {
                    Destroy(guide);
                }
                _thePlayer._prevMapName = _SceneController.sceneName;
                _SceneController.nextScene = _NextSceneName;
                _SceneController.TimeChangedScene();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (_transferMapType == Define.TransferMapType.Door && _canTransfer)
            {
                guide = GameObject.FindObjectOfType<GuideEKey>().gameObject;
                if (guide != null)
                {
                    Destroy(guide);
                }
            }

            _isPlayerEnter = false;
        }
    }
}