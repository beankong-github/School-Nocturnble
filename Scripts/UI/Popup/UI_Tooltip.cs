using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_Tooltip : UI_Popup
{
    public static UI_Tooltip instance;
    private TextMeshProUGUI itemName;
    private TextMeshProUGUI itemInform;

    public TextMeshProUGUI ItemName { get => itemName; set => itemName = value; }
    public TextMeshProUGUI ItemInform { get => itemInform; set => itemInform = value; }

    public enum TMProUGUIs
    {
        Name,
        Information
    }

    private void Start()
    {
        Init();
        InitItemInfo();
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

        Bind<TextMeshProUGUI>(typeof(TMProUGUIs));

        ItemName = Get<TextMeshProUGUI>((int)TMProUGUIs.Name);
        ItemInform = Get<TextMeshProUGUI>((int)TMProUGUIs.Information);
    }

    public void SetItemInfo(string _name, string _info)
    {
        ItemName.text = _name;
        ItemInform.text = _info;

        gameObject.SetActive(true);
    }

    public void InitItemInfo()
    {
        ItemName.text = null;
        ItemInform.text = null;

        gameObject.SetActive(false);
    }

    public void SetTooltipPos(PointerEventData eventData)
    {
        float posX = eventData.position.x > 1765 ? 1765 : eventData.position.x;
        float posY = eventData.position.y;
        Vector2 eventPos = new Vector2(posX, posY);

        gameObject.GetComponent<RectTransform>().anchoredPosition = eventPos;
    }
}