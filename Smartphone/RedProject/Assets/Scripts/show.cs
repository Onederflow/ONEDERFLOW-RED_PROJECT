using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class show : MonoBehaviour {
    public float speed = 10f;
    private RectTransform rect;
    private int moving = 0;

    public void Initialize_new_moving(int operation)
    {
        rect = GetComponent<RectTransform>();
        moving = operation;
        if(moving == 1)
            rect.localPosition = new Vector2(rect.rect.size.x, 0f);
    }


    void FixedUpdate() {
        if (moving == 1)
        {
            if (rect.localPosition.x > 0)
            {
               rect.localPosition = new Vector2(rect.localPosition.x - speed,0f);
            };
            if (rect.localPosition.x <= 0)
            {
                rect.localPosition = new Vector2(0f, 0f);
                moving = 0;
            };
        };
        if (moving == -1)
        {
            if (rect.localPosition.x > -rect.rect.size.x)
            {
                rect.localPosition = new Vector2(rect.localPosition.x - speed, 0f);
            };
            if (rect.localPosition.x <= -rect.rect.size.x)
            {
                rect.localPosition = new Vector2(rect.rect.size.x, 0f);
                moving = 0;
            };
        };
    }
}
