using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;

    private static Managers Instance
    { get { Init(); return s_instance; } }

    private ResourceManager _resource = new ResourceManager();
    private UIManager _ui = new UIManager();
    private EyetrackerManager _eyetracker = new EyetrackerManager();
    private DataManager _data = new DataManager();
    private DialogueManager _dialogue = new DialogueManager();
    private QuestManager _quest = new QuestManager();
    private EventManager _event = new EventManager();

    public static ResourceManager Resource { get { return Instance._resource; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static EyetrackerManager Eyetracker { get { return Instance._eyetracker; } }
    public static DataManager Data { get { return Instance._data; } }
    public static DialogueManager Dialogue { get { return Instance._dialogue; } }
    public static QuestManager Quest { get { return Instance._quest; } }
    public static EventManager Event { get { return Instance._event; } }

    private void Awake()
    {
        Init();

        s_instance._data.Init();
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Manager");
            if (go == null)
            {
                go = new GameObject { name = "@Manager" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }
    }
}