using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMoveUpdate : MonoBehaviour
{
    public int num = 0;
    
    public float time = 1.0f;
    public float timer = 0;

    Vector2 startPos;
    Vector2 firstPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (transform.position.y > 22.85 && timer == 0)
        {
            transform.position = startPos;
        }
        else if (transform.position.y < -4 && timer == 0)
        {
            transform.position = new Vector2(startPos.x, 17.25f);
        }
    }

    public void SetNum(string arrow)
    {
        firstPos = transform.position;

        if (arrow == "DOWN" && timer == 0)
        {
            if (num == 0)
                num = 9;
            else
                num--;
            StartCoroutine(WheelMove(firstPos, new Vector2(firstPos.x, transform.position.y - 2.3f), time));
        }
        else if (arrow == "UP" && timer == 0)
        {
            if (num == 9)
                num = 0;
            else
                num++;
            StartCoroutine(WheelMove(firstPos, new Vector2(firstPos.x, transform.position.y + 2.3f), time));
        }

    }

    private IEnumerator WheelMove(Vector2 _firstPos, Vector2 secondPos, float _time)
    {
        Vector2 position = _firstPos;
        transform.position = position;

        while (timer < _time)
        {
            timer += Time.deltaTime;

            position.y = Mathf.Lerp(_firstPos.y, secondPos.y, timer / _time);

            transform.position = position;

            yield return null;
        }

        position = secondPos;
        transform.position = position;

        timer = 0f;
    }
}
