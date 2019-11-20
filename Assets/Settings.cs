using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public Toggle voiceToggle;
    public static float speedVal;
    public static bool voiceVal = false;
    void Start()
    {
        speedVal = 0.1f;
    }

    public void UsingVoice()
    {
        voiceVal = !voiceVal;
    }
    // Update is called once per frame
    void Update()
    {
        speedVal = slider.value;
    }
}
