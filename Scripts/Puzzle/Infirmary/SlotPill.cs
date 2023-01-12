using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotPill : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IDropHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector2 defaultPos;

    private GameObject acquireObj;
    private Sprite acqSprite;

    private bool pillObj = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        defaultPos = rectTransform.anchoredPosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            if (pillObj)
            {
                if (GetComponent<Image>().sprite != null)
                {
                    CreatePill();
                }
                GetComponent<Image>().sprite = acqSprite;
                Destroy(acquireObj);
            }
            else
            {
                if (acqSprite != null)
                {
                    Sprite temp = acqSprite;

                    acquireObj.GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
                    gameObject.GetComponent<Image>().sprite = temp;
                }
            }
        }
        SetColorEdit();
    }
   
    public void OnTriggerEnter2D(Collider2D other)
    {
        pillObj = false;
        acquireObj = other.gameObject;
        acqSprite = acquireObj.GetComponent<Image>().sprite;

        if (other.CompareTag("Puzzle"))
        {
            pillObj = true;
        }
    }

    public void CreatePill()
    {
        GameObject parent = GameObject.Find("PillCon");
        GameObject obj = Resources.Load("Prefabs/Puzzle/Infirmary/" + GetComponent<Image>().sprite.name) as GameObject;
        GameObject tempPill = Instantiate(obj);

        tempPill.transform.SetParent(parent.transform.GetChild(0));
        tempPill.transform.localPosition = obj.GetComponent<RectTransform>().localPosition;
        tempPill.transform.localScale = Vector3.one;
    }

    public void SetColor(GameObject obj, float alpha)
    {
        Color color = obj.GetComponent<Image>().color;
        color.a = alpha;
        obj.GetComponent<Image>().color = color;
    }

    private void SetColorEdit()
    {
        if (acquireObj.GetComponent<Image>().sprite == null)
            SetColor(acquireObj, 0);
        else SetColor(acquireObj, 1);

        if (GetComponent<Image>().sprite == null)
            SetColor(gameObject, 0);
        else SetColor(gameObject, 1);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        rectTransform.anchoredPosition = defaultPos;
    }
}
