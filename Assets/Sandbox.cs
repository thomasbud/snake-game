using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandbox : MonoBehaviour
{
    public bool godmode;

    // Start is called before the first frame update
    void Start()
    {
        godmode = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void godModeSwitch()
    {
        if(godmode == false)
        {
            godmode = true;
        } else
        {
            godmode = false;
        }
    }


}
