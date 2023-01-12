using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Slot_Item : UI_Base
{
    // == 슬롯 정보 ==============================
    public Sprite initImg;

    // == 아이템의 정보 ===========================

    public string itemName;
    public Image itemImage;

    [SerializeField]
    private Text itemCount;

    private Stack<Item> myItems = new Stack<Item>();

    // 스택에 값이 0이면 true 아니면 false
    public bool IsEmpty
    {
        get
        {
            return myItems.Count == 0;
        }
    }

    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return myItems.Peek();
            }

            return null;
        }
    }

    public Stack<Item> MyItems
    {
        get => myItems;
    }

    public Text ItemCount { get => itemCount; private set => itemCount = value; }

    // UI Inven의 아이템 창의 자식들 ==============
    private enum Images
    {
        Item_sprite
    }

    private enum Texts
    {
        Item_count
    }

    private void Start()
    {
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        itemImage = GetImage((int)Images.Item_sprite);
        ItemCount = GetText((int)Texts.Item_count);

        this.gameObject.AddUIEvent((PointerEventData) => OnPointerEnter(PointerEventData), Define.UIEvent.Enter);
        this.gameObject.AddUIEvent((PointerEventData) => OnPointerExit(), Define.UIEvent.Exit);

        Init();
    }

    private void OnPointerEnter(PointerEventData eventData)
    {
        UI_Tooltip toolTip = FindObjectOfType<UI_Tooltip>();
        if (toolTip == null)
        {
            toolTip = UI_Tooltip.instance;
        }

        if (toolTip != null & !IsEmpty)
        {
            toolTip.SetItemInfo(this.MyItem.ItemData.name, this.MyItem.ItemData.information);
            toolTip.SetTooltipPos(eventData);
        }

        return;
    }

    private void OnPointerExit()
    {
        UI_Tooltip toolTip = FindObjectOfType<UI_Tooltip>();
        if (toolTip == null)
        {
            toolTip = UI_Tooltip.instance;
        }

        toolTip.InitItemInfo();
        return;
    }

    public override void Init()
    {
        myItems = new Stack<Item>();

        SetInfo(null, initImg);

        DenoteItemCount();
    }

    public void SetInfo(string name, Sprite img)
    {
        itemName = name;
        itemImage.sprite = img;
    }

    public bool AddItem(Item _item)
    {
        // 아이템 정보 설정
        SetInfo(_item.ItemData.name, _item.Icon);

        // 아이템 스택에 추가
        for (int i = 0; i < _item.StackSize; i++)
        {
            MyItems.Push(_item);
            DenoteItemCount();
        }

        if (MyItems.Count > 1)
        {
            ItemCount.gameObject.SetActive(true);
            ItemCount.text = $"{MyItems.Count}";
        }
        return true;
    }

    // 아이템 사용
    public void UseItem()
    {
        Item curItem = MyItem;
        if (curItem.UseItem())
        {
            MyItems.Pop();
            DenoteItemCount();
        }
        else
        {
            return;
        }

        if (this.IsEmpty)
        {
            this.Init();
        }
    }

    // 아이템 스택으로 저장
    public bool StackItem(Item _item)
    {
        if (!IsEmpty && MyItem.ItemData.name == _item.ItemData.name)
        {
            MyItems.Push(_item);
            DenoteItemCount();
            return true;
        }
        else
        {
            return false;
        }
    }

    // 아이템 stack의 개수를 text로 표시, stack 사용 후 다음 줄에 꼭 붙여주기!
    private void DenoteItemCount()
    {
        ItemCount.text = $"{MyItems.Count}";

        if (MyItems.Count > 1)
        {
            ItemCount.gameObject.SetActive(true);
        }
        else
        {
            ItemCount.gameObject.SetActive(false);
        }
    }
}