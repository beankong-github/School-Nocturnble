using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MiniMap : UI_Scene
{
    public static UI_MiniMap Instance;
    public RectTransform characterImgPos;

    public SceneController sceneController;

    private Define.SceneName sceneName;

    private void OnEnable()
    {
        if (UI_Dialog.MyInstance == null)
        {
            Invoke("OnEnable", 0.3f);
            return;
        }
        UI_Dialog.MyInstance.DialogActiveEvent += HideMiniMap;
        UI_Dialog.MyInstance.DialogDeactiveEvent += ShowMiniMap;

        SceneManager.sceneLoaded += newSceneLoaded;
    }

    private void OnDisable()
    {
        UI_Dialog.MyInstance.DialogActiveEvent -= HideMiniMap;
        UI_Dialog.MyInstance.DialogDeactiveEvent -= ShowMiniMap;
        SceneManager.sceneLoaded -= newSceneLoaded;
    }

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            if (Instance != this)
                Destroy(Instance.gameObject);
        }
        sceneController = FindObjectOfType<SceneController>();
        sceneName = sceneController.sceneName;

        SetCharaterImgPos();
    }

    private void SetCharaterImgPos()
    {
        switch (sceneName)
        {
            case (Define.SceneName.MainHall01):
            case (Define.SceneName.MainHall02):
            case (Define.SceneName.MainHall03):
                characterImgPos.anchoredPosition = new Vector2(-14.2f, -42.5f);
                break;

            case (Define.SceneName.Library01):
            case (Define.SceneName.Library02):
            case (Define.SceneName.Library03):
                characterImgPos.anchoredPosition = new Vector2(-105.4f, -18.8f);
                break;

            case (Define.SceneName.WestHallway01):
            case (Define.SceneName.WestHallway02):
                characterImgPos.anchoredPosition = new Vector2(-75f, -45.9f);
                break;

            case (Define.SceneName.EastHallway01):
            case (Define.SceneName.EastHallway02):
                characterImgPos.anchoredPosition = new Vector2(45.8f, -45.9f);
                break;

            case (Define.SceneName.PrincipalOffice01):
            case (Define.SceneName.PrincipalOffice02):
                characterImgPos.anchoredPosition = new Vector2(102.9f, -45.9f);
                break;
        }
    }

    private void newSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        sceneController = FindObjectOfType<SceneController>();
        sceneName = sceneController.sceneName;
        SetCharaterImgPos();
    }

    private void ShowMiniMap()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;

            child.SetActive(true);
        }
    }

    private void HideMiniMap()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;

            child.SetActive(false);
        }
    }
}