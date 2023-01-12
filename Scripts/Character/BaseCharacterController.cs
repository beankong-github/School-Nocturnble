using UnityEngine;
using System.Collections;

// 주인공 캐릭터를 포함한 모든 인간 캐릭터에게 적용
public class BaseCharacterController : MonoBehaviour
{
    // == 외부 파라미터 표시(Inspector에 표시)===================

    // == 외부 파라미터 ========================================

    [SerializeField]
    private float hpValue;

    [SerializeField]
    private float mpValue;

    [SerializeField]
    private float hpMAX;

    [SerializeField]
    private float mpMAX;

    public float speed = 3.0f;
    public float runSpeed = 2.0f;
    public LayerMask layerMask; // 충돌처리- 통과 불과 layer

    [System.NonSerialized] public float dir = 1.0f;
    [System.NonSerialized] public Vector3 movDir = new Vector3(1.0f, 0.0f, 0.0f); // npc가 움직일 방향 설정
    [System.NonSerialized] public float applyRunSpeed = 0.0f;
    [System.NonSerialized] public float basScaleX;                                // 캐릭터의 기본 Scale.x 값
    [System.NonSerialized] public bool activeSts = false;
    [System.NonSerialized] public bool jumped = false;
    [System.NonSerialized] public bool grounded = false;
    [System.NonSerialized] public bool groundedPrev = false;
    [System.NonSerialized] public GameObject curItem = null;

    // == 캐쉬 ================================================

    [System.NonSerialized] public Animator animator;
    protected Transform groundCheck_L;
    protected Transform groundCheck_C;
    protected Transform groundCheck_R;

    // == 내부 파라미터 ========================================

    protected float gravityScale;
    protected float jumpStartTime = 0.0f;
    protected GameObject groundCheck_OnRoadObject;
    protected GameObject groundCheck_OnMoveObject;

    // == 코드(Movobehaviour 기본 기능 구현) ===================
    protected virtual void Awake()
    {
        //animator = GetComponent<Animator>();
        groundCheck_L = transform.Find("GroundCheck_L");
        groundCheck_C = transform.Find("GroundCheck_C");
        groundCheck_R = transform.Find("GroundCheck_R");

        // dir에 따른 캐릭터 방향 설정(최초)
        dir = (transform.localScale.x > 0.0f) ? 1.0f : -1.0f;
        basScaleX = transform.localScale.x * dir;
        transform.localScale = new Vector3(basScaleX, transform.localScale.y, transform.localScale.z);

        activeSts = true;
        gravityScale = GetComponent<Rigidbody2D>().gravityScale;
    }

    protected virtual void Update()
    {
        // 낙하 체크
        if (transform.position.y < -30.0f)
        {
            // 참고) 구멍에 빠져 죽었을 땐 gameOver false, 적에게 죽었을 땐 gameOver true
            Dead(false);
        }

        // 체력 체크
        if (hpValue <= 0)
        {
            Dead(true);
        }

        // 지면 체크
        groundedPrev = grounded;
        grounded = false;

        groundCheck_OnRoadObject = null;
        groundCheck_OnMoveObject = null;

        Collider2D[][] groundCheckCollider = new Collider2D[3][];
        groundCheckCollider[0] = Physics2D.OverlapPointAll(groundCheck_L.position);
        groundCheckCollider[1] = Physics2D.OverlapPointAll(groundCheck_C.position);
        groundCheckCollider[2] = Physics2D.OverlapPointAll(groundCheck_R.position);

        foreach (Collider2D[] groundCheckList in groundCheckCollider)
        {
            foreach (Collider2D groundCheck in groundCheckList)
            {
                if (!groundCheck.isTrigger)
                {
                    grounded = true;
                    if (groundCheck.tag == "Road")
                    {
                        groundCheck_OnRoadObject = groundCheck.gameObject;
                    }
                    else if (groundCheck.tag == "MoveObject")
                    {
                        groundCheck_OnMoveObject = groundCheck.gameObject;
                    }
                }
            }
        }

        // 캐릭터 개별 처리할 동작
        UpdateCharacter();
    }

    protected virtual void UpdateCharacter()
    {
    }

    // == 코드 (기본 액션) ======================================
    // 캐릭터 좌우 이동
    public virtual void MoveChar(float direction)
    {
    }

    // == 코드 (기타) ============================================
    public virtual void Dead(bool gameOver)
    {
        if (!activeSts)
        {
            return;
        }

        activeSts = false;
        //animator.SetTrigger("Dead");
        Debug.Log("플레이어 사망");
    }

    public void SetHP(float _hp, float _hpMax = 0)
    {
        hpValue = _hp > hpMAX ? hpMAX : _hp;
        if (hpValue < 0) hpValue = 0;

        if (_hpMax != 0)
            hpMAX = _hpMax;
    }

    public float GetHP()
    {
        return hpValue;
    }

    public float GetMaxHP()
    {
        return hpMAX;
    }

    public void SetMP(float _mp, float _mpMax = 0)
    {
        mpValue = _mp > mpMAX ? mpMAX : _mp;
        if (mpValue < 0) mpValue = 0;

        if (_mpMax != 0)
            mpMAX = _mpMax;
    }

    public float GetMP()
    {
        return mpValue;
    }

    public float GetMaxMP()
    {
        return mpMAX;
    }
}