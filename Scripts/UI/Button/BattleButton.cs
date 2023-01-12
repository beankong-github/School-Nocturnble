using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public SelecBtnGroup btnGroup;

    private void Start()
    {
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
}