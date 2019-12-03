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

    private Dictionary<string, KeyCode> keyBinds = new Dictionary<string, KeyCode>();
    public Text up, left, down, right;

    private GameObject currentKey;

    private Color32 normal = new Color32(255, 255, 255, 255);
    private Color32 selected = new Color32(239, 116, 36, 255);

    void Start()
    {
        speedVal = 0.1f;

        keyBinds.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Up", "W")));
        keyBinds.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
        keyBinds.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
        keyBinds.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));

        up.text = keyBinds["Up"].ToString();
        left.text = keyBinds["Left"].ToString();
        down.text = keyBinds["Down"].ToString();
        right.text = keyBinds["Right"].ToString();
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

    private void OnGUI()
    {
        
        if(currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                keyBinds[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
                currentKey.GetComponent<Image>().color = normal;
                currentKey = null;
            }
        }
        
    }

    public void ChangeKey(GameObject clicked)
    {
        if(currentKey != null)
        {
            currentKey.GetComponent<Image>().color = normal;
        }
        currentKey = clicked;
        currentKey.GetComponent<Image>().color = selected;
    }

    public void SaveKeys()
    {
        foreach (var tempKey in keyBinds)
        {
            //PlayerPrefs.SetString(tempKey.Key, tempKey.Value.ToString());
            //string val = tempKey.Value.ToString();
            //PlayerPrefs.SetString(tempKey.Key, val);
            PlayerPrefs.SetString(tempKey.Key, tempKey.Value.ToString());
            //PlayerPrefs.SetString(tempKey.Key, "W");
            //Debug.Log(tempKey.Value.ToString());
            //Debug.Log(PlayerPrefs.GetString("Up"));
            Debug.Log(tempKey.Key + ": " + tempKey.Value.ToString() + ":: " + PlayerPrefs.GetString(tempKey.Key.ToString()));
        }

        PlayerPrefs.Save();
    }
}
