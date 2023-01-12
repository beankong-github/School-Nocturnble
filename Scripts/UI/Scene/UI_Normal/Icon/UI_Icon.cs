using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Icon : UI_Scene
{
    private static UI_Icon instance;

    private bool isActive;

    public static UI_Icon MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UI_Icon>();
            }

            return instance;
        }
    }

    private enum Buttons
    {
        Quest_Btn,
        Map_Btn,
        Skill_Btn,
        Inven_Btn,
        Menu_Btn
    }

    private void OnEnable()
    {
        if (UI_Dialog.MyInstance == null)
        {
            Invoke("OnEnable", 0.3f);
            return;
        }
        UI_Dialog.MyInstance.DialogActiveEvent += HideIcons;
        UI_Dialog.MyInstance.DialogDeactiveEvent += ShowIcons;
    }

    private void OnDisable()
    {
        UI_Dialog.MyInstance.DialogActiveEvent -= HideIcons;
        UI_Dialog.MyInstance.DialogDeactiveEvent -= ShowIcons;
    }

    private void ShowIcons()
    {
        isActive = true;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;

            child.SetActive(true);
        }
    }

    private void HideIcons()
    {
        isActive = false;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;

            child.SetActive(false);
        }
    }

    private void Start()
    {
        isActive = true;

        Bind<Button>(typeof(Buttons));

        // 버튼에 클릭 이벤트에 따른 행동 추가
        GetButton((int)Buttons.Quest_Btn).gameObject.AddUIEvent(OnQuestClicked, Define.UIEvent.Click);
        GetButton((int)Buttons.Skill_Btn).gameObject.AddUIEvent(OnSkillClicked, Define.UIEvent.Click);
        GetButton((int)Buttons.Inven_Btn).gameObject.AddUIEvent(OnInvenClicked, Define.UIEvent.Click);
        GetButton((int)Buttons.Map_Btn).gameObject.AddUIEvent(OnMapClicked, Define.UIEvent.Click);
    }

    private void Update()
    {
        if (isActive)
            OnKeyboard();
    }

    // 퀘스트(편지) 아이콘을 클릭하면 퀘스트 창 On/Off
    public void OnQuestClicked(PointerEventData data = null)
    {
        // 퀘스트 창이 안떠있으면
        if (UI_Quest_Popup.Instance == null)
        {
            Managers.UI.ShowPopupUI<UI_Quest_Popup>("UI_Quest_Popup");
        }
        // 퀘스트 창이 떠있으면
        else
        {
            UI_Popup quest_Popup = FindObjectOfType<UI_Quest_Popup>();
            Managers.UI.ClosePopupUI(quest_Popup);
        }
    }

    // 스킬북 아이콘을 클릭하면 스킬 창 On/Off
    public void OnSkillClicked(PointerEventData data = null)
    {
        // 스킬창이 안떠있으면
        if (UI_Skill_Popup.Instance == null)
        {
            Managers.UI.ShowPopupUI<UI_Skill_Popup>("UI_Skill_Popup");
        }
        // 스킬창이 떠있으면
        else
        {
            UI_Popup skill_Popup = FindObjectOfType<UI_Skill_Popup>();
            Managers.UI.ClosePopupUI(skill_Popup);
        }
    }

    public void OnInvenClicked(PointerEventData data = null)
    {
        if (UI_InvenExtension.Instance == null)
        {
            Managers.UI.ShowPopupUI<UI_InvenExtension>("UI_InvenExtension");
        }
        else
        {
            if (UI_InvenExtension.Instance.gameObject.activeSelf)
            {
                UI_InvenExtension.Instance.gameObject.SetActive(false);
            }
            else
            {
                UI_InvenExtension.Instance.gameObject.SetActive(true);
            }
        }
    }

    public void OnMapClicked(PointerEventData data = null)
    {
        if (!PlayerData.isClear_sildepuzzle)
            return;

        // 미니맵이 안떠있으면
        if (UI_MiniMap.Instance == null)
        {
            Managers.UI.ShowSceneUI<UI_MiniMap>("UI_Normal/Map/MiniMap");
        }
        // 미니맵 떠있으면
        else
        {
            UI_Scene minimap = FindObjectOfType<UI_MiniMap>();
            Destroy(minimap.gameObject);
        }
    }

    private void OnKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Managers.UI.ClosePopupUI();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnQuestClicked();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            OnSkillClicked();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            OnInvenClicked();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            OnMapClicked();
        }
    }
}