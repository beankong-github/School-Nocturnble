using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEyeInteractableButton : MonoBehaviour
{
    protected bool isCursorEntered;

    protected virtual void OnEnable()
    {
        CursorController.BlinkAlert += DetectBlink;
        CursorController.CursorModeChanged += UpdateCursorMode;
    }

    protected virtual void OnCursorEnter()
    { }

    protected virtual void OnCursorExit()
    { }

    protected virtual void DetectBlink()
    {
    }

    protected virtual void UpdateCursorMode()
    {
    }

    protected virtual void OnDisable()
    {
        CursorController.BlinkAlert -= DetectBlink;
        CursorController.CursorModeChanged -= UpdateCursorMode;
    }
}