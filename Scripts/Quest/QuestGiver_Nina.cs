using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver_Nina : MonoBehaviour
{
    private void Start()
    {
        CharacterInform nina_inform = gameObject.GetComponent<CharacterInform>();

        if (nina_inform.QuestID == 30)
        {
            if (Managers.Quest.CompletedQuestList.ContainsKey(30))
                nina_inform.QuestID = 40;
            return;
        }
        else
        {
            UI_InvenExtension ui_Inven = UI_InvenExtension.Instance;
            if (ui_Inven == null)
                return;
            foreach (UI_Slot_Item item in ui_Inven.InvenItems)
            {
                if (item != null && item.itemName == "책")
                {
                    if (Managers.Quest.CompletedQuestList.ContainsKey(70))
                        nina_inform.QuestID = 80;
                    return;
                }
            }
        }
    }
}