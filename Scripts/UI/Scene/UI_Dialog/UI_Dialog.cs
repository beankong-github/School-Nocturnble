using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_Dialog : UI_Scene
{
    private static UI_Dialog instance;

    public delegate void DialogDelegate();

    public DialogDelegate DialogActiveEvent;
    public DialogDelegate DialogDeactiveEvent;

    private Button nextBtn;
    private TextMeshProUGUI speakerTMP;
    private TextMeshProUGUI scriptTMP;
    private Dictionary<string, Image> PortraitDic = new Dictionary<string, Image>();

    private BaseTalk baseTalk;

    private bool isAction;
    private int questTalkIndex;
    private int talkIndex = 0;
    private int curquestId = 0;
    public Color[] portraitTypes = { Color.clear, Color.white, Color.gray };

    public static UI_Dialog MyInstance { get => instance; }
    public bool IsAction { get => isAction; set => isAction = value; }
    public int CurquestId { get => curquestId; set => curquestId = value; }

    private enum TMPro
    {
        Script,
        Name
    }

    private enum Buttons
    {
        Next
    }

    private enum PortraitImgs
    {
        Player,
        Nina,
        Cory,
        Chalotte,
        Dien,
        Maya,
        SubPlayer,
        Nova
    }

    private enum PortraitType
    {
        Clear,
        White,
        Gray
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }

        Bind<TextMeshProUGUI>(typeof(TMPro));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(PortraitImgs));

        speakerTMP = Get<TextMeshProUGUI>((int)TMPro.Name);
        scriptTMP = Get<TextMeshProUGUI>((int)TMPro.Script);
        nextBtn = GetButton((int)Buttons.Next);

        foreach (PortraitImgs character in Enum.GetValues(typeof(PortraitImgs)))
        {
            PortraitDic.Add(character.ToString(), GetImage((int)character));
        }

        nextBtn.gameObject.AddUIEvent(ShowNextSentence, Define.UIEvent.Click);
        nextBtn.gameObject.SetActive(false);

        InitDialog();
    }

    private void Update()
    {
        if (gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            InitDialog();
        }
        if (gameObject.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextSentence();
        }
    }

    public void InitDialog()
    {
        isAction = false;
        baseTalk = null;
        questTalkIndex = 0;
        talkIndex = 0;
        CurquestId = 0;

        speakerTMP.text = null;
        scriptTMP.text = null;

        foreach (KeyValuePair<string, Image> character in PortraitDic)
        {
            character.Value.color = portraitTypes[(int)PortraitType.Clear];

            if (character.Key != "Player")
            {
                Vector3 portraitePos = character.Value.gameObject.GetComponent<RectTransform>().localPosition;
                character.Value.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-675f, portraitePos.y, portraitePos.z);
            }
        }

        MyInstance.gameObject.SetActive(isAction);
        nextBtn.gameObject.SetActive(isAction);

        if (DialogDeactiveEvent != null)
            DialogDeactiveEvent();
    }

    public void Action(int _id, int _qusetId = 0)
    {
        CurquestId = _qusetId;

        Talk(_id, CurquestId);

        MyInstance.gameObject.SetActive(isAction);

        if (isAction)
        {
            if (DialogActiveEvent != null)
                DialogActiveEvent();
        }
        else
        {
            if (DialogDeactiveEvent != null)
                DialogDeactiveEvent();
        }
    }

    private void Talk(int _id, int _questId = 0)  // _id는 NPC나 Object의 ID
    {
        // Set Portrait
        if (!Managers.Data.TalkDic[_id].isNPC)
        {
            // Object일 경우 초상화 설정 & questTalkIndex 수정
            speakerTMP.text = Managers.Data.CharDic[1000].name;
            PortraitDic["Player"].color = portraitTypes[(int)PortraitType.White];

            questTalkIndex = 0;
        }
        else
        {
            // NPC일 경우 초상화 설정 & qustTalkIndex 가져오기.
            BaseCharacter tmpChar = Managers.Data.CharDic[_id];
            speakerTMP.text = tmpChar.name;
            PortraitDic[tmpChar.portraitName].color = portraitTypes[(int)PortraitType.White];

            questTalkIndex = Managers.Quest.GetQuestTalkIndex(_id, _questId);
        }

        // Get Talk Data
        Managers.Data.TalkDic.TryGetValue(questTalkIndex + _id, out baseTalk);

        // Set Talk Data
        string talk;

        // 랜덤 대화 출력
        if (baseTalk.isRandom)
        {
            // End Talk
            if (isAction)
            {
                InitDialog();
                return;
            }

            talk = Managers.Dialogue.GetTalk(questTalkIndex + _id, -1);

            scriptTMP.text = talk;
        }

        // 순차적 대화 출력
        else
        {
            talk = Managers.Dialogue.GetTalk(questTalkIndex + _id, talkIndex);

            // End Talk
            if (talk == null)
            {
                if (baseTalk.isNPC)
                    Managers.Quest.CheckQuest(_id, _questId);
                InitDialog();
                return;
            }

            string[] talks = talk.Split('#');

            // 대화에 따른 초상화 상태, 발화자 이름 반영
            if (talks.Length > 1)
            {
                SetPortraite(int.Parse(talks[1]) * 1000);
            }

            string script = null;

            // 대화 적용
            for (int i = 0; i < talks[0].Length; i++)
            {
                if (talks[0][i] == '@')
                {
                    script += Managers.Data.CharDic[1000].name;
                }
                else
                {
                    script += talks[0][i];
                }
            }
            scriptTMP.text = script;

            talkIndex++;
        }

        isAction = true;
        nextBtn.gameObject.SetActive(true);
    }

    private void SetPortraite(int _id)
    {
        // 현재 활성화 되어있는 초상화는 회색으로
        foreach (KeyValuePair<string, Image> character in PortraitDic)
        {
            if (character.Value.color == portraitTypes[(int)PortraitType.White])
                character.Value.color = portraitTypes[(int)PortraitType.Gray];

            if (character.Key != "Player" && _id != 1000)
                if (character.Value.color == portraitTypes[(int)PortraitType.Gray])
                {
                    Vector3 portraitePos = character.Value.gameObject.GetComponent<RectTransform>().localPosition;
                    character.Value.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-280f, portraitePos.y, portraitePos.z);
                }
        }

        // 활성화할 캐릭터의 초상화는 흰색으로
        BaseCharacter tmpChar = Managers.Data.CharDic[_id];
        speakerTMP.text = tmpChar.name;
        PortraitDic[tmpChar.portraitName].color = portraitTypes[(int)PortraitType.White];
        if (tmpChar.portraitName != "Player")
        {
            Vector3 portraitePos = PortraitDic[tmpChar.portraitName].GetComponent<RectTransform>().localPosition;
            PortraitDic[tmpChar.portraitName].GetComponent<RectTransform>().localPosition = new Vector3(-675f, portraitePos.y, portraitePos.z);
        }
    }

    private void ShowNextSentence(PointerEventData data = null)
    {
        if (baseTalk != null)
        {
            Action(baseTalk.id - questTalkIndex, CurquestId);
        }
    }
}