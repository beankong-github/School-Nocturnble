using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyDragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int id;
    public bool dropOnSlot;
    private Vector3 defaultPos;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        defaultPos = GetComponent<RectTransform>().localPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dropOnSlot = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        gameObject.transform.SetAsLastSibling(); //하이라키에서의 위치 변경
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        StartCoroutine("Return");
    }

    public IEnumerator Return()
    {
        yield return new WaitForEndOfFrame();

        if (dropOnSlot == false)
        {
            rectTransform.anchoredPosition = defaultPos;
        }
    }
}
