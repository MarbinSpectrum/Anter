using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEndingAnt : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float endTime = 2f;
    private float time = 0f;

    private Animator antAni;

    private Vector2 dir;

    private IEnding_AniManager ending_AniManager;
    private Anima2D.SpriteMeshInstance[] spriteMeshInstances;
    private void Awake()
    {
        antAni = GetComponentInChildren<Animator>();
        spriteMeshInstances = GetComponentsInChildren<Anima2D.SpriteMeshInstance>();
    }

    private void Start()
    {
        Setting(transform.position);
    }
    public void Order(int order)
    {
        for (int i = 0; i < spriteMeshInstances.Length; i++)
        {
            spriteMeshInstances[i].sortingOrder += order;
        }
    }

    public void Setting(Vector3 pos)
    {
        transform.position = pos;
        dir = new Vector2(pos.x / 10, -1f);
        endTime = Random.Range(0.5f, endTime);
        SetAni(AntAni.Walk);
    }

    public void EndingManager(IEnding_AniManager ending_AniManager)
    {
        this.ending_AniManager = ending_AniManager;
    }

    public virtual void SetAni(AntAni antAni)
    {
        for (AntAni i = 0; i <= AntAni.Walk; i++)
        {
            this.antAni.SetBool(i.ToString(), antAni == i);
        }


    }

    public void Ending()
    {
        antAni.SetTrigger("Ending");
    }

    // Update is called once per frame
    private void Update()
    {
        if(time < endTime)
        {
            time += Time.deltaTime;
            transform.Translate(dir * speed * Time.deltaTime);
            if(time >= endTime)
            {
                SetAni(AntAni.Idle);
                ending_AniManager.MoveEnd();
            }

        }
    }
}
