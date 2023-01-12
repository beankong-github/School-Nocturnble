using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_EyetrackingTest : MonoBehaviour
{
    public Button btn;
    public TextMeshProUGUI title;
    public TextMeshProUGUI content;

    public string[] contents;

    private int counter;
    private bool isConnected;

    private void Start()
    {
        counter = 0;

        btn.GetComponentInChildren<TextMeshProUGUI>().text = "다 음";
        content.text = contents[counter];
    }

    public void OnButtonCilcked()
    {
        counter++;

        if (counter < contents.Length - 1)
        {
            content.text = contents[counter];
            if (counter == 2)
                StartCoroutine(CheckEyeTracker());
        }
        else if (counter == contents.Length - 1)
        {
            content.text = contents[counter];
            btn.GetComponentInChildren<TextMeshProUGUI>().text = "확 인";
        }
        else if (counter > contents.Length - 1)
        {
            if (isConnected)
            {
                Destroy(this.gameObject);
                Managers.Resource.Instantiate("Util/EyeTestCanvas_stay");
                CursorController.MyInstance.FixCursorMode(CursorController.CursorMode.Eyetracker);
            }
            else
                Util.ExitGame();
        }
    }

    private IEnumerator CheckEyeTracker()
    {
        isConnected = Managers.Eyetracker.isConnected();
        btn.interactable = false;

        for (int i = 0; i < 3; i++)
        {
            content.text = contents[counter];

            for (int j = 0; j < 3; j++)
            {
                content.text += '.';
                yield return new WaitForSeconds(0.3f);
            }
            new WaitForSeconds(0.3f);
        }

        if (!isConnected)
        {
            contents[3] = "연결된 아이트래커를 찾을 수 없습니다. 아이트래커 연결을 확인해주십시오. 확인 버튼을 누르면 게임이 종료됩니다.";
        }

        OnButtonCilcked();
        btn.interactable = true;
    }
}