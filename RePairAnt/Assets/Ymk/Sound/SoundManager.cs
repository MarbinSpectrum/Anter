using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource antSong;
    public AudioSource inGame;
    public AudioSource se01;
    public AudioSource se02ingame;
    public AudioSource se03succes;
    public AudioSource se04fail;
    public AudioSource se05away;
    public AudioSource se06shovel;
    public AudioSource se07dance;

    void Awake()
    {
        instance = this;
    }

}
