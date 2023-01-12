using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPoint : MonoBehaviour
{
    public Define.SceneName _connectedSceneName;
    public bool _isDefualtSpwanPoint;

    private PlayerController _PlayerCtrl;
    private CameraController _CameraCtrl;

    private GameObject _player;
    private GameObject _camera;

    private void OnEnable()
    {
        SceneController.PlayerGenerated += SetPlayerAndCam;
    }

    private void SetPlayerAndCam()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _PlayerCtrl = _player.GetComponent<PlayerController>();

        _camera = GameObject.FindGameObjectWithTag("MainCamera");
        _CameraCtrl = _camera.GetComponent<CameraController>();

        SetPlayerPos();
    }

    private void SetPlayerPos()
    {
        Define.SceneName prevSceneName = _PlayerCtrl._prevMapName;

        if (_connectedSceneName == prevSceneName || _isDefualtSpwanPoint)
        {
            float clampedX = Mathf.Clamp(this.transform.position.x, _CameraCtrl._minBound.x + _CameraCtrl._halfWidth, _CameraCtrl._maxBound.x - _CameraCtrl._halfWidth);
            float clampedY = Mathf.Clamp(this.transform.position.y + 4.5f, _CameraCtrl._minBound.y + _CameraCtrl._halfHeight, _CameraCtrl._maxBound.y - _CameraCtrl._halfHeight);
            Vector3 targetPos = new Vector3(clampedX, clampedY, _CameraCtrl.transform.position.z);

            _CameraCtrl.transform.position = targetPos;
            _PlayerCtrl.gameObject.transform.position = this.transform.position;
        }
    }

    private void OnDisable()
    {
        SceneController.PlayerGenerated -= SetPlayerAndCam;
    }
}