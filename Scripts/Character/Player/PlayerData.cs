using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Scriptable Object/Player Data", order = int.MaxValue)]
public class PlayerData : ScriptableObject
{
    public static Vector3 savedPosition;

    public static bool isSubPlayerExist = false;
    public static bool isClear_sildepuzzle = false;
    public static bool isDark = false;
    public static bool isSafeUnlock = false;
    public static bool isListDone = false;
    public static bool isClearCori = false;
}