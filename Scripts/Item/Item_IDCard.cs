using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_IDCard : Item
{
    private UI_InvenExtension inven;

    private bool canGet;

    private void Start()
    {
        InitItem();

        if (!ConnectInven())
            Invoke("ConnectInven", 0.5f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canGet = true;
            Managers.Resource.Instantiate("Util/E", collision.gameObject.transform);
        }

        if (collision.tag == "SubPlayer")
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
        if (!ItemData.isUsable)
        {
            Debug.Log("이곳에서 학생증을 사용할 수 없습니다.");
            return false;
        }
        else
            Debug.Log("학생증 사용");
        return true;
    }

    private bool ConnectInven()
    {
        inven = UI_InvenExtension.Instance;

        if (inven == null)
        {
            Debug.Log("인벤 연결 실패");
            return false;
        }

        return true;
    }
}