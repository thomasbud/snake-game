using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicClass : MonoBehaviour
{
    private AudioSource audioClip;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        audioClip = GetComponent<AudioSource>();
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (audioClip.isPlaying) return;
        audioClip.Play();
    }

    public void StopMusic()
    {
        audioClip.Stop();
    }



}