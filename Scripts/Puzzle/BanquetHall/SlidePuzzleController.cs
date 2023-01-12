using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePuzzleController : MonoBehaviour
{
    // == 전역 변수 ====================================

    public SceneController scene;
    public Define.SceneName nextScene;

    public GameObject succesParticle;
    public GameObject succesMessage;

    private enum STATE
    { wait, idle, click, move, calc, finish, cancel }

    private STATE state = STATE.wait;

    private int sliceCnt = 3;

    private class TileSize
    {
        public float w;
        public float h;
    }

    private TileSize tileScale = new TileSize();    // 완성 이미지 크기에 대한 각 타일의 크기 비율
    private TileSize tileSpan = new TileSize();     // 타일 간격

    private List<Sprite> sprites = new List<Sprite>();      // 각 타일들을 저장할 List
    private List<Transform> tiles = new List<Transform>();  // 각 타일들의 위치를 저장할 List

    private List<int> orders = new List<int>();             // 각각의 타일 번호를 저장하는 List
    private List<int> moveTiles = new List<int>();          // 이동해야할 타일 번호

    private int dir;            // 타일 이동 방향 1: 위, 2: 오른쪽, 3: 아래, 4: 왼쪽
    private int tileNum;        // Click한 타일 번호
    private bool canCalc;        // 타일 이동 후 orders의 타일 번호를 갱신해야 하는지 여부

    // Texture 자르기 <- Init
    private void SpriteTexture()
    {
        // 완성 타일 Texture
        Texture2D org = Resources.Load("Sprites/GameScene/Remental/BanquetHall/Puzzle/Map", typeof(Texture2D)) as Texture2D;

        // Texture의 크기
        tileScale.w = (float)org.width;
        tileScale.h = (float)org.height;

        // 자를 조각의 크기
        float w = tileScale.w / sliceCnt;
        float h = tileScale.h / sliceCnt;

        // Texture 를 위에서부터 자르기
        sprites.Clear();
        for (int y = sliceCnt - 1; y >= 0; y--)
        {
            for (int x = 0; x < sliceCnt; x++)
            {
                Rect rect = new Rect(x * w, y * h, w, h);    // 잘라낼 이미지의 크기와 기준점
                Vector2 pivot = new Vector2(0, 1);           // 잘라낸 이미지의 피봇(왼쪽 위를 기준점으로 설정)

                Sprite sprite = Sprite.Create(org, rect, pivot);    // Texture를 잘라서 새로운 Sprite 만들기, 매개 변수의 순서는 텍스처, 크기, Pivot
                sprites.Add(sprite);    // 잘라낸 sprite 저장
            }
        }
    }

    // Make Tile <- Init
    private void MakeTiles()
    {
        // 타일, 순서 배열 초기화
        tiles.Clear();
        orders.Clear();

        Vector2 size = sprites[0].bounds.size;          // 화면에 출력되는 Sprite의 크기. 미터(m)로 환산된 값이며, 이 값에 tileScale을 곱하면 원본 이미지를 자른 것과 같은 크기가 됨. 참고로 픽셀 단위의 크기는 Sprite.rect.width, Sprite.rect.height()임
        int n = 0;                                      // 타일 시작 번호

        for (int y = 0; y < sliceCnt; y++)
        {
            for (int x = 0; x < sliceCnt; x++)
            {
                MakeSingleTile(n, size);
                orders.Add(n++);                        // List에 일련번호 넣기
            }
        }

        // 마지막 타일 (공란)
        orders[orders.Count - 1] = -1;                          // 마지막 타일의 번호는 -1
        tiles[orders.Count - 1].gameObject.SetActive(false);    // 마지막 타일은 비활성화
    }

    // Make SingleTile <- MakeTile
    private void MakeSingleTile(int idx, Vector2 size)
    {
        GameObject tile = Instantiate(Resources.Load("Prefabs/Puzzle/BanquetHall/Tile")) as GameObject;    // 프리랩 Tile을 새로운 GO로 생성
        tile.transform.localScale = new Vector3(1, 1, 1);   // 타일의 Scale 설정. 이미지가 작은 타일은 확대하고 큰 타일은 축소해서 기준 이미지의 크기와 일치시킴

        // Tile에 분할한 Sprite 입히기
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        renderer.sprite = sprites[idx];

        renderer.material.SetInt("_count", sliceCnt);   // Shader 속성 설정
        tile.name = "Tile" + idx;

        // BoxCollider2D
        BoxCollider2D collider = tile.GetComponent<BoxCollider2D>();
        collider.size = size;
        collider.offset = new Vector2(size.x / 2, -size.y / 2);

        //tile 저장
        tiles.Add(tile.transform);
    }

    // Init Game
    private void InitGame()
    {
        SpriteTexture();
        MakeTiles();
    }

    private void DrawTiles()
    {
        state = STATE.wait;

        Transform parent = new GameObject("Tiles").transform;

        // 타일 간격 구하기
        Sprite sprite = tiles[0].GetComponent<SpriteRenderer>().sprite;
        tileSpan.w = sprite.bounds.size.x;    // 가로
        tileSpan.h = sprite.bounds.size.y;    // 세로

        for (int y = 0; y < sliceCnt; y++)
        {
            for (int x = 0; x < sliceCnt; x++)
            {
                int idx = y * sliceCnt + x;     // 2차원 좌표를 1차원으로 변환

                // 타일의 인덱스
                int n = orders[idx];
                if (n == -1)
                {
                    n = orders.Count - 1;   // 만약 곤란 타일이면 마지막 타일 번호로 설정(마지막 타일은 비활성화 상태)
                }

                Vector3 pos = new Vector3(x * tileSpan.w, -y * tileSpan.h, 0);  // 타일 위치 계산
                tiles[n].position = pos;
                tiles[n].parent = parent;
            }
        }

        state = STATE.idle;
    }

    // ShufflTile <- Awake
    private void ShufflTile()
    {
        for (int i = 0; i < orders.Count - 1; i++)
        {
            int n = Random.Range(i + 1, orders.Count);      // 현재 번호보다 큰 난수 생성하기
            int tmp = orders[i];                            // 현재 값과 난수가 가리키는 위치의 값을 서로 교환
            orders[i] = orders[n];
            orders[n] = tmp;
        }

        if (!CheckValidate())
            ShufflTile();
    }

    // 무결성 조사 <-  ShufflTile
    private bool CheckValidate()
    {
        int sum = 0;
        for (int i = 0; i < orders.Count; i++)
        {
            if (orders[i] == -1)
                continue;

            for (int j = 1; j + i < orders.Count; j++)
            {
                if (orders[j] != -1 && orders[i] > orders[j + i]) sum++;
            }
        }

        return (sum % 2 == 0);
    }

    // SetClick <- Tile
    private void SetClick(int _tileNum)
    {
        if (state == STATE.idle)
        {
            tileNum = _tileNum;
            state = STATE.click;
        }

        Debug.Log("TileNunm = " + tileNum);
    }

    // Check Tile <- Update
    private void CheckTiles()
    {
        state = STATE.wait;

        // 이동 방향과 이동할 타일 리스트 초기화
        dir = 0;
        moveTiles.Clear();

        // 클릭한 타일과 공백 위치 찾기
        int tile = orders.FindIndex(x => x == tileNum);
        int blank = orders.FindIndex(x => x == -1);

        // 좌표 계산 (1차원 좌표를 2차원 좌표로 변환)
        int x1 = tile % sliceCnt;
        int y1 = tile / sliceCnt;

        int x2 = blank % sliceCnt;
        int y2 = blank / sliceCnt;

        // 세로 방향 조사
        if (x1 == x2)
        {
            // 공백 번호 저장
            moveTiles.Add(blank);

            // 이동 방향과 행 간격
            dir = (y1 > y2) ? 1 : 3;
            int row = (y1 > y2) ? sliceCnt : -sliceCnt;
            int idx = blank + row;  // 공백과 인접한 타일 번호 저장

            while (true)
            {
                moveTiles.Add(idx);     // 타일 번호 저장
                idx += row;
                if ((dir == 1 && idx > tile) || (dir == 3 && idx < tile)) break;
            }
        }

        // 가로 방향 조사
        else if (y1 == y2)
        {
            moveTiles.Add(blank);

            // 이동 방향과 열간격
            dir = (x1 > x2) ? 4 : 2;
            int col = (x1 > x2) ? 1 : -1;
            int idx = blank + col;

            while (true)
            {
                moveTiles.Add(idx);
                idx += col;
                if ((dir == 2 && idx < tile) || (dir == 4 && idx > tile)) break;
            }
        }

        // 이동할 타일이 있으면 move 상태로 전환
        state = (moveTiles.Count > 0) ? STATE.move : STATE.idle;
    }

    // SetCalc <- Tile
    private void SetCalc()
    {
        state = STATE.calc;
    }

    private void MoveTiles()
    {
        Vector3[] vectors = { Vector3.zero, Vector3.up, Vector3.right, Vector3.down, Vector3.left };

        foreach (int idx in moveTiles)
        {
            int p = orders[idx];
            if (p == -1) continue;

            Vector3 pos = tiles[p].position;
            Vector3 target;

            if (dir == 1 || dir == 3) target = pos + (vectors[dir] * tileSpan.h);
            else if (dir == 2 || dir == 4) target = pos + (vectors[dir] * tileSpan.w);
            else continue;

            tiles[p].SendMessage("SetMove", target);
        }
        state = STATE.wait;
        canCalc = true;
    }

    private void CalcOrder()
    {
        if (!canCalc)   // 색인 정렬할 필요가 았는가?
        {
            state = STATE.idle;
            return;
        }

        canCalc = false;        // 색인 중복 정렬 방지
        state = STATE.wait;

        //이동한 타일 목록 전체 조사
        for (int i = 0; i < moveTiles.Count - 1; i++)
        {
            int n1 = moveTiles[i];
            int n2 = moveTiles[i + 1];
            orders[n1] = orders[n2];    // 색인을 1칸씩 이동
        }

        // 공백 이동
        int blank = moveTiles[moveTiles.Count - 1];
        orders[blank] = -1;

        // 정리 완료인지 조사
        bool finished = true;
        for (int i = 0; i < orders.Count - 1; i++)
        {
            if (orders[i] != i)
            {
                finished = false;
                break;
            }
        }

        if (finished)
        {
            state = STATE.finish;
        }
        else
        {
            state = STATE.idle;
        }
    }

    private void SetFInished()
    {
        foreach (Transform tile in tiles)
        {
            // 각 타일의 테두리 제거
            tile.GetComponent<SpriteRenderer>().material.SetInt("_count", 0);
        }

        // 마지막 타일
        int last = orders.Count - 1;
        tiles[last].gameObject.SetActive(true);
        tiles[last].position = tiles[last = 1].position + (Vector3.right * tileSpan.w) + (Vector3.down * tileSpan.h * 2);

        // Effect
        succesMessage.SetActive(true);
        succesParticle.SetActive(true);

        //데이터 완료
        PlayerData.isClear_sildepuzzle = true;

        // 커서 모드 변경
        CursorController.MyInstance.isGameObjectMode = false;
        CursorController.MyInstance.CurCursorMode = CursorController.CursorMode.Mouse;

        // Scene 전환
        scene.nextScene = nextScene;
        StartCoroutine("SceneChangeAfterSuccess");
    }

    private IEnumerator SceneChangeAfterSuccess()
    {
        yield return new WaitForSeconds(1.0f);

        CursorController.MyInstance.isGameObjectMode = false;
        scene.TimeChangedScene();
    }

    private void Awake()
    {
        InitGame();
        ShufflTile();
        DrawTiles();
    }

    private void Update()
    {
        switch (state)
        {
            case STATE.click:
                CheckTiles();
                break;

            case STATE.move:
                MoveTiles();
                break;

            case STATE.calc:
                CalcOrder();
                break;

            case STATE.finish:
                SetFInished();
                break;
        }
    }
}