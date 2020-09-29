using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pingpong : MonoBehaviour
{
    private RectTransform now;
    public float speed = 1f;
    public float faza = 0f;
    void Start()
    {
        now = GetComponent<RectTransform>();
    }

    void Update()
    {
        now.localPosition = new Vector2(Mathf.PingPong((Time.time + faza) * speed, now.sizeDelta.x) - now.sizeDelta.x/2, 0f);
    }
}
