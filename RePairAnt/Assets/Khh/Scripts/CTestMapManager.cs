using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LandType
{
    Dust = 0,
    Road = 1,
}



public class CTestMapManager : MonoBehaviour
{
    private int[,] map = new int [10,18]
    {
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        { 0,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,0},
        { 0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0},
        { 0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0},
        { 0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0},
        { 0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0},
        { 0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
        { 0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
        { 0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
    };

    [SerializeField] private GameObject dust;
    [SerializeField] private GameObject road;
    [SerializeField] private GameObject ant;
    private static CTestMapManager instance;
    public static CTestMapManager Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                Instantiate((map[i, j] == 0 ? dust : road), new Vector3(-8.5f + j, -4.5f + i, 0f), Quaternion.identity, transform);
            } 
        }

        //GameObject go = Instantiate(ant, null);
        //go.GetComponent<CAnt>().SetAnt(new Vector2Int(1, 0));

    }

    public int GetMapInfo(int x, int y)
    {
        if (map.GetLength(1) <= x || 0 > x)
        {
            return 0;
        }
        else if (map.GetLength(0) <= y || 0 > y)
        {
            return 0;
        }
        else
        {
            return map[y, x];
        }
    }

}
