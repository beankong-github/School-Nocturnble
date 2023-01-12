using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Map : Item
{
    private void Start()
    {
        InitItem();
    }

    public override bool UseItem()
    {
        GameObject uiRoot = GameObject.FindGameObjectWithTag("UI_Root");
        if (uiRoot != null)
            Managers.Resource.Instantiate("UI/Scene/UI_Normal/Map/MiniMap", uiRoot.transform);
        return true;
    }

    protected override void InitItem()
    {
        if (Id == 0)
        {
            Debug.LogError("아이템 ID가 설정되어있지 않습니다.");
            return;
        }

        if (!Managers.Data.ItemDic.TryGetValue(Id, out itemData))
        {
            Debug.Log($"아이템 정보 가져오기 실패 *id: {Id}");
        }
    }
}