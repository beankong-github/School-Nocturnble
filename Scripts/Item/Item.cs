using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public BaseItem itemData = new BaseItem();

    [SerializeField]
    protected int id;         // 아이템 아이디

    [SerializeField]
    protected Sprite icon;    // 아이템 아이콘

    [SerializeField]
    protected int stackSize;  // 아이템의 개수

    public int Id { get => id; set => id = value; }
    public Sprite Icon { get => icon; set => icon = value; }
    public BaseItem ItemData { get => itemData; set => itemData = value; }
    public int StackSize { get => stackSize; set => stackSize = value; }

    protected abstract void InitItem();

    public abstract bool UseItem();
}