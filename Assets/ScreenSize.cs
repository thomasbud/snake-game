using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("bitch");
        Screen.SetResolution(1920, 1080, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resolution1()
    {
        Debug.Log("res1");
        Screen.SetResolution(1280, 960, true);

    }

    public void Resolution2()
    {

        Debug.Log("res2");
        Screen.SetResolution(1280, 720, true);

    }

    public void Resolution3()
    {
        Debug.Log("res3");
        Screen.SetResolution(1920, 1080, true);

    }
}
