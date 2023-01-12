using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarPattern : MonoBehaviour
{
    private Dictionary<int, StarId> stars;

    private List<StarId> lines;

    public GameObject linePrefab;
    public Canvas canvas;
    private GameObject eye;

    private GameObject lineOnEdit;
    private RectTransform lineOnEditRectTrans;
    private StarId starOnEdit;

    private bool isConnectable;

    private void Start()
    {
        //CursorController.MyInstance.FixCursorMode(CursorController.CursorMode.Eyetracker);

        stars = new Dictionary<int, StarId>();
        lines = new List<StarId>();

        for (int i = 0; i < transform.childCount; i++)
        {
            StarId child = transform.GetChild(i).gameObject.GetComponent<StarId>();
            stars.Add(child.id, child);
        }

        isConnectable = false;
    }

    private void Update()
    {
        if (isConnectable)
        {
            if (eye == null)
                eye = GameObject.Find("Eye");

            // 커서 위치에 따라 선 길이 및 회전 설정
            RectTransform CursorRectTransform = eye.GetComponent<RectTransform>();
            lineOnEditRectTrans.sizeDelta = new Vector2(lineOnEditRectTrans.sizeDelta.x, Vector3.Distance(CursorRectTransform.localPosition, starOnEdit.transform.localPosition));
            lineOnEditRectTrans.rotation = Quaternion.FromToRotation(Vector3.up, (CursorRectTransform.position - starOnEdit.transform.position).normalized);
        }
    }

    public void OnEyeEnter(StarId star)
    {
        if (lineOnEditRectTrans != null && starOnEdit != null)
        {
            lineOnEditRectTrans.sizeDelta = new Vector2(lineOnEditRectTrans.sizeDelta.x, Vector3.Distance(starOnEdit.transform.localPosition, star.transform.localPosition));
            lineOnEditRectTrans.rotation = Quaternion.FromToRotation(Vector3.up, (star.transform.localPosition - starOnEdit.transform.localPosition).normalized);
        }
    }

    public void StaySuccess(StarId star)
    {
        isConnectable = true;
        OnEyeEnter(star);

        TrySetLineEdit(star);
    }

    private void TrySetLineEdit(StarId star)
    {
        if (!TryFinish())
            StartCoroutine(WrongAnswer());

        foreach (StarId line in lines)
        {
            if (line.id == star.id)
                return;
        }

        lineOnEdit = CreateLine(star.transform.localPosition, star.id);
        lineOnEditRectTrans = lineOnEdit.GetComponent<RectTransform>();
        starOnEdit = star;

        // 마지막 라인 일 경우
        if (lines.Count >= transform.childCount && lines[lines.Count - 1].id == star.id)
        {
            isConnectable = false;

            if (!TryFinish())
                StartCoroutine(WrongAnswer());
            else
                StartCoroutine(CorrectAnswer());
        }
    }

    private GameObject CreateLine(Vector3 startPos, int id)
    {
        // 라인 생성 및 위치 조정
        GameObject line = GameObject.Instantiate(linePrefab, canvas.transform);
        line.transform.localPosition = startPos;

        // 라인 리스트에 보관
        StarId lineIdf = line.GetOrAddComponent<StarId>();
        lineIdf.id = id;
        lines.Add(lineIdf);

        return line;
    }

    // 패턴 검사
    private bool TryFinish()
    {
        int i = 1;
        foreach (StarId star in lines)
        {
            if (star.id != i)
            {
                Debug.Log("잘못된 순서입니다");
                return false;
            }
            else
                i++;
        }

        return true;
    }

    private IEnumerator WrongAnswer()
    {
        isConnectable = false;

        for (int i = 0; i < 3; i++)
        {
            foreach (StarId star in lines)
            {
                star.gameObject.GetComponent<Image>().color = Color.red;
            }
            yield return new WaitForSeconds(0.5f);

            foreach (StarId star in lines)
            {
                star.gameObject.GetComponent<Image>().color = Color.white;
            }
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1f);
        Destroy(canvas.gameObject);
        Managers.Resource.Instantiate("Puzzle/GameAnim");
    }

    private IEnumerator CorrectAnswer()
    {
        GameObject effect = Managers.Resource.Instantiate("Puzzle/Success_Eff");

        for (int i = 0; i < 3; i++)
        {
            foreach (StarId star in lines)
            {
                star.gameObject.GetComponent<Image>().color = Color.yellow;
            }
            yield return new WaitForSeconds(0.5f);

            foreach (StarId star in lines)
            {
                star.gameObject.GetComponent<Image>().color = Color.white;
            }
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1f);

        GameObject anim_Letter = GameObject.Find("Letter");
        anim_Letter.GetComponent<Animator>().enabled = true; //성공애니메이션 실행

        FindObjectOfType<TransferMap_Letter>().Init();

        Destroy(effect);
        Destroy(canvas.gameObject);
    }
}