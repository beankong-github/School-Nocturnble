using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_InvenExtension : UI_Popup
{
    private static UI_InvenExtension instance;

    private GameObject background;
    private List<UI_Slot_Item> invenItems = new List<UI_Slot_Item>();

    private UI_QuickSlot quickSlot;
    private UI_Tooltip toolTip;

    public List<UI_Slot_Item> InvenItems { get => invenItems; }
    public static UI_InvenExtension Instance { get => instance; }

    private void Start()
    {
        Init();

        // 퀵 슬롯 찾기
        quickSlot = FindObjectOfType<UI_QuickSlot>();
        if (quickSlot == null)
        {
            quickSlot = UI_QuickSlot.Instance;
        }
        // 툴 팁 찾기
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
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if ((instance != this))
                Destroy(this.gameObject);
        }

        // 인벤 확장창 초기화
        Init_InvenExtension();

        // 부모 설정
        gameObject.transform.SetParent(GameObject.Find("Inventory").transform);

        // 좌표 위치 설정
        gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(749, -301);
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(318, 267);
    }

    // 인벤 확장창 내용물 초기화
    private void Init_InvenExtension()
    {
        if (gameObject != null)
        {
            // 인벤토리 항목들 삭제
            foreach (Transform child in gameObject.transform)
                Managers.Resource.Destroy(child.gameObject);

            // 실제 인벤토리 정보를 참고해서 인벤토리 항목을 다시 채움
            for (int i = 0; i < 12; i++)
            {
                GameObject item = Managers.Resource.Instantiate("UI/Scene/UI_Normal/Inven/Inven_slot_Item");
                item.transform.SetParent(gameObject.transform);
                UI_Slot_Item invenItem = item.GetOrAddComponent<UI_Slot_Item>();

                invenItem.gameObject.AddUIEvent((PointerEventData) => OnSlotClick(invenItem), Define.UIEvent.Click);
                invenItem.gameObject.AddUIEvent((PointerEventData) => OnSlotRightClick(invenItem), Define.UIEvent.RightClick);

                InvenItems.Add(invenItem);
            }
        }
    }

    private void OnSlotClick(UI_Slot_Item slot_Item)
    {
        if (slot_Item.IsEmpty)
        {
            Debug.Log("빈 슬롯 입니다");
            return;
        }
        Debug.Log($"{slot_Item.itemName} 클릭");
    }

    // 우클릭하면 퀵 슬롯에 아이템 저장
    private void OnSlotRightClick(UI_Slot_Item slot_Item)
    {
        if (!slot_Item.IsEmpty)
        {
            quickSlot.AddItem(slot_Item.MyItem, slot_Item);
            return;
        }
    }

    // 아이템 추가
    public bool AddItem(Item _item)
    {
        if (_item.StackSize > 0)
        {
            if (PlaceInStack(_item))
            {
                return true;
            }

            if (PlaceInEmpty(_item))
            {
                return true;
            }
            else return false;
        }

        Debug.LogWarning("아이템에 스택 사이즈를 입력해주세요.");
        return false;
    }

    // 빈 슬롯에 아이템을 추가
    private bool PlaceInEmpty(Item _item)
    {
        foreach (UI_Slot_Item slotItem in InvenItems)
        {
            if (slotItem.IsEmpty)
            {
                if (slotItem.AddItem(_item))
                {
                    quickSlot.AddItem(_item);
                    return true;
                }
                else
                    Debug.LogError($"아이템 추가 실패 : {_item.name}");
            }
        }
        return false;
    }

    // 동일한 아이템을 한 슬롯에 겹쳐서 추가
    private bool PlaceInStack(Item _item)
    {
        foreach (UI_Slot_Item slot_Item in InvenItems)
        {
            if (slot_Item.StackItem(_item))
            {
                foreach (UI_Slot_Item _quickslot in quickSlot.QuickItems)
                {
                    if (_quickslot.MyItem.ItemData.name == slot_Item.MyItem.ItemData.name)
                    {
                        quickSlot.DenoteQuickSlotItemCount(slot_Item, _quickslot);
                        break;
                    }
                }
                return true;
            }
        }

        return false;
    }

    // 확장 인벤이 켜질때마다 배경 on/off
    private void OnEnable()
    {
        background = Managers.Resource.Instantiate("UI/Scene/UI_Normal/Inven/BACK_UI_InvenExtension");
        background.transform.SetParent(GameObject.Find("Inventory").transform);
        background.GetComponent<RectTransform>().anchoredPosition = new Vector2(757.7f, -307.2f);
    }

    private void OnDisable()
    {
        GameObject.Destroy(background);
    }
}