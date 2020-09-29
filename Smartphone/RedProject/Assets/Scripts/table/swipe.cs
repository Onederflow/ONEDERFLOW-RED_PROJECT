using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class swipe : MonoBehaviour
{
    public matrix3 mx;
    private bool mouse = false;

    private void OnMouseOver()
    {
        if (mouse)
        {
            mx.set_position(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    private void OnMouseDown()
    {
        mouse = true;
        mx.set_position(Input.mousePosition.x, Input.mousePosition.y);
    }

    private void OnMouseUp()
    {
        mouse = false;
        mx.set_position(Input.mousePosition.x, Input.mousePosition.y);
    }
}

