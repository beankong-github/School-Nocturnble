using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveItem : MonoBehaviour
{
    public int questId;
    public Item item;

    // Start is called before the first frame update
    private void Start()
    {
        Managers.Quest.questStartEvent -= GiveItemOnQuestStart;
        Managers.Quest.questStartEvent += GiveItemOnQuestStart;
    }

    private void GiveItemOnQuestStart(int _questId)
    {
        if (_questId == questId)
        {
            Managers.Data.ItemDic.TryGetValue(item.Id, out item.itemData);
            UI_InvenExtension.Instance.AddItem(item);
        }
    }
}