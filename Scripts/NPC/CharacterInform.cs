using UnityEngine;

public class CharacterInform : MonoBehaviour
{
    /* 캐릭터 정보 */

    [SerializeField]
    private int id;

    [SerializeField]
    private int questID;

    private BaseCharacter myCharacter;

    /* 필요한 기능들 */

    private UI_Dialog dialogUI;

    private bool canTalk;

    [SerializeField]
    private bool reverse;

    private float leftSide;     // Defualt 1
    private float rightSide;    // -1

    public BaseCharacter MyCharacter { get => myCharacter; private set => myCharacter = value; }
    public int QuestID { get => questID; set => questID = value; }

    private void Start()
    {
        Managers.Data.CharDic.TryGetValue(id, out myCharacter);
        canTalk = false;

        if (transform.localScale.x > 0)
        {
            leftSide = transform.localScale.x;
            rightSide = -transform.localScale.x;
        }
        else
        {
            leftSide = -transform.localScale.x;
            rightSide = transform.localScale.x;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canTalk = true;
            Managers.Resource.Instantiate("Util/E", collision.gameObject.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canTalk = false;
            GuideEKey guide = collision.GetComponentInChildren<GuideEKey>();
            if (guide != null)
            {
                Destroy(guide.gameObject);
            }
        }
    }

    private void Update()
    {
        if (dialogUI == null)
        {
            dialogUI = UI_Dialog.MyInstance;
            return;
        }
        if (dialogUI.IsAction)
            return;

        if (canTalk)
        {
            GameObject player = FindObjectOfType<PlayerMain>().gameObject;
            if (player != null)
            {
                float dir = gameObject.transform.position.x - player.transform.position.x > 0 ? leftSide : rightSide;
                if (reverse)
                    dir = -dir;
                gameObject.transform.localScale = new Vector3(dir, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            }
        }
        if (canTalk && Input.GetKeyDown(KeyCode.E))
        {
            dialogUI.Action(MyCharacter.id, QuestID);
        }
    }
}