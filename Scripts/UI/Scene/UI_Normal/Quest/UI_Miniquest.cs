using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Miniquest : UI_Scene
{
    private static UI_Miniquest instance;

    [SerializeField]
    private GameObject miniQuestArea;

    [SerializeField]
    private GameObject miniQuestPrefab;

    public static UI_Miniquest Instance { get => instance; private set => instance = value; }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if ((instance != this))
                Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        if (UI_Dialog.MyInstance == null)
        {
            Invoke("OnEnable", 0.3f);
            return;
        }
        UI_Dialog.MyInstance.DialogActiveEvent += HideUI;
        UI_Dialog.MyInstance.DialogDeactiveEvent += ShowUI;

        Managers.Quest.questStartEvent -= AddMiniQuest;
        Managers.Quest.questCompleteEvent -= DeleteMiniQuest;
        Managers.Quest.questStartEvent += AddMiniQuest;
        Managers.Quest.questCompleteEvent += DeleteMiniQuest;
    }

    private void OnDisable()
    {
        UI_Dialog.MyInstance.DialogActiveEvent -= HideUI;
        UI_Dialog.MyInstance.DialogDeactiveEvent -= ShowUI;
    }

    public void AddMiniQuest(int _questId)
    {
        if (miniQuestArea.transform.childCount > 3)
        {
            Debug.Log("MiniQuest 알림창엔 최대 3개의 퀘스트만 띄울 수 있습니다.");
            return;
        }

        for (int i = 0; i < miniQuestArea.transform.childCount; i++)
        {
            GameObject mini = miniQuestArea.transform.GetChild(i).gameObject;
            if (mini.GetComponent<MiniQuest>().QuestId == _questId)
            {
                Debug.Log("이미 MiniQyest창에 등록된 퀘스트입니다.");
                return;
            }
        }

        GameObject go = GameObject.Instantiate(miniQuestPrefab, miniQuestArea.transform);

        string questTitle = Managers.Quest.OngoingQuestList[_questId].name;
        if (questTitle == null)
        {
            Debug.Log("완료된 퀘스트는 추가할 수 없습니다.");
            return;
        }

        string questGoal = "";
        foreach (string goal in Managers.Quest.OngoingQuestList[_questId].goals)
        {
            questGoal += goal;
        }

        go.GetComponent<MiniQuest>().QuestTitle = questTitle;
        go.GetComponent<MiniQuest>().QuestGoal = questGoal;
        go.GetComponent<MiniQuest>().QuestId = _questId;
    }

    public void DeleteMiniQuest(int _questID)
    {
        for (int i = 0; i < miniQuestArea.transform.childCount; i++)
        {
            GameObject mini = miniQuestArea.transform.GetChild(i).gameObject;
            if (mini.GetComponent<MiniQuest>().QuestId == _questID)
            {
                Destroy(mini);
            }
        }
    }

    private void ShowUI()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;

            child.SetActive(true);
        }
    }

    private void HideUI()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;

            child.SetActive(false);
        }
    }
}