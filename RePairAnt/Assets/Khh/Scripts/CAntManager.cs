using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum AntType
{
    NormalAnt = 0,
    MineAnt = 1,
}

[Serializable]
public class CreateAnt
{
    public AntType antType;
    public Vector2Int createAntPos;
    public float antCreateTime;
}

public class CAntManager : MonoBehaviour
{
    [SerializeField] private GameObject normalAnt;
    [SerializeField] private GameObject mineAnt;

    [SerializeField] private CreateAnt[] createAnts;


    private float createTime = 0f;

    private float time = 0f;
    private int createNum = 0;

    [HideInInspector] public List<CAnt> antList = new List<CAnt>();
    private static CAntManager instance;
    public static CAntManager Instance { get { return instance; } }

    private float danceTime = 0f;
    private float nextDanceTime = 6f;

    private int mineAntCount = 1;
    private int normalAntCount = 1;


    private void Awake()
    {
        instance = this;
        createTime = createAnts[createNum].antCreateTime;
    }

    private void Update()
    {
        if (createNum < createAnts.Length)
        {
            time += Time.deltaTime;
            if (time >= createTime)
            {
                time -= createTime;
                GameObject go = Instantiate((createAnts[createNum].antType == AntType.NormalAnt) ? normalAnt : mineAnt, transform);
                CAnt ant = go.GetComponent<CAnt>();
                ant.SetAnt(createAnts[createNum].createAntPos);
                if(createAnts[createNum].antType == AntType.NormalAnt)
                {
                    normalAntCount++;
                    ant.Order(5000 + (normalAntCount * 100));
                }
                else
                {
                    mineAntCount++;
                    ant.Order(10000 + (mineAntCount * 100));
                }

                antList.Add(ant);
                createNum++;
                if (createNum < createAnts.Length)
                {
                    createTime = createAnts[createNum].antCreateTime;
                }

            }
        }
        else
            GameManager.instance.AntNumFlag = true;
        danceTime += Time.deltaTime;
        if (danceTime >= nextDanceTime)
        {
            danceTime = 0f;
            Dance();
        }
    }
    
    public bool AntLocationCheck(Vector2Int tilelocation)
    {
        for (int i = 0; i < antList.Count; i++)
        {
            if(antList[i].GetLocation() == tilelocation)
            {
                return true;
            }
        }
        return false;
    }

    public bool NextAntLocationCheck(Vector2Int tilelocation)
    {
        for (int i = 0; i < antList.Count; i++)
        {
            if (antList[i].GetNextLocation() == tilelocation)
            {
                return true;
            }
        }
        return false;
    }


    public CAnt GetAnt(Vector2Int tilelocation)
    {
        for (int i = 0; i < antList.Count; i++)
        {
            if (antList[i].GetLocation() == tilelocation)
            {
                return antList[i];
            }
        }
        return null;
    }

    public void DeadAnd(CAnt deadAnt)
    {
        if(antList.Contains(deadAnt))
        {
            antList.Remove(deadAnt);
        }
        Destroy(deadAnt.gameObject);
    }

    public void Dance()
    {
        bool flag = false;
        for (int i = 0; i < antList.Count; i++)
        {
            try
            {
                CNormalAnt emp = (CNormalAnt)antList[i];
                if (emp)
                {
                    flag = true;
                    break;
                }
            }
            catch { }
        }
        if (flag)
            SoundManager.instance.se07dance.Play();
        for (int i = 0; i < antList.Count; i++)
        {
            antList[i].Dance();
        }
    }
}
