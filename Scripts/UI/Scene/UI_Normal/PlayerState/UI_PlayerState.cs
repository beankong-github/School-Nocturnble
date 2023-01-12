using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerState : UI_Scene
{
    private SceneController scene;

    private void OnEnable()
    {
        scene = GameObject.Find("@Scene").GetComponent<SceneController>();

        if (scene.sceneType != Define.SceneType.Game)
            return;

        if (UI_Dialog.MyInstance == null)
        {
            Invoke("OnEnable", 0.3f);
            return;
        }
        UI_Dialog.MyInstance.DialogActiveEvent += HideStates;
        UI_Dialog.MyInstance.DialogDeactiveEvent += ShowStates;
    }

    private void OnDisable()
    {
        if (scene.sceneType != Define.SceneType.Game)
            return;

        UI_Dialog.MyInstance.DialogActiveEvent -= HideStates;
        UI_Dialog.MyInstance.DialogDeactiveEvent -= ShowStates;
    }

    private void ShowStates()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;

            child.SetActive(true);
        }

        if (SubPlayerController.MyInstance == null || !SubPlayerController.MyInstance.gameObject.activeSelf)
        {
            gameObject.transform.Find("Lux").gameObject.SetActive(false);
        }
    }

    private void HideStates()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;

            child.SetActive(false);
        }
    }

    private void Start()
    {
        Init();
    }
}