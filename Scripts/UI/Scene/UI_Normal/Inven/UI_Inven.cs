using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{
    private static UI_Inven instance;

    private bool isExtensionActive;

    public static UI_Inven MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UI_Inven>();
            }

            return instance;
        }
    }

    private void OnEnable()
    {
        if (UI_Dialog.MyInstance == null)
        {
            Invoke("OnEnable", 0.3f);
            return;
        }
        UI_Dialog.MyInstance.DialogActiveEvent += HideIcons;
        UI_Dialog.MyInstance.DialogDeactiveEvent += ShowIcons;

        UI_Icon.MyInstance.OnInvenClicked();
    }

    private void OnDisable()
    {
        UI_Dialog.MyInstance.DialogActiveEvent -= HideIcons;
        UI_Dialog.MyInstance.DialogDeactiveEvent -= ShowIcons;
    }

    private void ShowIcons()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;

            child.SetActive(true);
        }

        if (!isExtensionActive)
            UI_Icon.MyInstance.OnInvenClicked();
    }

    private void HideIcons()
    {
        isExtensionActive = UI_InvenExtension.Instance.gameObject.activeSelf;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;

            child.SetActive(false);
        }
    }
}