using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // == 전역 변수 ============================

    private float speed = 30;       // 이동 속도
    private bool canMove = false;   // 이동이 가능한가?
    private bool eyeEnter = false;
    private Vector3 target;         // 목적지

    private GameObject puzzleCntrl; // SlidePuzzleController 가 적용된 GO

    // == Unity 함수 ============================
    private void Start()
    {
        puzzleCntrl = GameObject.Find("SlidePuzzleController");
    }

    private void Update()
    {
        if (canMove) MoveTile();
    }

    // == 기능 함수 ============================
    private void MoveTile()
    {
        // 목적지로 이동!
        Vector3 pos = transform.position;
        pos = Vector3.MoveTowards(pos, target, speed * Time.deltaTime);
        transform.position = pos;

        // 목적지와 근접하면 이동 종료
        if (Vector3.Distance(pos, target) < 0.05f)
        {
            transform.position = target;

            // SlidePuzzleController에게 이동이 완료됨을 통지
            puzzleCntrl.SendMessage("SetCalc");
            canMove = false;
        }
    }

    // SetMove <- SlidePuzzleController
    private void SetMove(Vector3 _target)
    {
        target = _target;
        canMove = true;
    }

    private void OnMouseDown()
    {
        // On Click
        int n = int.Parse(name.Substring(4));
        puzzleCntrl.SendMessage("SetClick", n);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cursor")
        {
            eyeEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Cursor")
        {
            eyeEnter = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (eyeEnter)
            if (Managers.Eyetracker.IsBlink())
            {
                OnMouseDown();
            }
    }
}