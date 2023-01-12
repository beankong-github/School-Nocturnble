using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleExitButton : MonoBehaviour, IPointerClickHandler
{
    public SceneController SceneController;
    public Define.SceneName nextScene = Define.SceneName.Unknown;

    private void Start()
    {
        SceneController = GameObject.Find("@Scene").GetComponent<SceneController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartTransfer();
    }

    public void StartTransfer()
    {
        if (nextScene != Define.SceneName.Unknown)
            SceneController.nextScene = nextScene;
        SceneController.TimeChangedScene();
    }
}