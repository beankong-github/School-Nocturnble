using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 키보드의 입력을 가져와 PlayerController를 조작하고 플레이어를 움직이는 클래스
public class PlayerMain : MonoBehaviour
{
    //== 전역 변수 ===============================
    public static PlayerMain player;

    private PlayerController _playerCtrl;

    private GameObject _subPlayer;
    private SubPlayerController _subPlayerCtrl;

    private bool _isSubplayerExist;

    public GameObject skillEffect;
    public GameObject playerLight;
    public PlayerController PlayerCtrl { get => _playerCtrl; set => _playerCtrl = value; }

    //== Unity 함수 ==============================
    private void Start()
    {
        /*        // Player 싱글톤
                if (player == null)
                {
                    player = this;
                }
                else
                {
                    Destroy(this.gameObject);
                }*/

        PlayerCtrl = GetComponent<PlayerController>();

        Managers.Event.SubPlayerGenerated += OnSubPlayerGenerated;
        Managers.Event.GetSkillEvent += ActiveSkillEffect;
    }

    private void OnDestroy()
    {
        Managers.Event.SubPlayerGenerated -= OnSubPlayerGenerated;
        Managers.Event.GetSkillEvent -= ActiveSkillEffect;
    }

    private void Update()
    {
        // 조작 가능한 상태가 아니면 종료(return)
        if (!PlayerCtrl.activeSts)
        {
            return;
        }

        if (PlayerData.isDark)
        {
            if (!playerLight.activeSelf)
                playerLight.SetActive(true);
        }

        OnKeyboard();
    }

    // == 기능 함수 ==========================
    private void OnKeyboard()
    {
        // 좌우이동
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            PlayerCtrl.MoveChar(1.0f);
            if (_isSubplayerExist && !_subPlayerCtrl.isSubPlayerAct)
            {
                _subPlayerCtrl.targetDir = 0.0f;
            }
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            PlayerCtrl.MoveChar(-1.0f);
            if (_isSubplayerExist && !_subPlayerCtrl.isSubPlayerAct)
            {
                _subPlayerCtrl.targetDir = 180.0f;
            }
        }

        // 멈추기
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            PlayerCtrl.PlayerIdle();
        }

        // 달리기
        PlayerCtrl.CheckRun(Input.GetKey(KeyCode.LeftShift));

        // 점프
        if (Input.GetButtonDown("Jump"))
        {
            PlayerCtrl.ActionJump();
        }
    }

    private void OnSubPlayerGenerated()
    {
        PlayerData.isSubPlayerExist = _isSubplayerExist = true;
        _subPlayer = GameObject.FindGameObjectWithTag("SubPlayer");
        _subPlayerCtrl = _subPlayer.GetComponent<SubPlayerController>();
    }

    private void ActiveSkillEffect(int skillId)
    {
        BaseSkill skill;

        if (skillId == -1)
        {
            for (int i = 0; i < Managers.Data.SkillDic.Count; i++)
            {
                Managers.Data.SkillDic.TryGetValue(i, out skill);

                if (skill != null)
                {
                    skill.isAcquired = true;
                }
            }

            StartCoroutine(waitForSec());
        }
        else
        {
            Managers.Data.SkillDic.TryGetValue(skillId, out skill);

            if (skill != null)
            {
                skill.isAcquired = true;
                StartCoroutine(waitForSec());
            }
        }
    }

    private IEnumerator waitForSec()
    {
        yield return new WaitForSeconds(0.3f);
        skillEffect.SetActive(true);

        yield return new WaitForSeconds(2.0f);
        skillEffect.SetActive(false);
    }
}