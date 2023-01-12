using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_QuickSlot : UI_Scene
{
    private static UI_QuickSlot instance;

    private UI_InvenExtension invenExtension;
    private UI_Tooltip toolTip;

    private List<UI_Slot_Item> quickItems = new List<UI_Slot_Item>();

    public static UI_QuickSlot Instance { get => instance; }
    public List<UI_Slot_Item> QuickItems { get => quickItems; private set => quickItems = value; }

    private void Start()
    {
        Init();

        toolTip = FindObjectOfType<UI_Tooltip>();
        if (toolTip == null)
        {
            toolTip = UI_Tooltip.instance;
        }
    }

    public override void Init()
    {
        base.Init();

        // 싱글턴
        if (Instance == null)
        {
            instance = this;
        }
        else
        {
            if ((Instance != this))
                Destroy(this.gameObject);
        }

        InitQuickSlot();
    }

    private void InitQuickSlot()
    {
        // 인벤토리 항목들 삭제
        foreach (Transform child in gameObject.transform)
            Managers.Resource.Destroy(child.gameObject);

        // 실제 인벤토리 정보를 참고해서 인벤토리 항목을 다시 채움
        for (int i = 0; i < 4; i++)
        {
            GameObject item = Managers.Resource.Instantiate("UI/Scene/UI_Normal/Inven/Inven_slot_Item");
            item.transform.SetParent(gameObject.transform);
            UI_Slot_Item invenItem = item.GetOrAddComponent<UI_Slot_Item>();
            invenItem.gameObject.AddUIEvent((PointerEventData) => OnSlotClick(invenItem), Define.UIEvent.Click);
            invenItem.gameObject.AddUIEvent((PointerEventData) => OnSlotRightClick(invenItem), Define.UIEvent.RightClick);
            QuickItems.Add(invenItem);
        }
    }

    public void OnSlotClick(UI_Slot_Item _mySlot)
    {
        invenExtension = UI_InvenExtension.Instance;

        if (_mySlot.IsEmpty)
        {
            Debug.Log("빈 슬롯 입니다");
            return;
        }

        if (invenExtension == null)
            return;

        foreach (UI_Slot_Item invenSlot in invenExtension.InvenItems)
        {
            if (!invenSlot.IsEmpty && invenSlot.itemName == _mySlot.itemName)
            {
                Debug.Log($"{_mySlot.itemName} 클릭");
                invenSlot.UseItem();
                DenoteQuickSlotItemCount(invenSlot, _mySlot);

                if (invenSlot.IsEmpty)
                {
                    _mySlot.Init();
                    return;
                }
            }
        }
    }

    private void OnSlotRightClick(UI_Slot_Item slot_Item)
    {
        if (!slot_Item.IsEmpty)
        {
            slot_Item.Init();
            toolTip.InitItemInfo();
        }
    }

    // 아이템 추가
    public bool AddItem(Item _item, UI_Slot_Item _invenSlot = null)
    {
        invenExtension = UI_InvenExtension.Instance;

        foreach (UI_Slot_Item mySlot in QuickItems)
        {
            if (mySlot.itemName == _item.ItemData.name)
            {
                Debug.Log("이미 퀵 슬롯에 등록된 아이템입니다.");
                return false;
            }

            if (mySlot.IsEmpty)
            {
                if (mySlot.AddItem(_item))
                {
                    if (_invenSlot != null)
                        DenoteQuickSlotItemCount(_invenSlot, mySlot);
                    return true;
                }
            }
        }

        return false;
    }

    // 아이템 개수 표시
    public void DenoteQuickSlotItemCount(UI_Slot_Item _invenSlot, UI_Slot_Item _mySlot)
    {
        if (_invenSlot.ItemCount.IsActive())
        {
            _mySlot.ItemCount.gameObject.SetActive(true);
            _mySlot.ItemCount.text = _invenSlot.ItemCount.text;
        }
        else
        {
            _mySlot.ItemCount.gameObject.SetActive(false);
            _mySlot.ItemCount.text = null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnSlotClick(QuickItems[0]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnSlotClick(QuickItems[1]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnSlotClick(QuickItems[2]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnSlotClick(QuickItems[3]);
        }
    }
}