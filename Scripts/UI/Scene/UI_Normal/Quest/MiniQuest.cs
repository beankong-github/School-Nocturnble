using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniQuest : UI_Base
{
    private string questTitle;
    private string questGoal;
    private int questId;

    public string QuestTitle { get => questTitle; set => questTitle = value; }
    public string QuestGoal { get => questGoal; set => questGoal = value; }
    public int QuestId { get => questId; set => questId = value; }

    private enum Texts
    {
        QuestTitle,
        QuestGoal
    }

    private enum Buttons
    {
        close_btn
    }

    // Start is called before the first frame update
    private void Start()
    {
        Init();

        Get<TextMeshProUGUI>((int)Texts.QuestTitle).text = QuestTitle;
        Get<TextMeshProUGUI>((int)Texts.QuestGoal).text = QuestGoal;
    }

    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.close_btn).onClick.AddListener(DestroyMiniQuestWin);
    }

    private void DestroyMiniQuestWin()
    {
        Destroy(this.gameObject);
    }
}