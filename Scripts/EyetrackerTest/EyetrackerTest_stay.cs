using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyetrackerTest_stay : MonoBehaviour
{
    private List<StayEyetracker> childs_stayEyetracker;

    private void Start()
    {
        childs_stayEyetracker = new List<StayEyetracker>();

        for (int i = 0; i < transform.childCount; i++)
        {
            StayEyetracker tmp;
            transform.GetChild(i).TryGetComponent(out tmp);

            if (tmp != null)
                childs_stayEyetracker.Add(tmp);
        }
    }

    public void CheckAllStarDone()
    {
        for (int i = 0; i < childs_stayEyetracker.Count; i++)
        {
            if (childs_stayEyetracker[i].isInteracterble)
                return;
        }

        Destroy(gameObject);
        Managers.Resource.Instantiate("Util/EyeTestCanvas_click");
    }
}