﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioClip menuSong;

    public void onMainClick()
    {
        SceneManager.LoadScene(0);
    }

    public void onPlayClick()
    {
        MusicClass.instance.StopMusic(menuSong);
        SceneManager.LoadScene(1);
    }

    public void onHelpClick()
    {
        SceneManager.LoadScene(2);
    }

    public void onScoresClick()
    {
        SceneManager.LoadScene(3);
    }

    public void onSettingsClick()
    {
        SceneManager.LoadScene(4);
    }

    public void onStoryClick()
    {
        SceneManager.LoadScene(6);
    }

    public void onStory1Click()
    {
        SceneManager.LoadScene(7);
    }

    public void onStory2Click()
    {
        SceneManager.LoadScene(8);
    }

    public void onNextLevelClick()
    {
        MusicClass.instance.PlayMusic(menuSong);
        SceneManager.LoadScene(10);
    }

    public void onQuitClick()
    {
        Application.Quit();
    }

}
