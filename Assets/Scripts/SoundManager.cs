using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource[] source;
    //0 - collect sound fx
    //1 - car running sound fx
    //2 -car idle sound fx
    //3 - punchshot01
    //4 - punchshot02



    //5 - when player win's
    //6 - button sound fx

    private void Awake()
    {
        instance = this;
    }

    public void PlaySound(int i)
    {
      if(source[i] != null)
      {
            source[i].Play();
      }
    }

    public void Stop(int i)
    {
        source[i].Stop();
    }

    public void StopAllSounds()
    {
        for(int i = 0; i < source.Length; i++)
        {
            source[i].Stop();
        }
    }
}
