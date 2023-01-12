using System.Collections;
using UnityEngine;

public class Define
{
    public enum SceneName
    {
        Unknown,
        Intro,
        Title,
        EyetrackerTestScene,
        AdmissionNotice,
        Entrance01,
        MainHall01,
        WatchTower01,
        WestHallway01,
        Library01,
        SlidePuzzle,
        EastHallway01,
        PrincipalOffice01,
        Entrance,
        MainHall02,
        EastHallway02,
        PrincipalOffice02,
        WestHallway02,
        Library02,
        Library_ListPuzzle,
        Library_LockPuzzle,
        Library03,
        Battle,
        MainHall03,
        Letter
    }

    public enum SceneType
    {
        Etc,
        Game,
        Puzzle,
        Battle
    }

    public enum UIEvent
    {
        Click,
        RightClick,
        DoubleClick,
        Drag,
        Up,
        Down,
        Enter,
        Exit
    }

    public enum TransferMapType
    {
        Unknown,
        Door,
        Teleport,
        Smooth,
    }
}