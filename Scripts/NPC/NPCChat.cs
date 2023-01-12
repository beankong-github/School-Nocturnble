using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCChat : MonoBehaviour
{
    public string[] senteces;    // 대사 저장용 배열
    public Transform chatTr;     // 말풍선 생성 위치
    public float TimeForOneSentence;    // 한 대사가 나오는데 사용하는 시간
    public float TimeForHoleSentences;   // 전체 대사가 나오는데 사용하는 시간

    private GameObject chatBox;   // 말풍선

    private void OnDisable()
    {
        UI_Dialog.MyInstance.DialogActiveEvent -= HideChatbox;
        UI_Dialog.MyInstance.DialogDeactiveEvent -= TalkNPC;
    }

    private void OnEnable()
    {
        if (UI_Dialog.MyInstance == null)
        {
            Invoke("OnEnable", 0.5f);
            return;
        }
        UI_Dialog.MyInstance.DialogActiveEvent += HideChatbox;
        UI_Dialog.MyInstance.DialogDeactiveEvent += TalkNPC;
    }

    private void Start()
    {
        TalkNPC();
    }

    public void TalkNPC()
    {
        if (chatBox != null)
            return;

        chatBox = Managers.Resource.Instantiate("Util/ChatBox");
        chatBox.GetComponent<ChatSystem>().Ondialogue(senteces, chatTr, TimeForOneSentence);
        Invoke("TalkNPC", TimeForHoleSentences);
    }

    private void HideChatbox()
    {
        Destroy(chatBox);
    }

    private void Update()
    {
        if (chatBox == null)
            return;

        chatBox.transform.position = chatTr.position;
    }
}