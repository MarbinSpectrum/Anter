using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CTitle : MonoBehaviour
{
    [SerializeField] private AudioClip sfx;

    [SerializeField] private Button loadGame;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        loadGame.interactable = (PlayerPrefs.GetInt("PlayerCheck", 0) != 0);
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Stage1,", 1);
        PlayerPrefs.SetInt("Stage2,", 0);
        PlayerPrefs.SetInt("Stage3,", 0);
        PlayerPrefs.SetInt("Stage4,", 0);
        PlayerPrefs.SetInt("PlayerCheck", 1);
        PlayerPrefs.Save();

        Debug.Log("TTT " + PlayerPrefs.GetInt("Stage1"));
        audioSource.PlayOneShot(sfx);

        SceneManager.LoadScene("Lobby");
    }

    public void LoadGame()
    {
        audioSource.PlayOneShot(sfx);
        SceneManager.LoadScene("Lobby");
    }

}
