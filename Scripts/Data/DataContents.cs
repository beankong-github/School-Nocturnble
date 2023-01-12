using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Characters

[Serializable]
public class BaseCharacter
{
    public int id;
    public string name;
    public string portraitName;
    public int maxHp;
    public int maxMp;
    public int minAttack;
    public int maxAttack;
}

[Serializable]
public class CharacterData : ILoader<int, BaseCharacter>
{
    public List<BaseCharacter> characters = new List<BaseCharacter>();

    public Dictionary<int, BaseCharacter> MakeDict()
    {
        Dictionary<int, BaseCharacter> dict = new Dictionary<int, BaseCharacter>();

        foreach (BaseCharacter chara in characters)
        {
            dict.Add(chara.id, chara);
        }
        return dict;
    }
}

#endregion Characters

#region Objects

[Serializable]
public class BaseObject
{
    public int id;
    public string objectName;
}

[Serializable]
public class ObjectData : ILoader<int, BaseObject>
{
    public List<BaseObject> objects = new List<BaseObject>();

    public Dictionary<int, BaseObject> MakeDict()
    {
        Dictionary<int, BaseObject> dict = new Dictionary<int, BaseObject>();

        foreach (BaseObject interaction in objects)
        {
            dict.Add(interaction.id, interaction);
        }
        return dict;
    }
}

#endregion Objects

#region Items

[Serializable]
public class BaseItem
{
    public int id;
    public string name;
    public string information;
    public bool isUsable;
}

[Serializable]
public class ItemData : ILoader<int, BaseItem>
{
    public List<BaseItem> items = new List<BaseItem>();

    public Dictionary<int, BaseItem> MakeDict()
    {
        Dictionary<int, BaseItem> dict = new Dictionary<int, BaseItem>();

        foreach (BaseItem item in items)
        {
            dict.Add(item.id, item);
        }
        return dict;
    }
}

#endregion Items

#region Talks

[Serializable]
public class BaseTalk
{
    public int id;
    public bool isNPC;
    public string[] sentences;
    public bool isRandom;
}

[Serializable]
public class TalkData : ILoader<int, BaseTalk>
{
    public List<BaseTalk> talks = new List<BaseTalk>();

    public Dictionary<int, BaseTalk> MakeDict()
    {
        Dictionary<int, BaseTalk> dict = new Dictionary<int, BaseTalk>();

        foreach (BaseTalk talk in talks)
        {
            dict.Add(talk.id, talk);
        }
        return dict;
    }
}

#endregion Talks

#region Quests

[Serializable]
public class BaseQuest
{
    public int id;
    public string name;
    public string[] goals;
    public string contents;
    public int[] npcID;
    public int questActionIndex = 0;
}

[Serializable]
public class QuestData : ILoader<int, BaseQuest>
{
    public List<BaseQuest> quests = new List<BaseQuest>();

    public Dictionary<int, BaseQuest> MakeDict()
    {
        Dictionary<int, BaseQuest> dict = new Dictionary<int, BaseQuest>();

        foreach (BaseQuest quest in quests)
        {
            dict.Add(quest.id, quest);
        }
        return dict;
    }
}

#endregion Quests

#region Skills

[Serializable]
public class BaseSkill
{
    public int id;
    public string name;
    public string description;
    public int damage;
    public int mana;
    public int heal;
    public int strength;
    public bool isAcquired;
}

[Serializable]
public class SkillData : ILoader<int, BaseSkill>
{
    public List<BaseSkill> skills = new List<BaseSkill>();

    public Dictionary<int, BaseSkill> MakeDict()
    {
        Dictionary<int, BaseSkill> dict = new Dictionary<int, BaseSkill>();

        foreach (BaseSkill skill in skills)
        {
            dict.Add(skill.id, skill);
        }

        return dict;
    }
}

#endregion Skills