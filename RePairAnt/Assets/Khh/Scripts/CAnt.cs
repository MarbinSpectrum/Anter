using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AntDir
{
    Up = 0,
    Down = 1,
    Right = 2,
    Left = 3,
}
public enum AntAni
{
    Idle = 0,
    Walk = 1,
    Dance = 2,
}
public class CAnt : MonoBehaviour
{
    [SerializeField] protected AntDir antDir = AntDir.Right;

    [SerializeField] protected Vector2Int location = new Vector2Int(1, 1);
    [SerializeField] protected Vector2Int nextLocation = new Vector2Int(1, 1);
    [SerializeField] protected Vector2 locationPos = new Vector2();

    [SerializeField] protected Vector2 nextLocationPos = new Vector2();

    protected List<AntDir> antDirList = new List<AntDir>();

    protected Animator antAni;
    protected Transform antAniTr;

    private Anima2D.SpriteMeshInstance[] spriteMeshInstances;

    private void Awake()
    {
        antAni = GetComponentInChildren<Animator>();
        antAniTr = antAni.transform;
        spriteMeshInstances = GetComponentsInChildren<Anima2D.SpriteMeshInstance>();
    }

    public virtual void Dance()
    {
        
    }

    public virtual void SetAni(AntAni antAni)
    {
        if (antAni != AntAni.Dance)
        {

            for (AntAni i = 0; i <= AntAni.Walk; i++)
            {
                if (antAni != AntAni.Dance)
                {
                    this.antAni.SetBool(i.ToString(), antAni == i);
                }
            }
        }


    }

    public void Order(int order)
    {
        for (int i = 0; i < spriteMeshInstances.Length; i++)
        {
            spriteMeshInstances[i].sortingOrder += order; 
        }
    }

    protected void SetAniDir()
    {
        if (antDir == AntDir.Left)
        {
            if (antAniTr.localScale.x < 0)
            {
                antAniTr.localScale = new Vector3(antAniTr.localScale.x * -1, antAniTr.localScale.y);
            }
        }
        else if (antDir == AntDir.Right)
        {
            if (antAniTr.localScale.x > 0)
            {
                antAniTr.localScale = new Vector3(antAniTr.localScale.x * -1, antAniTr.localScale.y);
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //location = new Vector2Int(1, 8);

        //locationPos.x = -8.5f + location.x;
        //locationPos.y = -4.5f + location.y;
        //SetNextPos();
    }

    public Vector2Int GetLocation()
    {
        return location;
    }

    public Vector2Int GetNextLocation()
    {
        return nextLocation; ;
    }

    public virtual void SetAnt(Vector2Int enterAnt)
    {
    }


    // Update is called once per frame
    protected virtual void Update()
    {

    }

    protected Vector3 GetDir()
    {
        return GetAntDirToVector(antDir);
    }

    private Vector3 GetAntDirToVector(AntDir antDir)
    {
        switch (antDir)
        {
            case AntDir.Up:
            default:
                return Vector3.up;
            case AntDir.Down:
                return Vector3.down;
            case AntDir.Right:
                return Vector3.right;
            case AntDir.Left:
                return Vector3.left;
        }
    }

    protected virtual void LocationChange()
    {
        TileManager.Tile tile = TileManager.Tile.공백;
        switch (antDir)
        {
            case AntDir.Up:
            default:
                if (transform.position.y >= nextLocationPos.y)
                {
                    location.y++;
                    locationPos.y++;

                    //Debug.Log("Up " + location + new Vector2Int(location.x, location.y + 1));

                    if (TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x, location.y + 1)))
                    {
                        tile = TileManager.instance.TileCheck(new Vector2Int(location.x, location.y + 1));
                        //Debug.Log(tile);
                        if (tile == TileManager.Tile.응가)
                        {
                            transform.position = locationPos;
                            antDir = AntDir.Down;
                            SetAniDir();
                            SetNextPos();
                        }
                        else
                        {
                            //Debug.Log("UP  커브");
                            transform.position = locationPos;
                            SetAntDirList();
                            CheckNextBlock();
                        }
                    }
                    else
                    {
                        transform.position = locationPos;
                        SetAntDirList();
                        CheckNextBlock();
                    }



                }
                break;
            case AntDir.Down:
                if (transform.position.y <= nextLocationPos.y)
                {
                    location.y--;
                    locationPos.y--;
                    //Debug.Log("Down " + location + new Vector2Int(location.x, location.y - 1));

                    if (TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x, location.y - 1)))
                    {
                        tile = TileManager.instance.TileCheck(new Vector2Int(location.x, location.y - 1));

                        if (tile == TileManager.Tile.응가)
                        {
                            transform.position = locationPos;
                            antDir = AntDir.Up;
                            SetAniDir();
                            SetNextPos();
                        }
                        else
                        {
                            //Debug.Log("Down  커브");
                            transform.position = locationPos;
                            SetAntDirList();
                            CheckNextBlock();
                        }
                    }
                    else
                    {
                        transform.position = locationPos;
                        SetAntDirList();
                        CheckNextBlock();
                    }

                }
                break;
            case AntDir.Right:
                if (transform.position.x >= nextLocationPos.x)
                {
                    location.x++;
                    locationPos.x++;
                    //Debug.Log("Right " + location + new Vector2Int(location.x + 1, location.y));
                    if (TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x + 1, location.y)))
                    {
                        tile = TileManager.instance.TileCheck(new Vector2Int(location.x + 1, location.y));
                       
                        if (tile == TileManager.Tile.응가)
                        {
                            transform.position = locationPos;
                            antDir = AntDir.Left;
                            SetAniDir();
                            SetNextPos();
                        }
                        else
                        {
                            //Debug.Log("Right  커브");
                            transform.position = locationPos;
                            SetAntDirList();
                            CheckNextBlock();
                        }
                    }
                    else
                    {
                        transform.position = locationPos;
                        SetAntDirList();
                        CheckNextBlock();
                    }


                }
                break;
            case AntDir.Left:
                if (transform.position.x <= nextLocationPos.x)
                {
                    location.x--;
                    locationPos.x--;
                    //Debug.Log("Left " + location + new Vector2Int(location.x - 1, location.y) +
                        //TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x - 1, location.y)));
                    if (TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x - 1, location.y)))
                    {
                        //Debug.Log("여기 체크");
                        tile = TileManager.instance.TileCheck(new Vector2Int(location.x - 1, location.y));
                        //Debug.Log(tile);
                        if (tile == TileManager.Tile.응가)
                        {
                            transform.position = locationPos;
                            antDir = AntDir.Right;
                            SetAniDir();
                            SetNextPos();
                        }
                        else
                        {
                            //Debug.Log("LEFT  커브");
                            transform.position = locationPos;
                            SetAntDirList();
                            CheckNextBlock();
                        }
                    }
                    else
                    {
                        transform.position = locationPos;
                        SetAntDirList();
                        CheckNextBlock();
                    }


                }
                break;
        }
    }

    protected virtual void SetAntDirList()
    {
        int ran = Random.Range(0, 10);
        antDirList.Clear();
        
        if(ran < 8)
        {
            antDirList.Add(antDir);
        }

        switch (antDir)
        {
            case AntDir.Up:
            default:
                

                ran = Random.Range(0, 2);

                if (ran == 0)
                {
                    antDirList.Add(AntDir.Left);
                    antDirList.Add(AntDir.Right);
                    antDirList.Add(AntDir.Down);
                }
                else
                {
                    antDirList.Add(AntDir.Right);
                    antDirList.Add(AntDir.Left);
                    antDirList.Add(AntDir.Down);
                }
                break;
            case AntDir.Down:
                ran = Random.Range(0, 2);
                if (ran == 0)
                {
                    antDirList.Add(AntDir.Left);
                    antDirList.Add(AntDir.Right);
                    antDirList.Add(AntDir.Up);
                }
                else
                {
                    antDirList.Add(AntDir.Right);
                    antDirList.Add(AntDir.Left);
                    antDirList.Add(AntDir.Up);
                }
                break;
            case AntDir.Right:
                ran = Random.Range(0, 2);
                if (ran == 0)
                {
                    antDirList.Add(AntDir.Up);
                    antDirList.Add(AntDir.Down);
                    antDirList.Add(AntDir.Left);
                }
                else
                {
                    antDirList.Add(AntDir.Down);
                    antDirList.Add(AntDir.Up);
                    antDirList.Add(AntDir.Left);
                }
                break;
            case AntDir.Left:
                ran = Random.Range(0, 2);
                if (ran == 0)
                {
                    antDirList.Add(AntDir.Up);
                    antDirList.Add(AntDir.Down);
                    antDirList.Add(AntDir.Right);
                }
                else
                {
                    antDirList.Add(AntDir.Down);
                    antDirList.Add(AntDir.Up);
                    antDirList.Add(AntDir.Right);
                }
                break;
        }
    }


    protected virtual void CheckNextBlock()
    {
        if(antDirList.Count >=4)
        {
            if (Checker(antDirList[0]))
            {
                this.antDir = antDirList[0];
            }
            else if (Checker(antDirList[1]))
            {
                this.antDir = antDirList[1];
            }
            else if (Checker(antDirList[2]))
            {
                this.antDir = antDirList[2];
            }
            else
            {
                this.antDir = antDirList[3];
            }
        }
        else
        {
            if (Checker(antDirList[0]))
            {
                this.antDir = antDirList[0];
            }
            else if (Checker(antDirList[1]))
            {
                this.antDir = antDirList[1];
            }
            else
            {
                this.antDir = antDirList[2];
            }
        }

        SetAniDir();
        SetNextPos();
    }

    protected virtual void SetNextPos()
    {
        switch (antDir)
        {
            case AntDir.Up:
            default:
                nextLocationPos.x = locationPos.x;
                nextLocationPos.y = locationPos.y + 1f;

                nextLocation = location;
                nextLocation.y++;

                break;
            case AntDir.Down:
                nextLocationPos.x = locationPos.x;
                nextLocationPos.y = locationPos.y - 1f;

                nextLocation = location;
                nextLocation.y--;
                break;
            case AntDir.Right:
                nextLocationPos.y = locationPos.y;
                nextLocationPos.x = locationPos.x + 1f;
                nextLocation = location;
                nextLocation.x++;
                break;
            case AntDir.Left:
                nextLocationPos.y = locationPos.y;
                nextLocationPos.x = locationPos.x - 1f;
                nextLocation = location;
                nextLocation.x--;
                break;
        }
        SetAniDir();
    }

    protected bool Checker(AntDir antDir)
    {
        switch (antDir)
        {
            case AntDir.Up:
            default:
                if (TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x, location.y + 1)))
                {
                    //Debug.Log(TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x, location.y + 1)) + " 위");
                    return TileMoveOnCheck(TileManager.instance.TileCheck(new Vector2Int(location.x, location.y + 1)));
                }
                return false;
            case AntDir.Down:
                if (TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x, location.y - 1)))
                {
                    //Debug.Log(TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x, location.y - 1)) + " 아래");
                    return TileMoveOnCheck(TileManager.instance.TileCheck(new Vector2Int(location.x, location.y - 1)));
                }
                return false;
            case AntDir.Right:
                if (TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x + 1, location.y)))
                {
                    //Debug.Log(TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x + 1, location.y)) + " 오른쪽");
                    return TileMoveOnCheck(TileManager.instance.TileCheck(new Vector2Int(location.x + 1, location.y)));
                }
                return false;
            case AntDir.Left:
                if (TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x - 1, location.y)))
                {
                    //Debug.Log(TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x - 1, location.y)) + " 왼쪽");
                    return TileMoveOnCheck(TileManager.instance.TileCheck(new Vector2Int(location.x - 1, location.y)));
                }
                return false;
        }
    }

    public bool TileMoveOnCheck(TileManager.Tile tile)
    {
        switch (tile)
        {
            case TileManager.Tile.공백:
            case TileManager.Tile.나뭇잎:
            case TileManager.Tile.응가:
            case TileManager.Tile.바위:
                return false;
            default:
                return true;
        }
    }
}
