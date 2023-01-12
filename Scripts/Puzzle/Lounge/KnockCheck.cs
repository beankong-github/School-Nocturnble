using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockCheck : MonoBehaviour
{
    private List<GameObject> knocks;
    private GameObject effPrefab;
    private Object obj;

    private int tempNum = 0;
    private int knockCount = 0;

    void Start()
    {
        knocks = new List<GameObject>();
        knockCount = transform.childCount;
        effPrefab = Resources.Load("Prefabs/Puzzle/PuzzleEff") as GameObject;

        for (int i = 0; i < knockCount; i++)
        {
            GameObject knock = transform.GetChild(i).gameObject;

            knocks.Add(knock);
            knocks[i].SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.name == gameObject.name)
                {
                    knocks[tempNum].SetActive(true);
                    tempNum++;
                    CountCheck();
                }
                if (hit.collider.name == "LostPillPrefab") 
                {
                    hit.collider.gameObject.SetActive(false);
                    Instantiate(effPrefab, effPrefab.transform.position, Quaternion.identity);
                    GameObject.Find("Item_Acq_Not").transform.Find("Back").gameObject.SetActive(true);
                }
            }
        }
    }

    private void CountCheck() 
    {
        if (tempNum == knockCount)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            obj = Instantiate(effPrefab, effPrefab.transform.position, Quaternion.identity);
            Invoke("AnimEdit", 1f);
        }
    }

    private void AnimEdit() 
    {
        Destroy(obj);
        MouseAnimCon mouseAnimCon = GameObject.Find("Anim1").GetComponent<MouseAnimCon>();
        mouseAnimCon.animCheck = true;
    }
}
