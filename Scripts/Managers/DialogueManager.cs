using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager
{
    public string GetTalk(int _id, int _talkIndex)
    {
        if (!Managers.Data.TalkDic.ContainsKey(_id))
        {
            if (Managers.Data.TalkDic.ContainsKey(_id - _id % 10))
            {   // 해당 퀘스트 맨 처음 대사마저 없을 때,
                // 기본 대사를 가지고 온다.
                return GetTalk(_id - _id % 100, _talkIndex);
            }
            else
            {
                // 해당 퀘스트 진행 순서 대사가 없을 때
                // 퀘스트 맨 처음 대사를 가지고 온다.
                return GetTalk(_id - _id % 10, _talkIndex);
            }
        }

        string[] talkData = Managers.Data.TalkDic[_id].sentences;

        if (_talkIndex >= talkData.Length)
        {
            return null;
        }

        if (_talkIndex == -1)
        {
            _talkIndex = Random.Range(0, talkData.Length);
        }

        string talk = talkData[_talkIndex];
        return talk;
    }
}