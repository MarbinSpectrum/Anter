using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAntManager : MonoBehaviour
{
    [SerializeField] private GameObject ant;
    [SerializeField] private GameObject mineAnt;

    private List<CAnt> antList = new List<CAnt>();
    private static CAntManager instance;
    public static CAntManager Instance { get { return instance; } }

    // Start is called before the first frame update
    private void Start()
    {
        //GameObject go = Instantiate(ant, transform);
        //go.GetComponent<CAnt>().SetAnt(new Vector2Int(5, 3));
        GameObject go = Instantiate(mineAnt, transform);
        CAnt ant = go.GetComponent<CAnt>();
        antList.Add(ant);
        ant.SetAnt(new Vector2Int(17, 5), AntDir.Left);
    }

    public bool AntLocationCheck(Vector2Int tileCation)
    {
        for (int i = 0; i < antList.Count; i++)
        {
            if(antList[i].GetLocation() == tileCation)
            {
                return true;
            }
        }
        return false;
    }
}
