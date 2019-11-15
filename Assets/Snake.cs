﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Snake : MonoBehaviour
{
    // Current Movement Direction
    // (by default it moves to the right)
    public GameObject foodPrefab;
    public GameObject trapPrefab;

    public TextMeshProUGUI answerDisplay;
    public int score;

    // Borders
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;

    Vector2 dir = Vector2.right;

    List<Collider2D> traps = new List<Collider2D>();
    List<Transform> tail = new List<Transform>();

    // Did the snake eat something?
    bool ate = false;

    // Tail Prefab
    public GameObject tailPrefab;
    // Use this for initialization
    void Start()
    {
        // Move the Snake every 300ms
        score = 0;
        answerDisplay.SetText(score.ToString());
        SpawnFood();
        InvokeRepeating("SpawnTrap", 2, 7);
        InvokeRepeating("RemoveTrap", 60, 15);
        InvokeRepeating("Move", 0.1f, 0.1f);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        // Food?
        if (coll.name.StartsWith("apple"))
        {
            // Get longer in next Move call
            ate = true;

            // Remove the Food
            Destroy(coll.gameObject);
            score += 100;
            answerDisplay.SetText(score.ToString());
            SpawnFood();
        }
        // Collided with Tail or Border
        else
        {
            // ToDo 'You lose' screen
            Debug.Log("You Lose");
            SceneManager.LoadScene(5);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move in a new Direction?
        if (Input.GetKey(KeyCode.RightArrow) && dir != -Vector2.right)
            dir = Vector2.right;
        else if (Input.GetKey(KeyCode.DownArrow) && dir != Vector2.up)
            dir = -Vector2.up;    // '-up' means 'down'
        else if (Input.GetKey(KeyCode.LeftArrow) && dir != Vector2.right)
            dir = -Vector2.right; // '-right' means 'left'
        else if (Input.GetKey(KeyCode.UpArrow) && dir != -Vector2.up)
            dir = Vector2.up;
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
}