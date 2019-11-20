using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update

    public Slider slide;

    void Start()
    {
        slide.value = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        Snake.speed = slide.value;
    }
}
