using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotate : MonoBehaviour
{
    public bool rotate = false;
    public float rotationAngle;

    private GameObject child;
    private Vector3 newRotation;
    private float t = 2f;

    private void Start()
    {
        child = transform.GetChild(0).gameObject;
        child.SetActive(false);
    }

    public void BtnClick(string arrow)
    {
        if (!rotate)
        {
            if (arrow == "R")
            {
                rotate = true;
                child.SetActive(true);
                StartCoroutine(Rotate(-rotationAngle));
            }
            if (arrow == "L")
            {
                rotate = true;
                child.SetActive(true);
                StartCoroutine(Rotate(rotationAngle));
            }
        }
    }

    private IEnumerator Rotate(float rot)
    {
        newRotation = new Vector3(0, 0, (transform.eulerAngles.z + rot + 360) % 360);
        while (Mathf.Abs(transform.eulerAngles.z - newRotation.z) >= Mathf.Abs(rot * Time.deltaTime * t))
        {
            transform.Rotate(new Vector3(0, 0, rot) * Time.deltaTime);
            yield return null;
        }

        transform.eulerAngles = newRotation;
        child.SetActive(false);
        rotate = false;
    }
}