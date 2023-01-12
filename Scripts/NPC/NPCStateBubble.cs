using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateBubble : MonoBehaviour
{
    /* NPC */
    private GameObject NPC;

    /* Target Quest */
    public int targetQuestID;

    /* Bubbles */
    private GameObject icon_startQuest;
    private GameObject icon_duringQuest;

    private void OnEnable()
    {
        Managers.Quest.questStartEvent -= OnQuestStart;
        Managers.Quest.questStartEvent += OnQuestStart;
        Managers.Quest.questCompleteEvent -= OnQuestComplete;
        Managers.Quest.questCompleteEvent += OnQuestComplete;
    }

    private void OnDestroy()
    {
        Managers.Quest.questStartEvent -= OnQuestStart;
        Managers.Quest.questCompleteEvent -= OnQuestComplete;
    }

    private void Start()
    {
        NPC = transform.parent.gameObject;
        targetQuestID = NPC.GetComponent<CharacterInform>().QuestID;

        icon_startQuest = gameObject.transform.Find("Icon_startQuest").gameObject;
        icon_duringQuest = gameObject.transform.Find("Icon_duringQuest").gameObject;
    }

    private void OnQuestStart(int _questId)
    {
        if (targetQuestID == _questId)
        {
            icon_startQuest.SetActive(false);
            icon_duringQuest.SetActive(true);
        }
    }

    private void OnQuestComplete(int _questId)
    {
        if (targetQuestID == _questId)
        {
            Destroy(gameObject);
        }
    }
}