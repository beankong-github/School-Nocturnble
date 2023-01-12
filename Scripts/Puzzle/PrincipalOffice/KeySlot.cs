using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class KeySlot : MonoBehaviour, IDropHandler
{
    private Dictionary<int, KeyDragDrop> keys;

    private int countNum;
    private int correctNum;
    private bool correctAnswer;

    private GameObject inputKey;
    public GameObject boxColor;
    public GameObject puzzleEff;
    public GameObject successEff;

    public TMP_Text text;
    public TMP_Text countText;
 
   private void Start()
    {
        keys = new Dictionary<int, KeyDragDrop>();
        inputKey = transform.Find("InputKey").gameObject;
        ResetPuzzle();

        GameObject starKey = GameObject.Find("StarKey");
        int keyCount = starKey.transform.childCount;

        for (int i = 0; i < keyCount; i++)
        {
            var key = starKey.transform.GetChild(i);
            var identifier = key.GetComponent<KeyDragDrop>();

            identifier.id = i;

            keys.Add(i, identifier);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<KeyDragDrop>().dropOnSlot = true;
        
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            TextNumEdit();

            if (correctAnswer)
            {
                correctNum++;
            }

            if (countNum == 3) 
            {
                inputKey.SetActive(true);
                StartCoroutine(ResultCoroutine());
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Puzzle"))
        {
            correctAnswer = true;
        } else correctAnswer = false;
    }

    private void TextNumEdit() 
    {
        countNum++;
        countText.text = countNum + "/3";
    }

    private void ResetPuzzle()
    {
        correctNum = 0;
        countNum = 0;
        countText.text = countNum + "/3";
        text.text = "";
        inputKey.SetActive(false);
    }

    private IEnumerator ResultCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        if (correctNum == 3)
        {
            text.text = "정답";

            yield return new WaitForSeconds(0.8f);
            puzzleEff.SetActive(true);
            successEff.SetActive(true);

            // 교장실로 씬 전환
        }
        else
        {
            text.text = "다시 생각해보자";

            SpriteRenderer render = boxColor.GetComponent<SpriteRenderer>();
            Color defaultColor = render.color;
            render.color = new Color(1, 100 / 255, 100 / 255);

            yield return new WaitForSeconds(1.5f);

            boxColor.GetComponent<SpriteRenderer>().color = defaultColor;

            foreach (var key in keys)
            {
                key.Value.dropOnSlot = false;
                key.Value.StartCoroutine(key.Value.Return());
            }

            ResetPuzzle();
        }
    }
}
