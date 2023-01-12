using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMouse : MonoBehaviour
{
    public GameObject effect;

    private void Start()
    {
        effect.SetActive(false);
    }

    private void OnMouseEnter()
    {
        effect.SetActive(true);
    }

    private void OnMouseExit()
    { 
        effect.SetActive(false);
    }
}
