using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item_ApplePie : Item
{
    [SerializeField]
    private float health;

    private UI_InvenExtension inven;

    private bool canGet;

    private void Start()
    {
        Id = 101;

        InitItem();

        inven = FindObjectOfType<UI_InvenExtension>();

        if (inven == null)
        {
            Debug.Log("인벤 연결 실패");
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canGet = true;
            Managers.Resource.Instantiate("Util/E", collision.gameObject.transform);
        }
        else if (collision.tag == "SubPlayer")
        {
            canGet = true;
            Managers.Resource.Instantiate("Util/E", collision.gameObject.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canGet = false;
            GuideEKey guide = collision.GetComponentInChildren<GuideEKey>();
            if (guide != null)
            {
                Destroy(guide.gameObject);
            }
        }
    }

    private void Update()
    {
        if (canGet && Input.GetKey(KeyCode.E))
        {
            if (!inven.AddItem(this))
                Debug.Log($"{ItemData.name} 획득 실패");
            else
                Destroy(this.gameObject);
        }
    }

    public override bool UseItem()
    {
        PlayerController playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        if (playerController == null)
            return false;

        if (playerController.GetHP() < playerController.GetMaxHP() && ItemData.isUsable)
        {
            playerController.SetHP(playerController.Hp.unitCurrentValue + health);
            Debug.Log("포션 사용");
            return true;
        }
        else
            Debug.Log("최대 체력 상태에서 포션을 사용할 수 없습니다.");

        return false;
    }
}