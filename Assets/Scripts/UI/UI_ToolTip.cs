using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] private float xLimit = 960;
    [SerializeField] private float yLimit = 540;
    [SerializeField] private float xOffSet = 150;
    [SerializeField] private float yOffSet = 150;
    public virtual void adjustPosition()
    {
        float newXOffSet = 0;
        float newYOffSet = 0;
        Vector2 MousePos = Input.mousePosition;
        if (MousePos.x > xLimit)
            newXOffSet = -xOffSet;
        else newXOffSet = xOffSet;

        if (MousePos.y > yLimit)
        {
            newYOffSet = -yOffSet;
        }
        else newYOffSet = yOffSet;

        transform.position = new Vector2(newXOffSet + MousePos.x, MousePos.y + newYOffSet);
    }
}
