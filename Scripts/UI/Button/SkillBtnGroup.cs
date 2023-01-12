using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillBtnGroup : MonoBehaviour
{
    private static SkillBtnGroup skillBtnGroup;

    public List<SkillButton> buttons;
    public SkillButton selected;
    public GameObject description;

    private BaseSkill skill;

    private const string NORMAL_ANIM = "Normal";
    private const string ENTER_ANIM = "Highlighted";
    private const string CLICK_ANIM = "Selected";
    private const string DOUBLE_ANIM = "Double Click";

    public static SkillBtnGroup MySkillBtnGroup { get => skillBtnGroup; }

    public bool canSkill = true; // 마나 검사

    private void Start()
    {
        if (skillBtnGroup == null)
            skillBtnGroup = this;
        else
            if (skillBtnGroup != this)
            Destroy(this.gameObject);

        BattleSystem.MyBattleSystem.BattleStateChanged -= OnBattleStateChanged;
        BattleSystem.MyBattleSystem.BattleStateChanged += OnBattleStateChanged;

        gameObject.SetActive(false);
    }

    public void OnBattleStateChanged(BattleState _battleState)
    {
        GroupReset();
        if (_battleState != BattleState.PLAYERTURN)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);

        if (_battleState == BattleState.ENEMYDOWN)
            gameObject.SetActive(true);
    }

    public void Subscribe(SkillButton button)
    {
        if (buttons == null)
        {
            buttons = new List<SkillButton>();
        }
        buttons.Add(button);
    }

    public void OnBtnSelected(SkillButton button)
    {
        // 버튼 2회 클릭 : 스킬 실행
        if (selected == button)
        {
            if (skill != null)
            {
                BattleSystem.MyBattleSystem.OnStartActSkill(skill);
            }
            else
                return;

            if (canSkill)// 마나 검사
            {
                button.GetComponent<Animator>().SetTrigger(DOUBLE_ANIM);
                description.SetActive(false); // 설명창 false
                button.OnSkill();
            }
        }

        // 버튼 1회 클릭 : 스킬 설명창
        else
        {
            selected = button;

            // 버튼의 스킬 정보 가져오기
            Managers.Data.SkillDic.TryGetValue(selected.skillId, out skill);
            if (skill == null)
            {
                Debug.Log($"잘못된 스킬 ID입니다. ID:({selected.skillId})");
                return;
            }

            BtnReset();
            button.GetComponent<Animator>().SetTrigger(CLICK_ANIM);
            description.SetActive(true); // 설명창 ture

            // 스킬 설명창 세팅
            TextMeshProUGUI skillName = description.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI skillDescription = description.transform.Find("Description").GetComponent<TextMeshProUGUI>();

            skillName.text = skill.name;
            skillDescription.text = skill.description;
        }
    }

    public void OnBtnEnter(SkillButton button)
    {
        BtnReset();
        if (selected == null || button != selected)
            button.GetComponent<Animator>().SetTrigger(ENTER_ANIM);
    }

    public void OnBtnExit(SkillButton button)
    {
        BtnReset();
    }

    public void BtnReset()
    {
        foreach (SkillButton button in buttons) // 선택 중복 검사
        {
            if (selected != null && button == selected) { continue; }
            button.GetComponent<Animator>().SetTrigger(NORMAL_ANIM);
        }
    }

    public void GroupReset() // 선택 리셋
    {
        if (selected != null)
        {
            selected.OffSkill();
            selected.GetComponent<Animator>().SetTrigger(NORMAL_ANIM);
            description.SetActive(false);
            selected = null;
        }
    }
}