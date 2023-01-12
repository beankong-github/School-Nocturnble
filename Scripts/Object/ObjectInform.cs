using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectInform : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private int id;

    private BaseObject myObject;
    private UI_Dialog dialogUI;

    public BaseObject MyObject { get => myObject; set => myObject = value; }

    private void Start()
    {
        Managers.Data.ObjectDic.TryGetValue(id, out myObject);
        dialogUI = FindObjectOfType<UI_Dialog>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (dialogUI == null)
        {
            dialogUI = UI_Dialog.MyInstance;
        }
        dialogUI.Action(MyObject.id);
    }
}