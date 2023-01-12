using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
    public delegate void QuestEventDelegate(int questId);

    public QuestEventDelegate questStartEvent;
    public QuestEventDelegate questCompleteEvent;

    public Dictionary<int, BaseQuest> OngoingQuestList = new Dictionary<int, BaseQuest>();
    public Dictionary<int, BaseQuest> CompletedQuestList = new Dictionary<int, BaseQuest>();

    private GameObject qusetGiver;

    // 퀘스트 시작
    private void QuestStart(int _questId)
    {
        if (OngoingQuestList.ContainsKey(_questId) || CompletedQuestList.ContainsKey(_questId))
        {
            Debug.Log("이미 진행중인 퀘스트이거나 완료된 퀘스트 입니다.");
            return;
        }
        else
        {
            OngoingQuestList.Add(_questId, Managers.Data.QuestDic[_questId]);
        }
    }

    // 퀘스트 진행 상황을 나타내는 ID를 받아온다.
    public int GetQuestTalkIndex(int _npcId, int _questId)
    {
        // 퀘스트가 진행중일 경우
        if (OngoingQuestList.ContainsKey(_questId))
        {
            CheckCompletable(_questId);
            return _questId + OngoingQuestList[_questId].questActionIndex;
        }

        // 퀘스트가 완료되었을 경우
        else if (CompletedQuestList.ContainsKey(_questId))
            return 0;

        // 퀘스트를 시작하는 경우
        else
            return _questId;
    }

    // 퀘스트 다음 단계로 전진
    public string CheckQuest(int _npcId, int _questId)
    {
        BaseQuest curQuest = Managers.Data.QuestDic[_questId];

        // Start Quest
        if (curQuest.questActionIndex == 0 && !OngoingQuestList.ContainsValue(curQuest))
        {
            QuestStart(_questId);
            curQuest.questActionIndex++;

            if (questStartEvent != null)
                questStartEvent(_questId);
        }

        // Next Talk Target
        else if (curQuest.questActionIndex < curQuest.npcID.Length - 1)
        {
            if (_npcId == curQuest.npcID[curQuest.questActionIndex])
            {
                if (OngoingQuestList.ContainsValue(curQuest))
                {
                    if (CheckCompletable(curQuest.id))
                        curQuest.questActionIndex++;
                }
            }
        }

        // Talk Complete & Next Quest
        else if (curQuest.questActionIndex == curQuest.npcID.Length - 1 && OngoingQuestList.ContainsValue(curQuest))
        {
            if (_npcId == curQuest.npcID[curQuest.questActionIndex])
            {
                //Object Control
                ControlObject(_questId);
                CompleteQuest(_questId);
            }
        }

        // Quest Name
        return curQuest.name;
    }

    // 퀘스트 완료 가능 확인
    public bool CheckCompletable(int _questId)
    {
        BaseQuest quest = Managers.Quest.OngoingQuestList[_questId];
        if (quest == null)
            return false;

        switch (_questId)
        {
            case 10:
                if (quest.questActionIndex == OngoingQuestList[_questId].npcID.Length - 2)
                    foreach (UI_Slot_Item item in UI_InvenExtension.Instance.InvenItems)
                    {
                        if (item != null && item.itemName == "학생증")
                        {
                            quest.questActionIndex = OngoingQuestList[_questId].npcID.Length - 1;
                            return true;
                        }
                    }
                break;

            case 20:
                return true;

            case 30:
                return true;

            case 40:
                if (quest.questActionIndex == OngoingQuestList[_questId].npcID.Length - 2)
                    if (PlayerData.isClear_sildepuzzle)
                    {
                        OngoingQuestList[_questId].questActionIndex++;
                        return true;
                    }
                break;

            case 50:
                if (quest.questActionIndex == OngoingQuestList[_questId].npcID.Length - 2)
                {
                    if (PlayerData.isClear_sildepuzzle)
                    {
                        OngoingQuestList[_questId].questActionIndex++;
                        return true;
                    }
                }
                break;

            case 60:
                return true;

            case 70:
                if (quest.questActionIndex == OngoingQuestList[_questId].npcID.Length - 2)
                {
                    GameObject chalotte = GameObject.Find("Chalotte");
                    chalotte.GetComponent<BoxCollider2D>().enabled = false;
                    chalotte.GetComponent<CharacterInform>().enabled = false;

                    if (qusetGiver == null)
                    {
                        qusetGiver = Managers.Resource.Instantiate("Util/QuestGiver");
                        qusetGiver.GetComponent<QuestGiver>().QuestId = 70;
                        qusetGiver.GetComponent<QuestGiver>().SpeakerId = 2000;
                        qusetGiver.transform.position = GameObject.Find("questTriggerPos").transform.position;
                    }
                }
                return true;

            case 80:
                return true;

            case 90:
                if (quest.questActionIndex == OngoingQuestList[_questId].npcID.Length - 2)
                    if (PlayerData.isClearCori)
                    {
                        OngoingQuestList[_questId].questActionIndex++;
                        return true;
                    }
                    else
                    {
                        SceneController sceneController = GameObject.FindObjectOfType<SceneController>();
                        if (sceneController != null)
                        {
                            sceneController.nextScene = Define.SceneName.Battle;
                            sceneController.TimeChangedScene();
                        }
                    }
                break;
        }

        return false;
    }

    // 퀘스트 완료
    private void CompleteQuest(int _questId)
    {
        if (!CompletedQuestList.ContainsKey(_questId))
        {
            CompletedQuestList.Add(_questId, OngoingQuestList[_questId]);
            OngoingQuestList.Remove(_questId);

            if (questCompleteEvent != null)
                questCompleteEvent(_questId);
        }
    }

    // 퀘스트 상황에 따라 오브젝트 표시 또는 퀘스트 다이어로그 진행
    private void ControlObject(int _questId)
    {
        BaseQuest quest = null;
        Managers.Quest.OngoingQuestList.TryGetValue(_questId, out quest);

        if (quest == null)
            return;

        switch (_questId)
        {
            case 10:
                GameObject potal = Managers.Resource.Instantiate("Object/Potal");
                potal.GetComponent<TransferMap>()._NextSceneName = Define.SceneName.WatchTower01;
                break;

            case 20:
                // npc 럭스 삭제
                GameObject npc_lux = GameObject.Find("Lux");
                GameObject.Destroy(npc_lux);

                // 스킬 획득
                if (Managers.Event.GetSkillEvent != null)
                    Managers.Event.GetSkillEvent(0);

                // 보조 캐릭터 생성
                Managers.Resource.Instantiate("Chara/SubPlayer");
                potal = Managers.Resource.Instantiate("Object/Potal");

                // 포탈 생성
                potal.transform.position = new Vector3(-3.85f, 5.6f, 0);
                potal.GetComponent<TransferMap>()._NextSceneName = Define.SceneName.MainHall01;

                // Quest Giver 생성
                GameObject questGiver_30 = Managers.Resource.Instantiate("Util/QuestGiver");
                questGiver_30.transform.position = new Vector3(-1.7f, 4.57f, 0);
                questGiver_30.GetComponent<QuestGiver>().QuestId = 30;
                questGiver_30.GetComponent<QuestGiver>().SpeakerId = 1000;

                break;

            case 30:
                GameObject nina = GameObject.Find("NPC_Nina");
                nina.GetComponent<CharacterInform>().QuestID = 40;
                Managers.Resource.Instantiate("Util/NPCStateBubble", nina.transform);

                break;

            case 40:
                nina = GameObject.Find("NPC_Nina");
                nina.GetComponent<CharacterInform>().QuestID = 60;
                Managers.Resource.Instantiate("Util/NPCStateBubble", nina.transform);

                break;

            case 50:
                foreach (UI_Slot_Item slot_Item in UI_QuickSlot.Instance.QuickItems)
                {
                    if (slot_Item.MyItem != null)
                        if (slot_Item.MyItem.Id == 103)
                        {
                            UI_QuickSlot.Instance.OnSlotClick(slot_Item);
                            break;
                        }
                }
                GameObject map = Managers.Resource.Instantiate("Item/지도");
                if (map != null)
                {
                    Item_Map mapItem = map.GetComponent<Item_Map>();

                    Managers.Data.ItemDic.TryGetValue(mapItem.Id, out mapItem.itemData);
                    UI_InvenExtension.Instance.AddItem(mapItem);

                    GameObject.Destroy(map);
                }
                break;

            case 60:
                nina = GameObject.Find("NPC_Nina");
                nina.GetComponent<CharacterInform>().QuestID = 70;
                Managers.Resource.Instantiate("Util/NPCStateBubble", nina.transform);
                break;

            case 70:
                if (Managers.Event.GetSkillEvent != null)
                {
                    Managers.Event.GetSkillEvent(-1);
                }
                break;

            case 80:
                foreach (UI_Slot_Item slot_Item in UI_QuickSlot.Instance.QuickItems)
                {
                    if (slot_Item.MyItem != null)
                        if (slot_Item.MyItem.Id == 105)
                        {
                            slot_Item.MyItem.ItemData.isUsable = true;
                            slot_Item.UseItem();
                            break;
                        }
                }
                break;
        }

        return;
    }
}