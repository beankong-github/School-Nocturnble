using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UI_Quest_Popup : UI_Popup
{
    private static UI_Quest_Popup instance;

    private GameObject quest_Win;
    private TextMeshProUGUI title;
    private TextMeshProUGUI goals;
    private TextMeshProUGUI contents;
    private Image completeFilter;
    private Button exit_btn;

    [SerializeField]
    private GameObject questParent;

    private List<GameObject> questList = new List<GameObject>();

    [SerializeField]
    private GameObject questPrefab;

    private enum Buttons
    {
        ExitButton
    }

    private enum Images
    {
        QuestComplete
    }

    private enum TMPros
    {
        QuestTitle,
        QuestGoals,
        QuestContents,
    }

    public static UI_Quest_Popup Instance { get => instance; }

    public void UpdateQuestList(int qustId = 0)
    {
        foreach (GameObject go in questList)
        {
            GameObject.Destroy(go);
        }

        foreach (KeyValuePair<int, BaseQuest> quest in Managers.Quest.OngoingQuestList)
        {
            UI_Miniquest miniquest = UI_Miniquest.Instance;

            GameObject go = Instantiate(questPrefab, questParent.transform);
            AddUIEvent(go, (PointerEventData data) => ShowQuestData(quest.Key), Define.UIEvent.Click);
            AddUIEvent(go, (PointerEventData data) => miniquest.AddMiniQuest(quest.Key), Define.UIEvent.DoubleClick);

            go.GetComponent<TextMeshProUGUI>().text = quest.Value.name;
            questList.Add(go);
        }

        foreach (KeyValuePair<int, BaseQuest> quest in Managers.Quest.CompletedQuestList)
        {
            GameObject go = Instantiate(questPrefab, questParent.transform);
            AddUIEvent(go, (PointerEventData data) => ShowQuestData(quest.Key), Define.UIEvent.Click);

            go.GetComponent<TextMeshProUGUI>().text = quest.Value.name;
            questList.Add(go);

            go.transform.SetAsLastSibling();
            go.GetComponent<Button>().interactable = false;
            go.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
        }

        InitQuestInfo();
    }

    private void ShowQuestData(int _questID)
    {
        // 퀘스트 정보 가져오기
        BaseQuest quest = Managers.Data.QuestDic[_questID];
        if (quest == null)
        {
            Debug.LogError($"{_questID}를 아이디로 가진 퀘스트가 존재하지 않습니다.");
            return;
        }

        // 퀘스트 UI 초기화
        InitQuestInfo();

        // 퀘스트 정보 표시
        title.text = quest.name;
        foreach (string goal in quest.goals)
        {
            goals.text += goal;
        }
        contents.text = quest.contents;
        completeFilter.gameObject.SetActive(Managers.Quest.CompletedQuestList.ContainsValue(quest));
    }

    private void InitQuestInfo()
    {
        // 퀘스트 UI 초기화
        title.text = null;
        goals.text = null;
        contents.text = null;
        completeFilter.gameObject.SetActive(false);
    }

    public override void Init()
    {
        base.Init();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if ((instance != this))
                Destroy(this.gameObject);
        }

        Bind<TextMeshProUGUI>(typeof(TMPros));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));

        title = Get<TextMeshProUGUI>((int)TMPros.QuestTitle);
        goals = Get<TextMeshProUGUI>((int)TMPros.QuestGoals);
        contents = Get<TextMeshProUGUI>((int)TMPros.QuestContents);
        completeFilter = GetImage((int)Images.QuestComplete);
        exit_btn = Get<Button>((int)Buttons.ExitButton);

        // 드래그하는대로 창 이동
        quest_Win = GameObject.Find("Quest_Win").gameObject;
        AddUIEvent(quest_Win, (PointerEventData data) => { quest_Win.GetComponent<RectTransform>().anchoredPosition += data.delta / this.GetComponent<Canvas>().scaleFactor; }, Define.UIEvent.Drag);
        AddUIEvent(exit_btn.gameObject, (PointerEventData data) => ClosePopupUI(), Define.UIEvent.Click);

        InitQuestInfo();
    }

    private void Start()
    {
        Init();
        UpdateQuestList();
    }

    private void OnEnable()
    {
        Managers.Quest.questStartEvent += UpdateQuestList;
        Managers.Quest.questCompleteEvent += UpdateQuestList;
    }

    private void OnDisable()
    {
        Managers.Quest.questStartEvent -= UpdateQuestList;
        Managers.Quest.questCompleteEvent -= UpdateQuestList;
    }
}