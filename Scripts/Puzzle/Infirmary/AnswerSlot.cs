using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswerSlot : MonoBehaviour
{
    private GameObject[] slotArray;

    private int answerCount = 0;

    public GameObject eff_Success1;
    public GameObject eff_Success2;
    private GameObject keyInput;
    private GameObject prefabPill;

    public TMP_Text text;

    private bool key_Input = true;
    private bool check = true;
    private bool clone = false;

    void Start()
    {
        eff_Success1.SetActive(false);
        eff_Success2.SetActive(false);

        keyInput = transform.Find("KeyInput").gameObject;
        
        GameObject Slot = GameObject.Find("Slots");
        int slotCount = Slot.transform.childCount;

        slotArray = new GameObject[slotCount];

        for (int i = 0; i < slotCount; i++)
            slotArray[i] = Slot.transform.GetChild(i).gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            Answer();
    }

    public void Answer()
    {
        if (key_Input)
        {
            key_Input = false;
            for (int i = 0; i < slotArray.Length; i++)
            {
                if (slotArray[i].name == "Slot0")
                    AnswerCheck(i, 0);
                else if (slotArray[i].name == "Slot1")
                    AnswerCheck(i, 1);
                else if (slotArray[i].name == "Slot2")
                    AnswerCheck(i, 2);
                else if (slotArray[i].name == "Slot3")
                    AnswerCheck(i, 3);
                else Debug.Log(slotArray[i] + "오류");

                if (check == false)
                {
                    StartCoroutine(ResultCoroutine());
                    return;
                }
            }
            StartCoroutine(ResultCoroutine());
        }
    }

    private void AnswerCheck(int i, int num)
    {
        if (slotArray[i].GetComponent<Image>().sprite != null)
        {
            if (slotArray[i].GetComponent<Image>().sprite.name == "Pill" + num)
            {
                answerCount++;
            }
            else check = false;
        }
        else check = false;
    }

    public void ResetPill()
    {
        answerCount = 0;
        check = true;

        if (clone == false)
        {
            Destroy(GameObject.Find("Pills"));
        } else Destroy(prefabPill);

        GameObject parent = GameObject.Find("PillCon");
        GameObject obj = Resources.Load("Prefabs/Puzzle/Infirmary/Pills") as GameObject;

        prefabPill = Instantiate(obj);
        prefabPill.transform.SetParent(parent.transform);
        clone = true;
    }

    private IEnumerator ResultCoroutine()
    {
        keyInput.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        if (answerCount == 12)
        {
            text.text = "정답";
            eff_Success1.SetActive(true);
            eff_Success2.SetActive(true);

            // 보건실로 씬 전환
        }
        else
        {
            text.text = "다시 생각해보자";

            for (int i = 0; i < slotArray.Length; i++)
            {
                Color color = slotArray[i].GetComponent<Image>().color;
                color.a = 0;
                slotArray[i].GetComponent<Image>().color = color;
                slotArray[i].GetComponent<Image>().sprite = null;
            }

            ResetPill();
            yield return new WaitForSeconds(1.3f);
            keyInput.SetActive(false);
            key_Input = true;
            text.text = "";
        }
    }
}
