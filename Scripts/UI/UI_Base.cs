using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);

        // UI의 요소들을 읽어와 타입별로 Dictionary에 추가한다.
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        // UI의 요소들에 해당하는 컴포넌트를 읽어온다.(버튼일 경우 해당 게임 오브젝트의 Button 컴포넌트를 찾는다.)
        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to bind!({names[i]})");
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected Text GetText(int idx)
    {
        return Get<Text>(idx);
    }

    protected Button GetButton(int idx)
    {
        return Get<Button>(idx);
    }

    protected Image GetImage(int idx)
    {
        return Get<Image>(idx);
    }

    protected GameObject GetGameObject(int idx)
    {
        return Get<GameObject>(idx);
    }

    public static void AddUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnPointerClickHandler -= action;
                evt.OnPointerClickHandler += action;
                break;
                
            case Define.UIEvent.RightClick:
                evt.OnPointerRightClickHandler -= action;
                evt.OnPointerRightClickHandler += action;
                break;


            case Define.UIEvent.DoubleClick:
                evt.OnPointerDoubleClickHandler -= action;
                evt.OnPointerDoubleClickHandler += action;
                break;

            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;

            case Define.UIEvent.Up:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action;
                break;

            case Define.UIEvent.Down:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action;
                break;

            case Define.UIEvent.Enter:
                evt.OnPointerEnterHandler -= action;
                evt.OnPointerEnterHandler += action;
                break;

            case Define.UIEvent.Exit:
                evt.OnPointerExitHandler -= action;
                evt.OnPointerExitHandler += action;
                break;
        }
    }
}