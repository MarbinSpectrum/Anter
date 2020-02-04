using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnding_Fade : MonoBehaviour
{
    public CEnding_AntManager ending_AntManager;
    [SerializeField] private GameObject endingText;

    private void Awake()
    {
        endingText.SetActive(false);
    }

    public void EndingStart()
    {
        ending_AntManager.CreateStart();
    }

    public void EndingEnd()
    {
        endingText.SetActive(true);
    }

}
