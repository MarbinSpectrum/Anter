using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CLobby : MonoBehaviour
{
    [SerializeField] private AudioClip sfx;

    [SerializeField] private Button[] stageBtn;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("Stage1"));
        
        //stageBtn[0].interactable = (PlayerPrefs.GetInt("Stage1") != 0);
        stageBtn[1].interactable = (PlayerPrefs.GetInt("Stage2") != 0);
        stageBtn[2].interactable = (PlayerPrefs.GetInt("Stage3") != 0);
        stageBtn[3].interactable = (PlayerPrefs.GetInt("Stage4") != 0);
    }

    public void Stage(int num)
    {
        if(num == 0) SceneManager.LoadScene("Stage01");
        else if (num == 1) SceneManager.LoadScene("Stage02");
        else if (num == 2) SceneManager.LoadScene("Stage03");
        else if (num == 3) SceneManager.LoadScene("Stage04");

        audioSource.PlayOneShot(sfx);
    }

    public void Back()
    {
        audioSource.PlayOneShot(sfx);
        SceneManager.LoadScene("Title");
    }
}
