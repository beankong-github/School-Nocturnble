using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EyetrackerStateBar : MonoBehaviour
{
    public Color[] bgColors;

    private TextMeshProUGUI text;
    private Image image;

    private CursorController.CursorMode cursorMode;

    private void Start()
    {
        text = FindObjectOfType<TextMeshProUGUI>();
        image = GetComponent<Image>();

        UpdateStateBar();

        CursorController.CursorModeChanged -= UpdateStateBar;
        CursorController.CursorModeChanged += UpdateStateBar;
    }

    private void UpdateStateBar()
    {
        cursorMode = CursorController.MyInstance.CurCursorMode;

        switch (cursorMode)
        {
            case CursorController.CursorMode.Eyetracker:
            case CursorController.CursorMode.SubPlayer:
                image.color = bgColors[0];
                text.text = "아이트래커 ON";
                break;

            case CursorController.CursorMode.Mouse:
                image.color = bgColors[1];
                text.text = "아이트래커 OFF";
                break;
        }
    }
}