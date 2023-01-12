using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item_MapPiece : Item, IPointerClickHandler
{
    private SceneController SceneController;

    private void Start()
    {
        InitItem();
    }

    public override bool UseItem()
    {
        if (!PlayerData.isClear_sildepuzzle)
        {
            SceneController = FindObjectOfType<SceneController>();
            SceneController.nextScene = Define.SceneName.SlidePuzzle;

            SceneController.TimeChangedScene();
            return false;
        }
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (UI_InvenExtension.Instance.AddItem(this))
            Debug.Log($"{ItemData.name} 획득 실패");
        else
            Destroy(this.gameObject);
    }
}