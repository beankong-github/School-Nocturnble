using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCon : MonoBehaviour
{
    void Start()
    {
        Invoke("CreateNextGame", 3.2f);
    }

    private void CreateNextGame() 
    {
        Destroy(gameObject);
        Managers.Resource.Instantiate("Puzzle/Next_Game");
    }
}
