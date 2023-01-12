using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    // 현재까지 최근에 사용한 order 저장
    private int _order = 0;

    // 가장 마지막에 띄운 UI 삭제 될 수 있게 stack
    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();

    private UI_Scene _SceneUI = null;

    public GameObject root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
            {
                root = new GameObject { name = "@UI_Root" };
            }
            root.tag = "UI_Root";
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = (_order);
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    // T : UI_Scene을 상속받은 Scene UI의 작동 클래스(ex>UI_normal_Skill)  name : 팝업UI 프리팹 이름(ex>SkillUI)
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        // 만약 이름이 없다면 T가 적용된 게임 오브젝트의 이름을 넣어준다.
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _SceneUI = sceneUI;

        // 새로 띄우는 팝업창을 UI_Root 태그를 가진 GO의 자식으로 설정한다.
        GameObject root = GameObject.FindGameObjectWithTag("UI_Root");
        if (root == null)
        {
            root = new GameObject { name = "@UI_Root" };
            root.tag = "UI_Root";
        }
        go.transform.SetParent(root.transform);

        return sceneUI;
    }

    // T : UI_Popup을 상속받은 PopupUI의 작동 클래스(ex>UI_Popup_Skill)  name : 팝업UI 프리팹 이름(ex>SkillUI)
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        // 만약 이름이 없다면 T가 적용된 게임 오브젝트의 이름을 넣어준다.
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        // 새로 띄우는 팝업창을 UI_Root 태그를 가진 GO의 자식으로 설정한다.
        GameObject root = GameObject.FindGameObjectWithTag("UI_Root");
        if (root == null)
        {
            root = new GameObject { name = "UI_Root" };
            root.tag = "UI_Root";
        }
        go.transform.SetParent(root.transform);

        return popup;
    }

    public bool IsPopupUIExist()
    {
        if (_popupStack.Count > 0)
            return true;
        else
            return false;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;
        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed");
            return;
        }
        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        // 팝업 스택에서 가장 최근에 열린 팝업을 가져와 삭제
        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;

        _order--;
    }

    public void CloseAllPopUI()
    {
        while (_popupStack.Count > 0)
        {
            CloseAllPopUI();
        }
    }
}