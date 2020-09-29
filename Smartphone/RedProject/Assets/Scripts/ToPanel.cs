using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToPanel : MonoBehaviour {

    public GameObject Device;
    public GameObject Prefabs;
    public GameObject Main;

    public GameObject now;

    public void click_Device()
    {
        reset_now();
        show show_x = Device.GetComponent<show>();
        show_x.Initialize_new_moving(1);
        now = Device;
    }

    public void click_Prefabs()
    {
        reset_now();
        show show_x = Prefabs.GetComponent<show>();
        show_x.Initialize_new_moving(1);
        now = Prefabs;
    }

    public void click_Main()
    {
        if (now != Main)
        {
            reset_now();
            show show_x = Main.GetComponent<show>();
            show_x.Initialize_new_moving(1);
            now = Main;
        };
    }

    public void reset_now()
    {
        show show_x = now.GetComponent<show>();
        show_x.Initialize_new_moving(-1);
    }


    void Start () {

        now = Main;
    }



	void Update () {
		
	}
}
