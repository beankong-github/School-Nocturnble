using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    private CursorController.CursorMode PrevCursorMode;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (!Managers.UI.IsPopupUIExist())
                if (gameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
    }

    public void Resume()
    {
        CursorController.MyInstance.CurCursorMode = PrevCursorMode;

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    private void Pause()
    {
        PrevCursorMode = CursorController.MyInstance.CurCursorMode;
        CursorController.MyInstance.CurCursorMode = CursorController.CursorMode.Mouse;

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }
}