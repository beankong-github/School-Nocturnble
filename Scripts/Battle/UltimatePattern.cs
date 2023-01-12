using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimatePattern : MonoBehaviour
{
    private Dictionary<int, StarIdentifier> stars;

    private List<StarIdentifier> lines;

    public GameObject linePrefab;
    public Canvas canvas;

    private GameObject eye;

    private GameObject lineOnEdit;
    private RectTransform lineOnEditRectTrans;
    private StarIdentifier starOnEdit;

    private bool isConnectable;

    private void Start()
    {
        CursorController.MyInstance.FixCursorMode(CursorController.CursorMode.Eyetracker);

        stars = new Dictionary<int, StarIdentifier>();
        lines = new List<StarIdentifier>();

        for (int i = 0; i < transform.childCount; i++)
        {
            StarIdentifier child = transform.GetChild(i).gameObject.GetComponent<StarIdentifier>();
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

    public void OnEyeEnter(StarIdentifier star)
    {
        if (lineOnEditRectTrans != null && starOnEdit != null)
        {
            lineOnEditRectTrans.sizeDelta = new Vector2(lineOnEditRectTrans.sizeDelta.x, Vector3.Distance(starOnEdit.transform.localPosition, star.transform.localPosition));
            lineOnEditRectTrans.rotation = Quaternion.FromToRotation(Vector3.up, (star.transform.localPosition - starOnEdit.transform.localPosition).normalized);
        }
    }

    public void StaySuccess(StarIdentifier star)
    {
        isConnectable = true;
        OnEyeEnter(star);

        TrySetLineEdit(star);
    }

    private void TrySetLineEdit(StarIdentifier star)
    {
        foreach (StarIdentifier line in lines)
        {
            if (line.id == star.id)
                return;
        }

        lineOnEdit = CreateLine(star.transform.localPosition, star.id);
        lineOnEditRectTrans = lineOnEdit.GetComponent<RectTransform>();
        starOnEdit = star;

        // 마지막 라인 일 경우
        if (lines.Count >= 5 && lines[lines.Count - 1].id == star.id)
        {
            lineOnEditRectTrans.sizeDelta = new Vector2(lineOnEditRectTrans.sizeDelta.x, Vector3.Distance(starOnEdit.transform.localPosition, lines[0].transform.localPosition));
            lineOnEditRectTrans.rotation = Quaternion.FromToRotation(Vector3.up, (lines[0].transform.localPosition - starOnEdit.transform.localPosition).normalized);
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
        StarIdentifier lineIdf = line.GetOrAddComponent<StarIdentifier>();
        lineIdf.id = id;
        lines.Add(lineIdf);

        return line;
    }

    // 패턴 검사
    private bool TryFinish()
    {
        int i = 1;
        foreach (StarIdentifier star in lines)
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
        for (int i = 0; i < 3; i++)
        {
            foreach (StarIdentifier star in lines)
            {
                star.gameObject.GetComponent<Image>().color = Color.red;
            }
            yield return new WaitForSeconds(0.5f);

            foreach (StarIdentifier star in lines)
            {
                star.gameObject.GetComponent<Image>().color = Color.white;
            }
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1f);

        Destroy(canvas.gameObject);
        Managers.Resource.Instantiate("Effect/Skill/Ult_Canvas");
    }

    private IEnumerator CorrectAnswer()
    {
        GameObject effect = Managers.Resource.Instantiate("Puzzle/Success_Eff");

        for (int i = 0; i < 3; i++)
        {
            foreach (StarIdentifier star in lines)
            {
                star.gameObject.GetComponent<Image>().color = Color.yellow;
            }
            yield return new WaitForSeconds(0.5f);

            foreach (StarIdentifier star in lines)
            {
                star.gameObject.GetComponent<Image>().color = Color.white;
            }
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1f);

        Destroy(effect);
        Destroy(canvas.gameObject);
        BattleSystem.MyBattleSystem.EndBattle(BattleState.WON);
    }
}