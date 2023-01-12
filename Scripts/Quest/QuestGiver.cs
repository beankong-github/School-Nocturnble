using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField]
    private int questId;

    [SerializeField]
    private int speakerId;

    private UI_Dialog dialogUI;

    public int QuestId { get => questId; set => questId = value; }
    public int SpeakerId { get => speakerId; set => speakerId = value; }

    private void OnTriggerStay2D(Collider2D collision)
    {
        dialogUI = FindObjectOfType<UI_Dialog>();
        if (dialogUI == null)
        {
            dialogUI = UI_Dialog.MyInstance;
        }

        if (dialogUI.IsAction)
            return;

        if (Input.anyKey)
        {
            dialogUI.Action(SpeakerId, QuestId);
            Destroy(this.gameObject);
        }
    }
}