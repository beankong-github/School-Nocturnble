using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, BaseCharacter> CharDic { get; private set; } = new Dictionary<int, BaseCharacter>();
    public Dictionary<int, BaseItem> ItemDic { get; private set; } = new Dictionary<int, BaseItem>();
    public Dictionary<int, BaseObject> ObjectDic { get; private set; } = new Dictionary<int, BaseObject>();
    public Dictionary<int, BaseTalk> TalkDic { get; private set; } = new Dictionary<int, BaseTalk>();
    public Dictionary<int, BaseQuest> QuestDic { get; private set; } = new Dictionary<int, BaseQuest>();
    public Dictionary<int, BaseSkill> SkillDic { get; private set; } = new Dictionary<int, BaseSkill>();

    public void Init()
    {
        CharDic = LoadJson<CharacterData, int, BaseCharacter>("CharacterData").MakeDict();
        ObjectDic = LoadJson<ObjectData, int, BaseObject>("ObjectData").MakeDict();
        ItemDic = LoadJson<ItemData, int, BaseItem>("ItemData").MakeDict();
        TalkDic = LoadJson<TalkData, int, BaseTalk>("TalkData").MakeDict();
        QuestDic = LoadJson<QuestData, int, BaseQuest>("QuestData").MakeDict();
        SkillDic = LoadJson<SkillData, int, BaseSkill>("SkillData").MakeDict();
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}