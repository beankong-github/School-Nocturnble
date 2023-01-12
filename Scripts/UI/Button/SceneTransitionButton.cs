using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SceneTransitionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public SceneController SceneController;
    public Define.SceneName nextScene = Define.SceneName.Unknown;

    private Outline outline;

    private CursorController cursorController;
    private bool isCursorEntered;

    private void Start()
    {
        cursorController = GameObject.Find("Cursor").GetComponent<CursorController>();
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    private void Update()
    {
        if (isCursorEntered && Managers.Eyetracker.IsBlink())
            StartTransfer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cursor")
        {
            isCursorEntered = true;
            outline.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Cursor")
        {
            isCursorEntered = false;
            outline.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
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