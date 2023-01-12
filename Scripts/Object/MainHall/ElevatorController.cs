using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class ElevatorController : MonoBehaviour
{
    public Transform endPos;
    public Transform startPos;
    public GameObject effect1;
    public GameObject effect2;
    private Transform desPos;

    public float _speed;
    private bool _isTriggerOn;
    private bool _isAct;

    private void Start()
    {
        effect1.SetActive(false);
        effect2.SetActive(false);
        transform.position = startPos.position;
        desPos = endPos;
    }

    private void Update()
    {
        OnKeyboard();

        if (_isAct == true)
            transform.position = Vector2.MoveTowards(transform.position, desPos.transform.position, Time.deltaTime * _speed);

        if (Vector2.Distance(transform.position, desPos.position) <= 0.3f)
        {
            if (desPos == endPos)
            {
                desPos = startPos;
                StartCoroutine(WaitOnTop());
            }
            else
            {
                desPos = endPos;
            }
            effect1.SetActive(false);
            _isAct = false;
        }
    }

    private IEnumerator WaitOnTop()
    {
        yield return new WaitForSeconds(3.0f);
        _isAct = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("E키를 눌러 엘레베이터 활성화");
            effect2.SetActive(true);

            Managers.Resource.Instantiate("Util/E", other.gameObject.transform);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_isAct == false)
                effect2.SetActive(true);
            _isTriggerOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("범위를 벗어났습니다.");
            effect2.SetActive(false);
            _isTriggerOn = false;

            GuideEKey guide = FindObjectOfType<GuideEKey>();
            if (guide != null)
            {
                Destroy(guide.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.gameObject.transform.SetParent(this.transform, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.gameObject.transform.SetParent(null);
            DontDestroyOnLoad(collision.gameObject);
        }
    }

    private void OnKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_isTriggerOn)
            {
                effect1.SetActive(true);
                effect2.SetActive(false);
                _isAct = true;

                GuideEKey guide = FindObjectOfType<GuideEKey>();
                if (guide != null)
                {
                    Destroy(guide.gameObject);
                }
            }
        }
    }
}