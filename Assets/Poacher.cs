using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using UnityEngine.Windows.Speech;

public class Poacher : MonoBehaviour
{

	//sound objs

	// Current Movement Direction
	// (by default it moves to the right)
	public GameObject poacher;

	// Borders
	public Transform borderTop;
	public Transform borderBottom;
	public Transform borderLeft;
	public Transform borderRight;

	Vector2 dirPoacher = Vector2.right;

	void Start()
	{
		// Move the Snake every 300ms
		if (System.Math.Abs(Settings.speedVal) < 0.0001)
		{
			Settings.speedVal = 0.1f;
		}
		InvokeRepeating("MovePoacher", Settings.speedVal, Settings.speedVal);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		// Food?
		if (coll.name.StartsWith("tails") || coll.name.StartsWith("Head"))
		{
			// Get longer in next Move call
			Debug.Log("You Lose");
			SceneManager.LoadScene(5);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (poacher.transform.position.x >= borderRight.position.x - 3)
		{
			dirPoacher = -Vector2.right;
		}
		if (poacher.transform.position.x <= borderLeft.position.x + 3)
		{
			dirPoacher = Vector2.right;
		}
	}

	void MovePoacher()
	{
		poacher.transform.Translate(dirPoacher);
	}

	/*************************************************************************/
}