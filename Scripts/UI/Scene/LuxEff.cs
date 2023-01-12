using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuxEff : MonoBehaviour
{
    public Sprite link_Off;

    private bool show_Eff = false;

    private Sprite link_On;
    private GameObject link;

    private void Start()
    {
        link = GameObject.Find("Link");
        link_On = link.GetComponent<Image>().sprite; // 기존 sprite 이미지 저장
    }

    private void Update()
    {
        Check();
    }

    private void Check()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            show_Eff = !show_Eff;

        if (!show_Eff)
        {
            GameObject.Find("Lux_Inform").transform.Find("Lux_Activ").gameObject.SetActive(false); // 이펙트 비활성화
            GameObject.Find("Icon").transform.Find("Icon_Btn").gameObject.SetActive(true); // 아이콘 활성화
            link.GetComponent<Image>().sprite = link_On; // sprite 이미지 교체
        }
        else
        {
            GameObject.Find("Lux_Inform").transform.Find("Lux_Activ").gameObject.SetActive(true); //이펙트 활성화
            GameObject.Find("Inven").transform.Find("InvenExtension_Popup").gameObject.SetActive(false); // 확장 인벤 비활성화
            GameObject.Find("Icon").transform.Find("Icon_Btn").gameObject.SetActive(false); // 아이콘 비활성화
            link.GetComponent<Image>().sprite = link_Off; // sprite 이미지 교체
        }
    }
}