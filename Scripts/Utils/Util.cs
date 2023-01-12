using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    // 컴포넌트를 가져오거나 컴포넌트를 추가하는 함수
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();

        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    // 게임 오브젝트 go의 자식 중 'name'이라는 이름을 가진 게임 오브젝트를 찾는 함수
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;
        else
            return transform.gameObject;
    }

    // 게임 오브젝트 go의 'name'이라는 이름을 가진 컴포넌트를 찾는 함수.(recursive? 자식의 자식까지 탐색 : 자식 만 탐색)
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        // 자식의 자식까지 재귀적으로 탐색
        if (recursive)
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }
        // 자식만 탐색
        else
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }

        return null;
    }

    // 게임 종료
    public static void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}