using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IDragHandler, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action<PointerEventData> OnDragHandler = null;
    public Action<PointerEventData> OnPointerClickHandler = null;
    public Action<PointerEventData> OnPointerRightClickHandler = null;
    public Action<PointerEventData> OnPointerUpHandler = null;
    public Action<PointerEventData> OnPointerDownHandler = null;
    public Action<PointerEventData> OnPointerEnterHandler = null;
    public Action<PointerEventData> OnPointerExitHandler = null;
    public Action<PointerEventData> OnPointerDoubleClickHandler = null;

    private int clickCount = 0;
    private WaitForSeconds doubleClickTreashHold = new WaitForSeconds(0.4f);

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Draged");
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            OnPointerRightClick(eventData);

        clickCount++;
        if (clickCount == 2)
        {
            if (OnPointerClickHandler != null)
                OnPointerDoubleClick(eventData);
            clickCount = 0;
            return;
        }
        else
        {
            StartCoroutine(TickDown());
        }

        Debug.Log("Clicked");
        if (OnPointerClickHandler != null)
            OnPointerClickHandler.Invoke(eventData);
    }

    private IEnumerator TickDown()
    {
        yield return doubleClickTreashHold;
        if (clickCount > 0)
        {
            clickCount--;
        }
    }

    public void OnPointerRightClick(PointerEventData eventData)
    {
        Debug.Log("Right Clicked");
        if (OnPointerRightClickHandler != null)
            OnPointerRightClickHandler.Invoke(eventData);
    }

    public void OnPointerDoubleClick(PointerEventData eventData)
    {
        Debug.Log("Double Clicked");
        if (OnPointerDoubleClickHandler != null)
            OnPointerDoubleClickHandler.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("PointerUp");
        if (OnPointerUpHandler != null)
            OnPointerUpHandler.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("PointerDown");
        if (OnPointerDownHandler != null)
            OnPointerDownHandler.Invoke(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("PointerEnter");
        if (OnPointerEnterHandler != null)
            OnPointerEnterHandler.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("PointerExit");
        if (OnPointerExitHandler != null)
            OnPointerExitHandler.Invoke(eventData);
    }
}