using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleCont: MonoBehaviour
{
    public RectTransform Point;
    private void Start()
    {
        if (GetComponent<Toggle>().isOn)
        {
            Point.anchoredPosition = new Vector3(86, Point.anchoredPosition.y);
        }
        else
        {
            Point.anchoredPosition = new Vector3(-86, Point.anchoredPosition.y);
        }
    }


    public void Click()
    {
        if (GetComponent<Toggle>().isOn)
        {
            Point.anchoredPosition = new Vector3(86, Point.anchoredPosition.y);
        }
        else
        {
            Point.anchoredPosition = new Vector3(-86, Point.anchoredPosition.y);
        }
        
    }
}
