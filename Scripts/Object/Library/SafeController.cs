using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SafeController : MonoBehaviour
{
    private bool _isTriggerOn = false;
    private bool _isUnlock = false; // false: 잠김 true: 열림

    private void Start()
    {
        _isUnlock = PlayerData.isSafeUnlock;
    }

    private void Update()
    {
        AcceseSafe();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _isTriggerOn = true;
            Managers.Resource.Instantiate("Util/E", collision.gameObject.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _isTriggerOn = false;
            GuideEKey guide = collision.GetComponentInChildren<GuideEKey>();
            if (guide != null)
            {
                Destroy(guide.gameObject);
            }
        }
    }

    private void AcceseSafe()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _isUnlock = PlayerData.isSafeUnlock;
            if (_isTriggerOn && !_isUnlock)
            {
                SceneManager.LoadSceneAsync("Library_Lockpuzzle");
            }

            if (_isTriggerOn && _isUnlock)
            {
                SceneManager.LoadScene("Library_Listpuzzle");
            }
        }
    }
}