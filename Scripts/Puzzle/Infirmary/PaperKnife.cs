using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PaperKnife : MonoBehaviour
{
    public Texture2D MagnifierCursor;
    public Texture2D PaperknifeCursor;
    public Material outLineOff;

    public GameObject eff_Success1;
    public GameObject eff_Success2;
    public TMP_Text text;

    public GameObject nextGame;
    public GameObject countDown;
    public GameObject circleNum;
    private GameObject ruleScreen;
    private GameObject keyInput;
    private GameObject tempHit = null;

    private RaycastHit2D hit;
    private Camera cam;
    private Animation camAni;
    private Coroutine runningCoroutine = null;
    private bool isKnifeCursor = false;

    private void Awake()
    {
        cam = Camera.main;
        camAni = cam.transform.GetComponent<Animation>();
    }

    private void Start()
    {
        keyInput = transform.Find("InputKey").gameObject;
        ruleScreen = transform.Find("DescriptionScreen").gameObject;
        nextGame.SetActive(false);

        EffectEdit();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                if (hit.collider.name == "Paper knife")
                {   
                    isKnifeCursor = true;
                    Cursor.SetCursor(PaperknifeCursor, Vector2.zero, CursorMode.ForceSoftware);
                    hit.collider.gameObject.SetActive(false);
                    hit.collider.GetComponent<Renderer>().material = outLineOff;
                    ObjHitEdit();
                }
                else if (hit.collider.name == "Magnifier")
                {
                    isKnifeCursor = false;
                    Cursor.SetCursor(MagnifierCursor, Vector2.zero, CursorMode.ForceSoftware);
                    hit.collider.gameObject.SetActive(false);
                    ObjHitEdit();
                }
                else if (hit.collider.name == "Letter")
                {
                    if (isKnifeCursor)
                    {
                        transform.Find("Magnifier").GetComponent<PolygonCollider2D>().enabled = false;
                        hit.collider.GetComponent<BoxCollider2D>().enabled = false;
                        keyInput.SetActive(true);
                        ruleScreen.SetActive(true);
                        eff_Success1.SetActive(true);
                    }
                    else CoroutineEdit();
                }
            }
        }
    }

    public void StartBtn()
    {
        eff_Success1.SetActive(false);
        CoroutineEdit();
    }

    private void CoroutineEdit() 
    {
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
        runningCoroutine = StartCoroutine(ResultCoroutine());
    }

    private void ObjHitEdit()
    {
        if (tempHit != null)
        {
            tempHit.GetComponent<Renderer>().material = outLineOff;
            tempHit.SetActive(true);
            tempHit = hit.collider.gameObject;
        }
        else tempHit = hit.collider.gameObject;
    }

    private void EffectEdit()
    {
        if (isKnifeCursor)
        {
            eff_Success1.SetActive(true);
            eff_Success2.SetActive(true);
        }
        else
        {
            eff_Success1.SetActive(false);
            eff_Success2.SetActive(false);
        }
    }

    private IEnumerator ResultCoroutine()
    {
        text.text = "";
        yield return new WaitForSeconds(0.1f);

        if (isKnifeCursor)
        {
            ruleScreen.SetActive(false);
            tempHit = null;
            camAni.Play("CameraZoomIn");
            EffectEdit();
            yield return new WaitForSeconds(1f);

            isKnifeCursor = false;
            EffectEdit();
            countDown.SetActive(true);
            circleNum.SetActive(true);
            yield return new WaitForSeconds(3f);

            keyInput.SetActive(false);
            nextGame.SetActive(true);
            yield return new WaitForSeconds(.5f);

            circleNum.SetActive(false);
            countDown.SetActive(false);
        }
        else
        {
            text.text = "다시 생각해보자";
            yield return new WaitForSeconds(1f);
            text.text = "";
            keyInput.SetActive(false);
        }
    }
}
