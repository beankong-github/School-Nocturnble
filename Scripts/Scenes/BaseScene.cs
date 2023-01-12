using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    protected virtual void Init()
    {
        // 이벤트 시스템 생성
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
        {
            obj = Managers.Resource.Instantiate("UI/EventSystem");
        }
    }

    public abstract void Clear();
}