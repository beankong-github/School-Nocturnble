using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;

    public int damage;

    public int maxHP;
    public int currentHP;

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public bool Heal(int value)
    {
        if (currentHP <= 0)
            return false;
        else if (currentHP >= maxHP)
            return false;
        else
            currentHP += value;

        return true;
    }
}