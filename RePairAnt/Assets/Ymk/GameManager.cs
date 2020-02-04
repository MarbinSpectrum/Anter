using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("꿀")]
    public float honeyTime = 3;
    public float honeyCoolTime = 3;
    public float honeyRange = 3;

    [Header("나뭇잎")]
    public int leafNum = 3;
    public float leafCoolTime = 3;

    [Header("꽃줄기")]
    public float flowerStemCoolTime = 3;

    [Header("마늘")]
    public float garlicTime = 3;
    public float garlicCoolTime = 3;

    [Header("거미줄")]
    public float spiderWebTime = 3;
    public float spiderWebCoolTime = 3;

    [Header("방울")]
    public float bubbleTime = 3;
    public float bubbleCoolTime = 3;

    [Header("응가")]
    public float pooTime = 3;
    public float pooCoolTime = 3;

    [Header("애벌레")]
    public float larvaTime = 3;
    public float larvaCoolTime = 3;

    [Header("바위")]
    public float rockCoolTime = 3;

    [Header("일반개미목표수")]
    public int normalAntTargetAmount = 2;
    [HideInInspector] public int nowNormalAnt = 0;

    [Header("채굴개미목표수")]
    public int mineAntTargetAmount = 2;
    [HideInInspector] public int nowMineNormalAnt = 0;

    [Header("제한시간")]
    public float timeLimit = 60;

    bool EndGame = false;
    [HideInInspector]  public bool AntNumFlag = false;
    int GameResult = 0;

    float Endtime = 4;

    //1 승리 2 패배

    void Awake()
    {
        nowNormalAnt = 0;
        nowMineNormalAnt = 0;
        instance = this;
    }

    void Start()
    {
        SoundManager.instance.inGame.Play();

        SoundManager.instance.inGame.Play();
        if (SceneManager.GetActiveScene().name == "Stage01")
            PlayerPrefs.SetInt("Stage1", 1);
        else if (SceneManager.GetActiveScene().name == "Stage02")
            PlayerPrefs.SetInt("Stage2", 1);
        else if (SceneManager.GetActiveScene().name == "Stage03")
            PlayerPrefs.SetInt("Stage3", 1);
        else if (SceneManager.GetActiveScene().name == "Stage04")
            PlayerPrefs.SetInt("Stage4", 1);    
    }

    private void Update()
    {
        Debug.Log(nowNormalAnt + "," + nowMineNormalAnt);

        if (!EndGame)
            timeLimit -= Time.deltaTime;
        else
            Endtime -= Time.deltaTime;

        if(Endtime < 0)
        {
            Endtime = 5;
            if (GameResult == 2)
                SceneManager.LoadScene("Title");
            else if (SceneManager.GetActiveScene().name == "Stage01")
                SceneManager.LoadScene("Stage02");
            else if (SceneManager.GetActiveScene().name == "Stage02")
                SceneManager.LoadScene("Stage03");
            else if (SceneManager.GetActiveScene().name == "Stage03")
                SceneManager.LoadScene("Stage04");
            else if (SceneManager.GetActiveScene().name == "Stage04")
                SceneManager.LoadScene("Ending");
        }

        timeLimit = timeLimit < 0 ? 0 : timeLimit;
        GameEndCheck();

    }

    void GameEndCheck()
    {
        if (EndGame)
            return;

        if(nowNormalAnt >= normalAntTargetAmount && nowMineNormalAnt >= mineAntTargetAmount)
        {
            GameResult = 1;
            SoundManager.instance.se03succes.Play();
            UIManager.instance.winUI.SetActive(true);
            EndGame = true;
            return;
        }

        if (timeLimit <= 0)
        {
            GameResult = 2;
            SoundManager.instance.se04fail.Play();
            UIManager.instance.loseUI.SetActive(true);
            EndGame = true;
            return;
        }

        if (AntNumFlag)
        {
            int minenum = 0;
            int normalnum = 0;

            for(int i = 0; i < CAntManager.Instance.antList.Count; i++)
            {
                CAnt ant = CAntManager.Instance.antList[i];
                try
                {
                    CMineAnt emp0 = (CMineAnt)ant;
                    if (emp0)
                        minenum++;
                }
                catch { }

                try
                {
                    CNormalAnt emp1 = (CNormalAnt)ant;
                    if (emp1)
                        normalnum++;
                }
                catch { }
            }

            if (minenum < (mineAntTargetAmount - nowMineNormalAnt) || normalnum < (normalAntTargetAmount - nowNormalAnt))
            {
                GameResult = 2;
                SoundManager.instance.se04fail.Play();
                UIManager.instance.loseUI.SetActive(true);
                EndGame = true;
                return;
            }
        }
    }

    public void GoTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
