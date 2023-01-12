using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InputFieldFocus : InputField
{
    IEnumerator MoveEndCoroutine()
    {
        yield return null;
        MoveTextEnd(false); // 커서의 맨끝으로 이동
    }

    // OnDeselect(새 개체가 선택 될 때 EventSystem에 의해 호출)
    public override void OnDeselect(BaseEventData eventData)
    {
        ActivateInputField(); // 호출되면 InputField 활성화
        StartCoroutine(MoveEndCoroutine());

        base.OnDeselect(eventData);
    }
}
