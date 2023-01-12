using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : BaseScene    // 씬 전환과 페이드 인/아웃 관리
{
    /* 캐릭터 관리*/
    public GameObject Player;
    private GameObject subPlayer;

    public delegate void generate();

    public static event generate PlayerGenerated;// 캐릭터 생성을 알리는 event

    /* 카메라 관리 */
    public GameObject Camera;

    /* UI 관리*/
    private static GameObject UI_Root;
    public GameObject Exit;

    /* 씬 관리 */

    public static event System.Action StartChangeScene; // 씬이 바뀔 타이밍을 알리는 이벤트

    public Define.SceneName nextScene;
    public Define.SceneName sceneName = Define.SceneName.Unknown;
    public Define.SceneType sceneType;

    public float WaitBeforeSceneChanged;    // 씬이 바뀌기 전까지 기다리는 타이밍

    /* 오디오 관리 */
    public AudioSource BackGroundAudio;     // 배경 음악 오디오

    /* 페이드 관리 */
    public float FadeInTime;        // 페이드 인 시간
    public float FadeOutTime;       // 페이드 아웃 시간
    private FadeController _Fade;   // Fade

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    private void Start()
    {
        Init();

        // 페이드
        _Fade.FadeIn(FadeInTime);

        // 오디오 Don't Destroy
        if (BackGroundAudio != null)
            DontDestroyOnLoad(BackGroundAudio);
    }

    protected override void Init()
    {
        // 페이드 가져오기
        _Fade = this.GetComponentInChildren<FadeController>();
        if (_Fade == null)
        {
            _Fade = Managers.Resource.Instantiate("Util/Fade").GetComponent<FadeController>();
        }

        // 커서 생성
        GameObject Cursor = GameObject.Find("Cursor");
        if (Cursor == null)
        {
            Cursor = Managers.Resource.Instantiate("Util/Cursor");
        }

        // 플레이어 탐색
        Player = GameObject.FindGameObjectWithTag("Player");
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        subPlayer = GameObject.FindGameObjectWithTag("SubPlayer");

        // UI 생성
        Exit = GameObject.Instantiate(Exit);

        // EventSystem
        base.Init();

        switch (sceneType)
        {
            case (Define.SceneType.Etc):
                ETCSceneInit();
                break;

            case (Define.SceneType.Game):
                GameSceneInit();
                break;

            case (Define.SceneType.Puzzle):
                PuzzleSceneInit();
                break;

            case (Define.SceneType.Battle):
                BattleSceneInit();
                break;
        }
    }

    // 게임 씬일 경우 초기화
    private void GameSceneInit()
    {
        // UI 생성
        UI_Root = GameObject.FindGameObjectWithTag("UI_Root");
        if (UI_Root == null)
        {
            UI_Root = Managers.Resource.Instantiate("Util/@UI_Root");
        }
        else
        {
            for (int i = 0; i < UI_Root.transform.childCount; i++)
            {
                UI_Root.transform.GetChild(i).gameObject.SetActive(true);
            }
            UI_Root.transform.Find("Dialog").gameObject.SetActive(false);
        }
        DontDestroyOnLoad(UI_Root);

        // Player 생성
        if (Player == null)
        {
            Player = Managers.Resource.Instantiate("Chara/Player");
        }
        Player.transform.position = PlayerData.savedPosition;
        DontDestroyOnLoad(Player);

        // 보조 캐릭터 생성
        if (PlayerData.isSubPlayerExist)
        {
            if (subPlayer == null)
            {
                subPlayer = Managers.Resource.Instantiate("Chara/SubPlayer");
            }
        }

        // Camera
        if (Camera == null)
        {
            Camera = Managers.Resource.Instantiate("Camera/Main Camera");
        }
        Camera.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + 3, Camera.transform.position.z);
        Camera.GetComponent<CameraController>()._target = Player;

        if (PlayerGenerated != null)
            PlayerGenerated();
    }

    // 기타 씬일 경우 초기화
    private void ETCSceneInit()
    {
        // 플레이어 삭제
        if (Player != null)
            Destroy(Player);

        // 플레이어 삭제
        if (Player != null)
        {
            Destroy(Player);
        }
        // 보조 캐릭터 삭제
        if (subPlayer != null)
            Destroy(subPlayer);

        if (UI_Root != null)
        {
            for (int i = 0; i < UI_Root.transform.childCount; i++)
            {
                UI_Root.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    // 퍼즐 씬일 경우 초기화
    private void PuzzleSceneInit()
    {
        // 플레이어 삭제
        if (Player != null)
        {
            Destroy(Player);
        }
        // 보조 캐릭터 삭제
        if (subPlayer != null)
            Destroy(subPlayer);

        if (UI_Root != null)
        {
            for (int i = 0; i < UI_Root.transform.childCount; i++)
            {
                UI_Root.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        // 카메라 새로 탐색
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // 전투 씬일 경우 초기화
    private void BattleSceneInit()
    {
        // 플레이어 삭제
        if (Player != null)
            Destroy(Player);
        // 보조 캐릭터 삭제
        if (subPlayer != null)
            Destroy(subPlayer);

        if (UI_Root != null)
        {
            for (int i = 0; i < UI_Root.transform.childCount; i++)
            {
                UI_Root.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        // 카메라 새로 탐색
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void TimeChangedScene()
    {
        if (Player != null)
            PlayerData.savedPosition = Player.transform.position;

        // 페이드 아웃
        _Fade.FadeOut(FadeOutTime, StartChangeScene);
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable");
        StartChangeScene += ChangeScene;
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    // 씬을 바꿉니다.
    private void ChangeScene()
    {
        SceneManager.LoadScene(nextScene.ToString());

        Scene scene = SceneManager.GetSceneByName(nextScene.ToString());
    }

    private void OnDisable()
    {
        Debug.Log("Disable");
        StartChangeScene -= ChangeScene;
        SceneManager.activeSceneChanged -= ChangedActiveScene;
    }

    // 씬이 전환된다는 이벤트가 발생하면 Log를 띄워줍니다.
    private void ChangedActiveScene(Scene current, Scene next)
    {
        string currentName = sceneName.ToString();

        if (currentName == null)
        {
            // 씬이 사라졌거나 씬 이름이 제대로 등록되지 않은 경우 실행
            currentName = Define.SceneName.Unknown.ToString();
        }
    }

    public override void Clear()
    {
    }
}