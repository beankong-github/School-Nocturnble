using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CursorEvent : MonoBehaviour
{
    public Texture2D MagnifierCursor; // 돋보기 커서 이미지
    public Texture2D PaperknifeCursor; // 종이칼 커서 이미지
    public Texture2D GeneralCursor;

    public TMP_Text text;// 하단 텍스트

    private RaycastHit2D hit;
    private Camera cam;

    private Coroutine runningCoroutine = null;
    private bool isKnifeCursor = false; // 현재 커서가 종이칼 커서인지

    private GameObject temp;

    private void Awake()
    {
        Cursor.SetCursor(GeneralCursor, Vector2.zero, CursorMode.ForceSoftware);
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            tempEdit();

            if (hit.collider != null)
            {
                if (hit.collider.name == "Paper knife") // 클릭했을 때 종이칼인 경우
                {
                    isKnifeCursor = true;
                    Cursor.SetCursor(PaperknifeCursor, Vector2.zero, CursorMode.ForceSoftware); // 커서 모양을 종이칼로 변경
                    temp = hit.collider.gameObject;
                    hit.collider.gameObject.SetActive(false);
                }
                else if (hit.collider.name == "Magnifier") // 클릭했을 때 돋보기인 경우
                {
                    isKnifeCursor = false;
                    Cursor.SetCursor(MagnifierCursor, Vector2.zero, CursorMode.ForceSoftware); // 커서 모양을 돋보기로 변경
                    temp = hit.collider.gameObject;
                    hit.collider.gameObject.SetActive(false);
                }
                else if (hit.collider.name == "Letter") // 클릭했을 때 편지인 경우
                {
                    tempEdit();
                    CoroutineEdit();
                }
            }
            else isKnifeCursor = false;
        }
    }

    private void tempEdit() 
    {
        if (temp != null)
        {
            Cursor.SetCursor(GeneralCursor, Vector2.zero, CursorMode.ForceSoftware);
            temp.SetActive(true);
            temp = null;
        }
    }

    private void CoroutineEdit()
    {
        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);
        runningCoroutine = StartCoroutine(ResultCoroutine());
    }

    private IEnumerator ResultCoroutine()
    {
        text.text = "";
        yield return new WaitForSeconds(0.1f);
        Cursor.SetCursor(GeneralCursor, Vector2.zero, CursorMode.ForceSoftware);

        if (isKnifeCursor)
        {
            Animation camAni = cam.transform.GetComponent<Animation>();
            camAni.Play("CameraZoomIn"); // 메인 카메라에 있는 애니메이션 실행

            Managers.Resource.Instantiate("Puzzle/DescriptionScreen");
            Destroy(gameObject);
        }
        else
        {
            text.text = "다시 생각해보자";
            yield return new WaitForSeconds(1f);
            text.text = "";
        }
    }
}
