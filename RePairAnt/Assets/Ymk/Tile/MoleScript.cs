using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleScript : MonoBehaviour
{
    public Vector2Int[] arr = new Vector2Int[4];
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Mole_Out"))
        {
            for(int i = 0; i < arr.Length; i++)
            {
                if (CAntManager.Instance.AntLocationCheck(arr[i]) && TileManager.instance.shiledArray[arr[i].x,arr[i].y] <= 0)
                {
                    SoundManager.instance.se05away.Play();
                    CAnt ant = CAntManager.Instance.GetAnt(arr[i]);
                    CAntManager.Instance.DeadAnd(ant);
                }
            }
        }
    }
}
