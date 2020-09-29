using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    private Quaternion res;
    private float angle;
    public float to_angle;
    public float speed;
    public bool process;
    private  Quaternion new_angle;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (((this.transform.localRotation.eulerAngles.z >= to_angle + 0.1f)|| (this.transform.localRotation.eulerAngles.z <= to_angle - 0.1f )) && (to_angle <= 360f) && (to_angle >= 0f))
        {
            process = true;

            if (to_angle > this.transform.localRotation.eulerAngles.z)
                angle = speed;
            else
                angle = -speed;

            if (Mathf.Abs(to_angle - this.transform.localRotation.eulerAngles.z) > 180f)
                angle = -angle;
     
            new_angle.eulerAngles = new Vector3(0f,0f, this.transform.localRotation.eulerAngles.z + angle);
            this.transform.localRotation = new_angle;
        }
        else process = false;
    }
}
