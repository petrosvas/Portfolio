using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    AudioSource[] sounds;

    private void Start()
    {
        DontDestroyOnLoad(this);
        // 0 = victory
        // 1 = defeat
        // 2 = menu theme
        // 3 = game theme
        // 4 = menu click
        // 5 = piece click
    }

    public void PlayVictory()
    {
        sounds[3].Stop();
        sounds[0].Play();
    }

    public void PlayDefeat()
    {
        sounds[3].Stop();
        sounds[1].Play();
    }

    public void PlayMenuTheme()
    {
        if(sounds == null)
        {
            sounds = GetComponents<AudioSource>();
            sounds[2].Play();
        }
        else
        {
            if (sounds[3].isPlaying)
                sounds[3].Stop();
            if (sounds[0].isPlaying)
                sounds[0].Stop();
            if (sounds[1].isPlaying)
                sounds[1].Stop();
            if (sounds[2].isPlaying)
                return;
            else
                sounds[2].Play();
        }
    }

    public void PlayGameTheme()
    {
        sounds[2].Stop();
        sounds[3].Play();
    }

    public void PlayMenuClick()
    {
        sounds[4].Play();
    }

    public void PlayPieceClick()
    {
        sounds[5].Play();
    }

}
