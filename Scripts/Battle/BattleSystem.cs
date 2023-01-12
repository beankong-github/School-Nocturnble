using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, ENEMYDOWN, WON, LOST };

public class BattleSystem : MonoBehaviour
{
    private static BattleSystem battleSystem;
    public static BattleSystem MyBattleSystem { get => battleSystem; }

    public BattleState state;

    public delegate void BattleDelegate(BattleState battleState);

    public BattleDelegate BattleStateChanged;

    public GameObject enemy_Go;
    public GameObject player_Go;

    private Unit playerUnit;
    private Unit enemyUnit;

    public TextMeshProUGUI battleDialogue;

    public UnitStat playerHPStat;
    public UnitStat playerMPStat;
    public UnitStat enemyHPStat;

    private bool additionalDamage;
    private bool canSkill;
    private bool isDefense;

    public SceneController sceneController;

    private void Start()
    {
        if (battleSystem == null)
            battleSystem = this;
        else
            if (battleSystem != this)
            Destroy(this.gameObject);

        state = BattleState.START;
        if (BattleStateChanged != null)
            BattleStateChanged(state);
        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {
        // 적과 플래이어 생성
        enemyUnit = enemy_Go.GetComponent<Unit>();
        playerUnit = player_Go.GetComponent<Unit>();

        battleDialogue.text = "저주에 걸린 " + enemyUnit.unitName + "와 싸워 저주를 풀어주자!";

        // 적과 플래이어의 체력바 설정
        playerHPStat.Initialized(playerUnit);
        enemyHPStat.Initialized(enemyUnit);
        playerMPStat.Initialized(30, 30);

        // 플레이어 MP값 설정

        yield return new WaitForSeconds(2f);

        // PlayerTurn으로 전환
        state = BattleState.PLAYERTURN;
        if (BattleStateChanged != null)
            BattleStateChanged(state);
        PlayerTurn();
    }

    private IEnumerator CheckMP(BaseSkill _skill)
    {
        if (playerMPStat.unitCurrentValue < _skill.mana)
        {
            canSkill = false;
            battleDialogue.text = "마력이 부족하여 해당 스킬을 사용할 수 없습니다.";
            yield return new WaitForSeconds(1.5f);

            battleDialogue.text = null;
        }
        canSkill = true;
        SkillBtnGroup.MySkillBtnGroup.canSkill = true;//추가한 코드
    }

    private void PlayerTurn()
    {
        battleDialogue.text = "당신의 행동을 선택하세요.";
    }

    private IEnumerator PlayerAttack(BaseSkill _skill)
    {
        // 스킬 중 작동
        playerMPStat.setCurrentValue(playerMPStat.unitCurrentValue - _skill.mana);
        battleDialogue.text = null;
        yield return new WaitForSeconds(1.8f);

        // 스킬 후 작동
        float damage = additionalDamage ? _skill.damage * 2.3f : _skill.damage;
        bool isDead = enemyUnit.TakeDamage((int)damage);
        enemyHPStat.setCurrentValue(enemyUnit.currentHP);
        battleDialogue.text = "당신은 " + _skill.name + $"을 시전하여 {damage}만큼 공격했다!";

        additionalDamage = false;

        yield return new WaitForSeconds(3f);

        // 적이 죽었는지 확인
        if (isDead)
        {
            // 체력이 0이면 궁극기 사용 가능 상태로 전환
            state = BattleState.ENEMYDOWN;
            battleDialogue.text = null;
            if (BattleStateChanged != null)
                BattleStateChanged(state);

            // 궁극기 사용 가능
            SetActiveUlt();
        }
        else
        {
            // 적의 차례
            state = BattleState.ENEMYTURN;
            if (BattleStateChanged != null)
                BattleStateChanged(state);

            StartCoroutine(EnemyTurn());
        }
    }

    private IEnumerator PlayerHeal(BaseSkill _skill)
    {
        // 스킬 중 작동
        battleDialogue.text = null;
        playerMPStat.setCurrentValue(playerMPStat.unitCurrentValue - _skill.mana);
        yield return new WaitForSeconds(1.8f);

        // 스킬 후 작동
        bool isHealed = playerUnit.Heal(_skill.heal);
        playerHPStat.setCurrentValue(playerUnit.currentHP);
        if (isHealed) battleDialogue.text = "당신은 " + _skill.name + "을 시전하여 " + _skill.heal + "만큼 회복했다!";
        else battleDialogue.text = "당신은 " + _skill.name + "을 시전했으나 이미 체력이 가득하다.";
        yield return new WaitForSeconds(3f);

        // 적의 차례
        state = BattleState.ENEMYTURN;
        if (BattleStateChanged != null)
            BattleStateChanged(state);

        StartCoroutine(EnemyTurn());
    }

    // 플레이어 공격 강화
    private IEnumerator PlayerStrength(BaseSkill _skill)
    {
        // 스킬 중 작동
        additionalDamage = true;
        battleDialogue.text = null;
        playerMPStat.setCurrentValue(playerMPStat.unitCurrentValue - _skill.mana);
        yield return new WaitForSeconds(1.8f);

        // 스킬 후 작동
        battleDialogue.text = _skill.name + "을 시전하여 다음 공격을 강화한다!";
        yield return new WaitForSeconds(3f);

        // 적의 차례
        state = BattleState.ENEMYTURN;
        if (BattleStateChanged != null)
            BattleStateChanged(state);

        StartCoroutine(EnemyTurn());
    }

    private IEnumerator PlayerDefense(BaseSkill _skill)
    {
        // 스킬 중 작동
        isDefense = true;
        battleDialogue.text = null;
        yield return new WaitForSeconds(1.8f);

        // 스킬 후 작동
        battleDialogue.text = "당신은 " + enemyUnit.unitName + "의 다음 공격을 방어한다.";
        yield return new WaitForSeconds(3f);

        // 적의 차례
        state = BattleState.ENEMYTURN;
        if (BattleStateChanged != null)
            BattleStateChanged(state);

        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        // 공격
        if (enemyUnit.currentHP > 15)
        {
            battleDialogue.text = "저주에 걸린 " + enemyUnit.unitName + "가 공격한다.";
            yield return new WaitForSeconds(1.5f);

            battleDialogue.text = null;
            enemy_Go.transform.Find("Enemy_Attack").gameObject.SetActive(true);
            yield return new WaitForSeconds(2.5f);

            int damage = isDefense ? Random.Range(0, 2) : enemyUnit.damage + Random.Range(-1, 3);
            isDefense = false;

            bool isDead = playerUnit.TakeDamage(damage);
            playerHPStat.setCurrentValue(playerUnit.currentHP);
            battleDialogue.text = enemyUnit.unitName + "는 당신에게 " + damage + "만큼의 피해를 주었다!";
            yield return new WaitForSeconds(2f);

            enemy_Go.transform.Find("Enemy_Attack").gameObject.SetActive(false);

            // 플레이어 사망
            if (isDead)
            {
                battleDialogue.text = null;
                EndBattle(BattleState.LOST);
            }
            // 플레이어 생존 -> 플레이어 턴으로 전환
            else
            {
                state = BattleState.PLAYERTURN;
                if (BattleStateChanged != null)
                    BattleStateChanged(state);
                PlayerTurn();
            }
        }

        // 회복
        else
        {
            battleDialogue.text = "저주에 걸린 " + enemyUnit.unitName + "가 회복한다.";
            yield return new WaitForSeconds(1.5f);

            battleDialogue.text = null;
            enemy_Go.transform.Find("Enemy_Heal").gameObject.SetActive(true);
            yield return new WaitForSeconds(2.5f);

            int healValue = 6 + Random.Range(-1, 4);
            bool isHealed = enemyUnit.Heal(healValue);
            enemyHPStat.setCurrentValue(enemyUnit.currentHP);
            if (isHealed) battleDialogue.text = enemyUnit.unitName + "는 " + healValue + "만큼 회복했다!";
            yield return new WaitForSeconds(1.5f);

            enemy_Go.transform.Find("Enemy_Heal").gameObject.SetActive(false);

            state = BattleState.PLAYERTURN;
            if (BattleStateChanged != null)
                BattleStateChanged(state);
            PlayerTurn();
        }
    }

    private void SetActiveUlt()
    {
        // 궁극기 사용 가능
        battleDialogue.text = enemyUnit.unitName + "가 약화되었습니다. " +
            "궁극기를 사용해 저주에서 해방시키십시오.";
    }

    private void enemyDead()
    {
        // 체력이 0이면 궁극기 사용 가능 상태로 전환
        state = BattleState.ENEMYDOWN;
        if (BattleStateChanged != null)
            BattleStateChanged(state);
    }

    public void EndBattle(BattleState _state)
    {
        state = _state;
        if (BattleStateChanged != null)
            BattleStateChanged(state);

        if (state == BattleState.WON)
        {
            PlayerData.isClearCori = true;
            battleDialogue.text = "당신은 " + enemyUnit.unitName + "를 저주에서 해방시켰습니다!";
            StartCoroutine(successScene());
        }
        else if (state == BattleState.LOST)
        {
            PlayerData.isClearCori = false;
            battleDialogue.text = "당신은 " + enemyUnit.unitName + "와의 전투에서 패배했습니다.";

            // Game Over
            StartCoroutine(failScene());
        }
    }

    private IEnumerator successScene()
    {
        Managers.Resource.Instantiate("Effect/Skill/Ult_Effect", playerUnit.transform);

        yield return new WaitForSeconds(3.0f);

        // 기존씬으로 이동
        sceneController.nextScene = Define.SceneName.MainHall03;
        sceneController.TimeChangedScene();
    }

    private IEnumerator failScene()
    {
        yield return new WaitForSeconds(3.0f);

        // 기존씬으로 이동
        sceneController.TimeChangedScene();
    }

    public void OnStartActSkill(BaseSkill _skill)
    {
        if (state == BattleState.ENEMYDOWN)
        {
            if (_skill.id == 5)
            {
                Managers.Resource.Instantiate("Effect/Skill/Ult_Canvas"); // 수정한 코드
                return;
            }
        }
        else if (state != BattleState.PLAYERTURN)
            return;

        // 마나 체크
        StartCoroutine(CheckMP(_skill));

        if (!canSkill)
        {
            SkillBtnGroup.MySkillBtnGroup.GroupReset();
            SkillBtnGroup.MySkillBtnGroup.canSkill = false;//추가한 코드
            return;
        }

        // 스킬 이팩트 작동 & 스킬 실행
        GameObject effet = null;
        switch (_skill.id)
        {
            case 1:
                effet = Managers.Resource.Instantiate("Effect/Skill/Fireball_Effect", player_Go.transform);
                StartCoroutine(PlayerAttack(_skill));
                break;

            case 2:
                effet = Managers.Resource.Instantiate("Effect/Skill/Heal_Effect", player_Go.transform);
                StartCoroutine(PlayerHeal(_skill));
                break;

            case 3:
                effet = Managers.Resource.Instantiate("Effect/Skill/Strength_Effect", player_Go.transform);
                StartCoroutine(PlayerStrength(_skill));
                break;

            case 4:
                effet = Managers.Resource.Instantiate("Effect/Skill/IceSpear_Effect", player_Go.transform);
                StartCoroutine(PlayerAttack(_skill));
                break;

            case 6:
                effet = Managers.Resource.Instantiate("Effect/Skill/Defense", player_Go.transform);
                StartCoroutine(PlayerDefense(_skill));
                break;

            case 7:
                StartCoroutine(failScene());
                break;
        }
        Destroy(effet, 5f);
    }
}