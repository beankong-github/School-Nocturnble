using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 전용 파라미터나 동작을 구현하는 파생 클래스
public class PlayerController : BaseCharacterController
{
    // == 전역 변수 =======================
    private UnitStat hp;

    private UnitStat mp;

    [SerializeField]
    private float InitMaxHp;

    [SerializeField]
    private float InitMaxMp;

    public float jump = 14.0f;
    public float doubleJump = 12.0f;

    public Define.SceneName _prevMapName;    // transferMap 스크립트에 있는 transferMapName의 변수의 값 저장
    public Animator playerAnimator;

    private int _jumpCount = 0;

    public UnitStat Hp { get => hp; set => hp = value; }
    public UnitStat Mp { get => mp; set => mp = value; }

    // == 코드 (Monobehaviour 기본 기능 구현) ===================
    private void OnEnable()
    {
        if (UI_Dialog.MyInstance == null)
        {
            Invoke("OnEnable", 0.3f);
            return;
        }
        UI_Dialog.MyInstance.DialogActiveEvent += OffPlayer;
        UI_Dialog.MyInstance.DialogDeactiveEvent += OnPlayer;
    }

    private void OnDisable()
    {
        UI_Dialog.MyInstance.DialogActiveEvent -= OffPlayer;
        UI_Dialog.MyInstance.DialogDeactiveEvent -= OnPlayer;
    }

    protected override void Awake()
    {
        base.Awake();

        // 플레이어 상태바와 연결
        Hp = GameObject.Find("PlayerHP").GetComponent<UnitStat>();
        Mp = GameObject.Find("PlayerMP").GetComponent<UnitStat>();

        // 파라미터 초기화
        Hp.Initialized(InitMaxHp, InitMaxHp);
        Mp.Initialized(InitMaxMp, InitMaxMp);

        SetHP(Hp.unitMaxValue, Hp.unitMaxValue);
        SetMP(Mp.unitMaxValue, Mp.unitMaxValue);
    }

    protected override void UpdateCharacter()
    {
        // 점프 후 착지 검사
        if (jumped)
        {
            // 이전에 점프 후 땅에 있으면 또는 점프한지 1초 이후에 땅에 있으면 착지한 것으로 간주
            if ((grounded && !groundedPrev) || (grounded && Time.fixedTime > jumpStartTime + 1.0f))
            {
                //animator.SetTrigger("Idle");
                jumped = false;
                _jumpCount = 0;
            }
        }
        if (!jumped)
        {
            _jumpCount = 0;
        }

        // 공중에 있을 때
        if (jumped && !grounded)
        {
        }

        // HP && MP Update
        if (Hp == null || Mp == null)
        {
            Hp = GameObject.Find("PlayerHP").GetComponent<UnitStat>();
            Mp = GameObject.Find("PlayerMP").GetComponent<UnitStat>();
        }
        if (Hp != null && Hp.unitCurrentValue != GetHP())
        {
            Hp.unitCurrentValue = GetHP();
            Debug.Log($"Player의 현재 HP : {Hp.unitCurrentValue}");
        }
        if (Mp != null && Mp.unitCurrentValue != GetMP())
        {
            Mp.unitCurrentValue = GetMP();
            Debug.Log($"Player의 현재 MP : {Mp.unitCurrentValue}");
        }
    }

    // == 코드 (기본 액션) ===================================

    private void OnPlayer()
    {
        activeSts = true;
    }

    private void OffPlayer()
    {
        playerAnimator.SetBool("PlayerMove", false);
        activeSts = false;
    }

    public void CheckRun(bool isRun)
    {
        if (isRun)
            applyRunSpeed = runSpeed;
        else
            applyRunSpeed = 0.0f;
    }

    public override void MoveChar(float direction)
    {
        dir = direction;
        movDir = new Vector2(dir, 0.0f);    // 캐릭터가 이동할 방향
        transform.localScale = new Vector3(basScaleX * dir, transform.localScale.y, transform.localScale.z);

        // 애니메이션 할당

        // 캐릭터 이동 불가 지역 설정
        RaycastHit2D _hitInfo;
        Vector2 _startRay = transform.position;      // 캐릭터의 현재 위치
        Vector2 _endRay = _startRay + new Vector2(movDir.x * speed, movDir.y * speed);      // 캐릭터가 이동하고자 하는 위치

        //boxCollider2D.enabled = false;
        _hitInfo = Physics2D.Raycast(_startRay, movDir, 1.0f, layerMask);
        //Debug.DrawRay(_startRay, movDir, Color.blue, 0.1f);
        //boxCollider2D.enabled = true;

        if (_hitInfo.transform != null)
            return;

        transform.position += movDir * (speed + applyRunSpeed) * Time.deltaTime;
        playerAnimator.SetBool("PlayerMove", true);
    }

    public void PlayerIdle()
    {
        playerAnimator.SetBool("PlayerMove", false);
    }

    public void ActionJump()
    {
        switch (_jumpCount)
        {
            case 0:
                if (grounded)
                {
                    //animator.SetTrigger("Jump");
                    GetComponent<Rigidbody2D>().velocity = Vector2.up * jump;
                    jumpStartTime = Time.fixedTime;
                    jumped = true;
                    _jumpCount++;
                }
                break;

            case 1:
                if (!grounded)
                {
                    //animator.Play("Player_Jump", 0, 0.0f);
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, doubleJump);
                    jumped = true;
                    _jumpCount++;
                }
                break;
        }
    }
}