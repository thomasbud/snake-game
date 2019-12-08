using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using UnityEngine.Windows.Speech;

public class Snake : MonoBehaviour
{
    //Voice commands code. Adds keywords array which contains words to check for. Also sets speed and confidence level. Instantiates all vars for voice
    /*************************************************************************/
    public string[] keywords = new string[] { "up", "down", "left", "right" };
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;

    //note that initial direction is set to right to match default below for keypad controls
    protected PhraseRecognizer recognizer;
    protected string word = "right";
    /*************************************************************************/
    public AudioClip menuSong;
    //sound objs
    public AudioSource eatSound;

    // Current Movement Direction
    // (by default it moves to the right)
    public GameObject foodPrefab;
    public GameObject trapPrefab;

    public TextMeshProUGUI answerDisplay;
    public int score;

	public TextMeshProUGUI helpControls;
	public string controlType;

    // Borders
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;

	//public KeyCode rightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D"));
	//public KeyCode upKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W"));
	//public KeyCode leftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A"));
	//public KeyCode downKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S"));

	public KeyCode rightKey;
	public KeyCode leftKey;
	public KeyCode downKey;
	public KeyCode upKey;
	private Dictionary<string, KeyCode> keyBinds = new Dictionary<string, KeyCode>();


	Vector2 dir = Vector2.right;

    List<Collider2D> traps = new List<Collider2D>();
    List<Transform> tail = new List<Transform>();

    public bool sandbox;


    public void Sandbox()
    {
        sandbox = !sandbox;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Did the snake eat something?
    bool ate = false;

    // Are we using voice controls? Sets update logic for snake movement
    /*************************************************************************/
    //Hard coded variable at the moment. Needs to be added to future settings page. For now, just edit the bool between runs to test both control schemes
    bool voiceEnable;
    /*************************************************************************/

    // Tail Prefab
    public GameObject tailPrefab;
    // Use this for initialization
    void Start()
    {
        //MusicClass.instance.StopMusic(menuSong);
        sandbox = false;

		Debug.Log("Start");

		// Move the Snake every 300ms
		if (System.Math.Abs(Settings.speedVal) < 0.0001) {
            Settings.speedVal = 0.1f;
        }
		voiceEnable = Settings.voiceVal;

		if (voiceEnable == false)
		{
			controlType = "Arrow Keys";
		}
		else
		{
			controlType = "Voice Controls";
		}
		//helpControls.SetText(controlType);
		helpControls.SetText(controlType);

		Debug.Log("Line 92");

		score = 0;
        answerDisplay.SetText(score.ToString());
        SpawnFood();
        InvokeRepeating("SpawnTrap", 2, 7);
        InvokeRepeating("RemoveTrap", 60, 15);
        InvokeRepeating("Move", Settings.speedVal, Settings.speedVal);


		KeyCode rightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D"));
		KeyCode upKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W"));
		KeyCode leftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A"));
		KeyCode downKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S"));

		keyBinds.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
		keyBinds.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
		keyBinds.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
		keyBinds.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));

		Debug.Log(rightKey.ToString());
		Debug.Log(upKey.ToString());
		Debug.Log(leftKey.ToString());
		Debug.Log(downKey.ToString());

		//Recognizer for voice controls, Passes through recognized word
		/*************************************************************************/
		if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        }
		/*************************************************************************/

	}

	void OnTriggerEnter2D(Collider2D coll)
    {
        // Food?
        if (coll.name.StartsWith("apple"))
        {
            // Get longer in next Move call
            ate = true;
            eatSound.Play();

            // Remove the Food
            Destroy(coll.gameObject);
            score += 100;

            if (score == 300) {
                SceneManager.LoadScene(9);
            }
            //insert eat noise clip ehre

            answerDisplay.SetText(score.ToString());
            SpawnFood();
        }
        // Collided with Tail or Border
        else
        {
            //insert death noise clip here

            // ToDo 'You lose' screen
            Debug.Log("You Lose");

            if (sandbox == false)
            {
                MusicClass.instance.PlayMusic(menuSong);
                SceneManager.LoadScene(5);
            }

            dir = Vector2.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
		//Move in a new Direction?
		//if (true)
		//{
		//	if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && dir != -Vector2.right)
		//		dir = Vector2.right;
		//	else if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && dir != Vector2.up)
		//		dir = -Vector2.up;    // '-up' means 'down'
		//	else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && dir != Vector2.right)
		//		dir = -Vector2.right; // '-right' means 'left'
		//	else if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && dir != -Vector2.up)
		//		dir = Vector2.up;
		//}
		if (true)
		{
			if (Input.GetKey(keyBinds["Right"]))
			{
				Debug.Log("KEY PRESS");
				dir = Vector2.right;
			}
			else if (Input.GetKey(keyBinds["Down"]))
			{
				dir = -Vector2.up;    // '-up' means 'down'
			}
			else if (Input.GetKey(keyBinds["Left"]))
			{
				dir = -Vector2.right; // '-right' means 'left'
			}
			else if (Input.GetKey(keyBinds["Up"]))
			{
				dir = Vector2.up;
			}
		}

		//additional code to handle voice control use case. Handles all 4 directions using original logic but with different logical parameter
		/*************************************************************************/
		else
		{
            if (word == "right" && dir != -Vector2.right)
                dir = Vector2.right;
            else if (word == "down" && dir != Vector2.up)
                dir = -Vector2.up;    // '-up' means 'down'
            else if (word == "left" && dir != Vector2.right)
                dir = -Vector2.right; // '-right' means 'left'
            else if (word == "up" && dir != -Vector2.up)
                dir = Vector2.up;
        }
        /*************************************************************************/
    }

    void SpawnFood()
    {
        // x position between left & right border
        int x = (int)Random.Range(borderLeft.position.x, borderRight.position.x);


        // y position between top & bottom border
        int y = (int)Random.Range(borderBottom.position.y, borderTop.position.y);


        // Instantiate the food at (x, y)
        Instantiate(foodPrefab, new Vector2(x, y), Quaternion.identity);

        // default rotation
    }

    void SpawnTrap()
    {
        // x position between left & right border
        int x = (int)Random.Range(borderLeft.position.x, borderRight.position.x);


        // y position between top & bottom border
        int y = (int)Random.Range(borderBottom.position.y, borderTop.position.y);


        // Instantiate the food at (x, y)
        Instantiate(trapPrefab, new Vector2(x, y), Quaternion.identity);

        // default rotation
    }

    void RemoveTrap() {
        //Debug.Log();
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
       
        List<GameObject> trapObjects = new List<GameObject>();
        for (int i = 0; i < allObjects.Length; i++) {
            if (allObjects[i].ToString().StartsWith("TrapPrefab"))
            {
                trapObjects.Add(allObjects[i]);
            }
        }

        Destroy(trapObjects[trapObjects.Count - 1]);
    }

    void Move()
    {
        // Save current position (gap will be here)
        Vector2 v = transform.position;

        // Move head into new direction (now there is a gap)
        transform.Translate(dir);

        // Ate something? Then insert new Element into gap
        if (ate)
        {
            // Load Prefab into the world
            GameObject g = (GameObject)Instantiate(tailPrefab, v, Quaternion.identity);

            // Keep track of it in our tail list
            tail.Insert(0, g.transform);

            // Reset the flag
            ate = false;
        }
        // Do we have a Tail?
        else if (tail.Count > 0)
        {
            // Move last Tail Element to where the Head was
            tail.Last().position = v;

            // Add to front of list, remove from the back
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
    }

    //In the event that a phrase is recognized, set word to it for update logic
    /*************************************************************************/
    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        //return word;
        Debug.Log(word);
    }

    //close the recognizer instance after application quit
    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
    /*************************************************************************/
}