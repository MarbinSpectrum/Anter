using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
