using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using System.Runtime.InteropServices;
using System.Drawing;
using UnityEngine.UI;

public class EyetrackerManager
{
    public bool isConnected()
    {
        return TobiiAPI.IsConnected;
    }

    public Vector2 ProjectionToPlaneInWorld(GazePoint gazePoint, Camera mainCam)
    {
        Vector3 gazeOnScreen = gazePoint.Screen;

        if (mainCam == null)
        {
            Debug.Log("mainCamera is null");
            return Vector2.zero;
        }

        if (float.IsNaN(gazeOnScreen.x) || float.IsNaN(gazeOnScreen.y))
        {
            Debug.Log("Undefined gaze point");
            return Vector2.zero;
        }

        return mainCam.ScreenToWorldPoint(gazeOnScreen);
    }

    public Vector2 ProjectionToUI(GazePoint gazePoint)
    {
        Vector3 gazeOnUI = gazePoint.Screen;

        if (float.IsNaN(gazeOnUI.x) || float.IsNaN(gazeOnUI.y))
        {
            Debug.Log("Undefined gaze point");
            return Vector2.zero;
        }

        return gazeOnUI;
    }

    public void MoveGOwithEyetracker(GameObject _go, float _speed)
    {
        if (!TobiiAPI.IsConnected && !TobiiAPI.GetDisplayInfo().IsValid)
            return;

        GazePoint gazePoint = TobiiAPI.GetGazePoint();
        if (!float.IsNaN(gazePoint.GUI.x) || !float.IsNaN(gazePoint.GUI.y))
        {
            if (gazePoint.IsRecent())
            {
                Vector3 targetPos = Managers.Eyetracker.ProjectionToPlaneInWorld(gazePoint, Camera.main);
                _go.transform.position = Vector2.Lerp(_go.transform.position, new Vector2(targetPos.x, targetPos.y), Time.deltaTime * _speed);
            }
        }
    }

    public void MoveUIwithEyetracker(GameObject _ui, float _speed)
    {
        if (!TobiiAPI.IsConnected && !TobiiAPI.GetDisplayInfo().IsValid)
            return;

        GazePoint gazePoint = TobiiAPI.GetGazePoint();

        if (gazePoint.IsRecent())
        {
            Vector2 targetPos = Managers.Eyetracker.ProjectionToUI(gazePoint);
            if (targetPos == Vector2.zero)
                return;

            RectTransform goRect = _ui.GetComponent<RectTransform>();
            goRect.anchoredPosition = Vector3.Lerp(goRect.anchoredPosition, new Vector2(targetPos.x, targetPos.y), Time.deltaTime * _speed);
            return;
        }

        return;
    }

    public bool IsBlink()
    {
        if (!isConnected())
            return false;

        GazePoint gazePoint = TobiiAPI.GetGazePoint();
        if (gazePoint.IsRecent(0.1f))
        {
            return false;
        }

        return true;
    }
}