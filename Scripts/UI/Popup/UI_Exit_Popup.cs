using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Exit_Popup : MonoBehaviour
{
    public PauseMenu pauseMenu;

    public void OnOKPreseed()
    {
        Util.ExitGame();
    }

    public void OnCancelPressed()
    {
        pauseMenu.Resume();
    }
}