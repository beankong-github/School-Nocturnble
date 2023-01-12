using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Book : Item
{
    public override bool UseItem()
    {
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
        return;
    }

    // Start is called before the first frame update
    private void Start()
    {
        Id = 105;

        InitItem();
    }
}