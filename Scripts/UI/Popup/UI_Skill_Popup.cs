using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Skill_Popup : UI_Popup
{
    private static UI_Skill_Popup instance;
    private GameObject skill_Win;

    public Button eixtBtn;

    public static UI_Skill_Popup Instance { get => instance; }

    private void Start()
    {
        Init();

        Transform skill_Win = transform.GetChild(0);

        // 모든 스킬 초기화
        for (int i = 0; i < skill_Win.childCount; i++)
        {
            BaseSkill skill;
            Managers.Data.SkillDic.TryGetValue(i, out skill);
            skill_Win.GetChild(i).gameObject.SetActive(skill.isAcquired);
        }
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

        // 드래그하는대로 창 이동
        skill_Win = this.gameObject.transform.Find("Skill_Win").gameObject;
        AddUIEvent(skill_Win, (PointerEventData data) => { skill_Win.GetComponent<RectTransform>().anchoredPosition += data.delta / this.GetComponent<Canvas>().scaleFactor; }, Define.UIEvent.Drag);
        AddUIEvent(eixtBtn.gameObject, (PointerEventData data) => ClosePopupUI(), Define.UIEvent.Click);
    }
}