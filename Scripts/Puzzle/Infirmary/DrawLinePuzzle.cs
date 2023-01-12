using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DrawLinePuzzle : MonoBehaviour
{
    public Texture2D onMouseCursor;
    public Texture2D exitMouseCursor;

    public Canvas canvas;
    public GameObject linePrefab;

    public GameObject eff_Success1;
    public GameObject eff_Success2;
    public TMP_Text text;

    public GameObject naxtGame;
    public GameObject item;
    private GameObject input;
    private Dictionary<int, StarIdentifier> stars;
    private List<StarIdentifier> lines;

    private GameObject lineLast;
    private RectTransform lineRectT;
    private StarIdentifier starLast;

    private bool isOnMouse = true;
    private bool isCreatLine = false;
    private bool correctAnswer = false;
    private bool chooseSame = true;

    private int numStar = 0;
    private int starCount = 0;

    Coroutine runningCoroutine;

    void Start()
    {
        item.SetActive(false);
        input = GameObject.Find("Input").transform.Find("Key").gameObject;
        Cursor.SetCursor(exitMouseCursor, Vector2.zero, CursorMode.ForceSoftware);

        stars = new Dictionary<int, StarIdentifier>();
        starCount = transform.childCount;

        for (int i = 0; i < starCount; i++)
        {
            var star = transform.GetChild(i);
            var idf = star.GetComponent<StarIdentifier>();

            idf.id = i;

            stars.Add(i, idf);
            Debug.Log(stars[i].name);
        }

        lines = new List<StarIdentifier>();
    }

    void Update()
    {
        if (!isOnMouse)
        {
            isCreatLine = true;
            Vector3 mousePos = canvas.transform.InverseTransformPoint(Input.mousePosition);
            lineRectT.sizeDelta = new Vector2(lineRectT.sizeDelta.x, Vector3.Distance(mousePos, starLast.transform.localPosition));
            lineRectT.rotation = Quaternion.FromToRotation(Vector3.up, (mousePos - starLast.transform.localPosition).normalized);
        }
    }

    GameObject CreateLine(Vector3 pos, int id)
    {
        var line = GameObject.Instantiate(linePrefab, canvas.transform);
        var idf = line.AddComponent<StarIdentifier>();

        line.transform.localPosition = pos;
        idf.id = id;
        lines.Add(idf);
        return line;
    }

    void LineEdit(StarIdentifier star)
    {
        foreach (var line in lines)
        {
            if (line.id == star.id)
            {
                return;
            }
        }

        lineLast = CreateLine(star.transform.localPosition, star.id);
        lineRectT = lineLast.GetComponent<RectTransform>();
        starLast = star;

        star.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void PuzzleReset()
    {
        numStar = 0;
        Camera.main.orthographicSize = 6.75f;
        GameObject.Find("Back").transform.Find("Letter").GetComponent<BoxCollider2D>().enabled = true;
        GameObject.Find("CursorEvent").transform.Find("Magnifier").GetComponent<PolygonCollider2D>().enabled = true;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
        item.SetActive(true);

        foreach (var star in stars)
        {
            star.Value.transform.GetChild(0).gameObject.SetActive(false);
            star.Value.GetComponent<Image>().color = Color.white;
            star.Value.GetComponent<Animator>().enabled = false;
        }

        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }

        lines.Clear();

        lineLast = null;
        lineRectT = null;
        starLast = null;
    }

    bool AnswerCheck(int _id)
    {
        if (_id == numStar)
        {
            numStar++;
            return true;
        }
        else return false;
    }

    void ColorFade(Animator animator)
    {
        animator.enabled = true;
        animator.Rebind();
    }

    public void OnMouseEnterStar(StarIdentifier idf)
    {
        CoroutineEdit();

        foreach (var line in lines)
        {
            if (line.id == idf.id)
            {
                chooseSame = true;
                return;
            }
        }

        Debug.Log(idf);
        Cursor.SetCursor(onMouseCursor, Vector2.zero, CursorMode.ForceSoftware);

        runningCoroutine = StartCoroutine(RingTime(idf));
    }

    public void ExitMouseEnterStar(StarIdentifier idf)
    {
        Cursor.SetCursor(exitMouseCursor, Vector2.zero, CursorMode.ForceSoftware);
        CoroutineEdit();

        if (!chooseSame)
        {
            isOnMouse = false;
        }
        else
        {
            idf.transform.GetChild(1).gameObject.SetActive(false);
            return;
        }

        if (!AnswerCheck(idf.id) && !chooseSame)
        {
            isOnMouse = true;
            isCreatLine = false;
            foreach (var star in stars)
            {
                star.Value.transform.GetChild(0).gameObject.SetActive(false);
                star.Value.GetComponent<Image>().color = new Color(255/255f, 26/255f, 26/255f);
            }
            foreach (var line in lines)
            {
                line.GetComponent<Image>().color = new Color(255 / 255f, 96 / 255f, 96 / 255f);
            }

            Destroy(lines[lines.Count - 1].gameObject);
            lines.RemoveAt(lines.Count - 1);

            runningCoroutine =  StartCoroutine(ResultCoroutine());
            return;
        }

        if (idf.id == starCount - 1)
        {
            isOnMouse = true;
            correctAnswer = true;

            foreach (var line in lines)
            {
                ColorFade(stars[line.id].gameObject.GetComponent<Animator>());
            }

            Destroy(lines[lines.Count - 1].gameObject);
            lines.RemoveAt(lines.Count - 1);

            foreach (var line in lines)
            {
                ColorFade(line.GetComponent<Animator>());
            }

            runningCoroutine = StartCoroutine(ResultCoroutine());
        }

    }

    private void CoroutineEdit() 
    {
        if (runningCoroutine != null) 
        {
            StopCoroutine(runningCoroutine);
        }
    }

    private IEnumerator ResultCoroutine()
    {
        input.SetActive(true);

        if (correctAnswer)
        {
            yield return new WaitForSeconds(3f);
            text.text = "성공";

            foreach (var star in stars)
            {
                star.Value.gameObject.SetActive(false);
            }

            foreach (var line in lines)
            {
                Destroy(line.gameObject);
            }

            Animator animator = GameObject.Find("Letter").GetComponent<Animator>();
            ColorFade(animator);
            
            yield return new WaitForSeconds(10f);
            eff_Success1.SetActive(true);
            eff_Success2.SetActive(true);

            // 보건실로 씬 전환
        }
        else
        {
            text.text = "실패";
            yield return new WaitForSeconds(3f);
            text.text = "";
            PuzzleReset();
            input.SetActive(false);
            naxtGame.SetActive(false);
        }
    }

    private IEnumerator RingTime(StarIdentifier star) 
    {
        chooseSame = true;
        star.transform.GetChild(1).gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        star.transform.GetChild(1).gameObject.SetActive(false);
        isOnMouse = true;
        chooseSame = false;

        if (isCreatLine)
        {
            lineRectT.gameObject.GetComponent<Image>().color = new Color(255 / 255f, 205 / 255f, 38 / 255f);
            lineRectT.sizeDelta = new Vector2(lineRectT.sizeDelta.x, Vector3.Distance(starLast.transform.localPosition, star.transform.localPosition));
            lineRectT.rotation = Quaternion.FromToRotation(Vector3.up, (star.transform.localPosition - starLast.transform.localPosition).normalized);

            LineEdit(star);
        }
        else LineEdit(star);
    }
}