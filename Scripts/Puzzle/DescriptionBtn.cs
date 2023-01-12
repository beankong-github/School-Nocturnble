using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionBtn : MonoBehaviour
{
    public void ClickBtn() 
    {
        CursorController.MyInstance.FixCursorMode(CursorController.CursorMode.Eyetracker);
        Managers.Resource.Instantiate("Puzzle/GameAnim");
        Destroy(gameObject);
    }
}
