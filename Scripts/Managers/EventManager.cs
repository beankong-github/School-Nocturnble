using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    /* 기타 이벤트 */

    public delegate void EventDelegate();

    public EventDelegate SubPlayerGenerated;

    /* 스킬 이벤트*/

    public delegate void SkillDelegate(int _skillId);

    public SkillDelegate GetSkillEvent;
}