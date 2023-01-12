using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public SkillBtnGroup btnGroup;
    public int skillId;
    public GameObject temp;

    private void Start()
    {
        OffSkill();
        btnGroup.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        btnGroup.OnBtnSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        btnGroup.OnBtnEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        btnGroup.OnBtnExit(this);
    }

    public void OnSkill() // 스킬 확인용..
    {
        temp.SetActive(true);
    }

    public void OffSkill() // 스킬 확인용..
    {
        temp.SetActive(false);
    }
}