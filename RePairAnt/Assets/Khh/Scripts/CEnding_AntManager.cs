using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IEnding_AniManager
{
    void MoveEnd();
}

public class CEnding_AntManager : MonoBehaviour, IEnding_AniManager
{

    [SerializeField] private GameObject ending_NormalAnt;
    [SerializeField] private GameObject ending_MineAnt;

    private bool ending_Enter = false;

    private float time = 0f;

    private float enterTime = 0.5f;

    private List<CEndingAnt> endingAnt = new List<CEndingAnt>();

    private int antCount = 20;
    private int endAntCount = 20;
    private int antMoveEnd = 0;

    [SerializeField] private AudioClip sfx;
    private AudioSource audioSource;
    [SerializeField] private Animator fadeAni;
    [SerializeField] private Animator queenAntAni;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        if(ending_Enter)
        {
            if(antCount > 0)
            {
                time += Time.deltaTime;
                if (time >= enterTime)
                {
                    time = 0f;
                    CreateAnt();
                }
            }

        }

        
    }

    public void CreateStart()
    {
        ending_Enter = true;
    }

    private void CreateAnt()
    {
        int ran = Random.Range(0, 100);
        GameObject createGo = (ran < 25) ? ending_MineAnt : ending_NormalAnt;
        GameObject go = Instantiate(createGo, transform);
        CEndingAnt ant = go.GetComponent<CEndingAnt>();
        endingAnt.Add(ant);
        ant.EndingManager(this);
        ant.Order((endAntCount - antCount + 1) * 100);
        go.transform.position = new Vector3(Random.Range(-4f, 4f), 6f);
        antCount--;

        if(!queenAntAni.GetBool("Start"))
        {
            //Debug.Log("체크!!");
            if(antCount > 10)
            {
                queenAntAni.SetBool("Start", true);
                //Debug.Log("체크222!!");
            }
        }

    }

    public void MoveEnd()
    {
        antMoveEnd++;
        //Debug.Log("Ending Count!! " + antCount + " " + antMoveEnd);
        if (antCount <= 0 && antMoveEnd >= endAntCount)
        {
            StartCoroutine(WaitEnding());
            for (int i = 0; i < endingAnt.Count; i++)
            {
                endingAnt[i].Ending();
            }
            //Debug.Log("Ending!!");
        }
    }


    IEnumerator WaitEnding()
    {
        yield return new WaitForSeconds(6f);
        fadeAni.SetBool("Out", true);
    }

    public void Title()
    {
        audioSource.PlayOneShot(sfx);
        SceneManager.LoadScene("Title");
    }

}
