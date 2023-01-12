using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 플레이어 HP와 MP 관리
public class UnitStat : MonoBehaviour
{
    public Image content;

    [SerializeField]
    private float currentFill;          // 현재 상태바에 등록된 플레이어의 HP/MP 상태값

    [SerializeField]
    private float currentValue;         // 현재 플레이어의 HP/MP 값

    public float speed;                 // 피가 줄어들고 늘어나는 속도

    public float unitMaxValue { get; set; }

    public float unitCurrentValue
    {
        get { return currentValue; }
        set
        {
            currentValue = value < unitMaxValue ? value : unitMaxValue;
            if (value < 0) currentValue = 0;

            currentFill = currentValue / unitMaxValue;
        }
    }

    private void Start()
    {
        currentValue = unitMaxValue;
        currentFill = 1;
        content = GetComponent<Image>();
    }

    private void Update()
    {
        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(currentFill, content.fillAmount, Time.deltaTime * speed);
        }
    }

    public void Initialized(float currentValue, float maxValue)
    {
        unitMaxValue = maxValue;
        unitCurrentValue = currentValue;
    }

    public void Initialized(Unit unit)
    {
        unitMaxValue = unit.maxHP;
        unitCurrentValue = unit.currentHP;
    }

    public void setCurrentValue(float _currenValue)
    {
        unitCurrentValue = _currenValue;
    }
}