using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecBtnGroup : MonoBehaviour
{
    public List<BattleButton> buttons;
    public List<GameObject> objects;
    public BattleButton selected;
    public SkillBtnGroup skillGroup;

    private const string CLICK_ANIM = "Selected";
    private const string ENTER_ANIM = "Highlighted";
    private const string NORMAL_ANIM = "Normal";

    private void Start()
    {
        BattleSystem.MyBattleSystem.BattleStateChanged -= OnBattleStateChanged;
        BattleSystem.MyBattleSystem.BattleStateChanged += OnBattleStateChanged;

        selected = transform.Find("btn_Skill").GetComponent<BattleButton>();
        selected.GetComponent<Animator>().SetTrigger(CLICK_ANIM);

        gameObject.SetActive(false);
    }

    public void OnBattleStateChanged(BattleState _battleState)
    {
        if (_battleState != BattleState.PLAYERTURN)
            gameObject.SetActive(false);
        else
        {
            gameObject.SetActive(true);
            if (selected != null)
                selected.GetComponent<Animator>().SetTrigger(CLICK_ANIM);
        }

        if (_battleState == BattleState.ENEMYDOWN) // 궁극기 버튼 활성화
        {
            gameObject.SetActive(true);
            
            for (int i = 0; i < buttons.Count; i++) 
            {
                buttons[i].gameObject.SetActive(false); // 버튼 fasle
                objects[i].gameObject.SetActive(false); // 버튼 -> 선택창 flase
            }

            transform.Find("btn_Ult").gameObject.SetActive(true); // 궁극기 버튼
        }
    }

    public void Subscribe(BattleButton button)
    {
        if (buttons == null)
            buttons = new List<BattleButton>();
        buttons.Add(button);
    }

    public void OnBtnSelected(BattleButton button)
    {
        selected = button;
        Reset();
        button.GetComponent<Animator>().SetTrigger(CLICK_ANIM);
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objects.Count; i++)
        {
            if (i == index)
                objects[i].SetActive(true);
            else
            {
                skillGroup.GroupReset();
                objects[i].SetActive(false);
            }
        }
    }

    public void OnBtnEnter(BattleButton button)
    {
        Reset();
        if (selected == null || button != selected)
            button.GetComponent<Animator>().SetTrigger(ENTER_ANIM);
    }

    public void OnBtnExit(BattleButton button)
    {
        Reset();
    }

    public void Reset()
    {
        foreach (BattleButton button in buttons)
        {
            if (selected != null && button == selected) { continue; }
            button.GetComponent<Animator>().SetTrigger(NORMAL_ANIM);
        }
    }
}