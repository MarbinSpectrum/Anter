using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMineAnt : CAnt
{
    private float actionTime = 0f;
    [SerializeField] private float oneTime = 2f;

    private bool mineing = false;

    private int moveingCount = 0;

    private bool nextMove = false;

    public override void SetAnt(Vector2Int enterAnt)
    {
        location = enterAnt;
        locationPos = TileManager.instance.GridPos(location);
        transform.position = locationPos;
        nextLocationPos = locationPos;
        SetAni(AntAni.Walk);
        FirstTarget();
        actionTime = 0f;
    }

    public override void SetAni(AntAni antAni)
    {
        for (AntAni i = 0; i <= AntAni.Dance; i++)
        {
            this.antAni.SetBool(i.ToString(), antAni == i);
        }

    }

    protected override void Update()
    {
        if (actionTime >= oneTime)
        {
            transform.position = nextLocationPos;
            LocationChange();
            actionTime = 0f;
        }
        else
        {
            actionTime += Time.deltaTime;
            if(!mineing)
            {
                transform.position = Vector3.Lerp(locationPos, nextLocationPos, actionTime / oneTime);
            }
            else
            {
                if(actionTime >= oneTime)
                {
                    TileManager.instance.DirtWallOpen(location, nextLocation);
                    if(TileManager.instance.GetTile(nextLocation) == TileManager.Tile.공백)
                        TileManager.instance.SetTile(nextLocation, TileManager.Tile.흙);

                    SoundManager.instance.se06shovel.Play();

                    actionTime = 0f;
                    mineing = false;
                    nextMove = true;
                    SetAni(AntAni.Walk);
                    FirstTarget();
                }
            }

        }

    }

    private void FirstTarget()
    {
        TileManager.Tile tile = TileManager.Tile.공백;
        AntDir backDir = AntDir.Up;
        bool dirtWall = false;
        switch (antDir)
        {
            case AntDir.Up:
            default:
                if (transform.position.y >= nextLocationPos.y)
                {
                    tile = TileManager.instance.GetTile(new Vector2Int(location.x, location.y + 1));
                    backDir = AntDir.Down;
                    dirtWall = TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x, location.y + 1));
                }
                break;
            case AntDir.Down:
                if (transform.position.y <= nextLocationPos.y)
                {
                    tile = TileManager.instance.GetTile(new Vector2Int(location.x, location.y - 1));
                    backDir = AntDir.Up;
                    dirtWall = TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x, location.y - 1));
                }
                break;
            case AntDir.Right:
                if (transform.position.x >= nextLocationPos.x)
                {
                    tile = TileManager.instance.GetTile(new Vector2Int(location.x + 1, location.y));
                    backDir = AntDir.Left;
                    dirtWall = TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x + 1, location.y));
                }
                break;
            case AntDir.Left:
                if (transform.position.x <= nextLocationPos.x)
                {
                    tile = TileManager.instance.GetTile(new Vector2Int(location.x - 1, location.y));
                    backDir = AntDir.Right;
                    dirtWall = TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x - 1, location.y));
                }
                break;
        }
        transform.position = locationPos;
        if (dirtWall)
        {
            if (tile == TileManager.Tile.응가)
            {
                antDir = backDir;
                SetNextPos();
            }
            else
            {
                SetAntDirList();
                CheckNextBlock();
            }
        }
        else
        {
            SetAntDirList();
            CheckNextBlock();
        }
    }


    protected override void LocationChange()
    {
        TileManager.Tile tile = TileManager.Tile.공백;
        AntDir backDir = AntDir.Up;
        bool dirtWall = false;
        switch (antDir)
        {
            case AntDir.Up:
            default:
                if (transform.position.y >= nextLocationPos.y)
                {
                    location.y++;
                    locationPos.y++;
                    tile = TileManager.instance.GetTile(new Vector2Int(location.x, location.y + 1));
                    backDir = AntDir.Down;
                    dirtWall = TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x, location.y + 1));
                }
                break;
            case AntDir.Down:
                if (transform.position.y <= nextLocationPos.y)
                {
                    location.y--;
                    locationPos.y--;
                    tile = TileManager.instance.GetTile(new Vector2Int(location.x, location.y - 1));
                    backDir = AntDir.Up;
                    dirtWall = TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x, location.y - 1));
                }
                break;
            case AntDir.Right:
                if (transform.position.x >= nextLocationPos.x)
                {
                    location.x++;
                    locationPos.x++;
                    tile = TileManager.instance.GetTile(new Vector2Int(location.x + 1, location.y));
                    backDir = AntDir.Left;
                    dirtWall = TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x + 1, location.y));
                }
                break;
            case AntDir.Left:
                if (transform.position.x <= nextLocationPos.x)
                {
                    location.x--;
                    locationPos.x--;
                    tile = TileManager.instance.GetTile(new Vector2Int(location.x - 1, location.y));
                    backDir = AntDir.Right;
                    dirtWall = TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x - 1, location.y));
                }
                break;
        }
        transform.position = locationPos;

        Vector2Int honeyPos = TileManager.instance.GetHoneyPos(location);

        //Debug.Log("꿀 위치 " + honeyPos);

        if(dirtWall)
        {
            if (tile == TileManager.Tile.응가)
            {
                antDir = backDir;
                SetNextPos();
            }
            else
            {
                if (honeyPos.x <= -100f)
                {
                    SetAntDirList();
                }
                else
                {
                    SetHoneyAntDirList(honeyPos);
                }
                CheckNextBlock();
            }
        }
        else
        {
            if (honeyPos.x <= -100f)
            {
                SetAntDirList();
            }
            else
            {
                SetHoneyAntDirList(honeyPos);
            }
            CheckNextBlock();
        }

    }



    protected override void CheckNextBlock()
    {
        for (int i = 0; i < antDirList.Count; i++)
        {
            /*
            if (Checker(antDirList[i]))
            {
                this.antDir = antDirList[i];
                nextMove = false;
                moveingCount++;
                SetAni(AntAni.Walk);
                SetNextPos();
                break;
            }
            else if (MineCheck(antDirList[i]))
            {
                mineing = true;
                SetAni(AntAni.Dance);
                this.antDir = antDirList[i];
                SetAniDir();
                moveingCount = 0;
                break;
            }
            */
            int ran = Random.Range(0, 10);
            if(ran >= 7)
            {
                if (Checker(antDirList[i]))
                {
                    this.antDir = antDirList[i];
                    nextMove = false;
                    moveingCount++;
                    SetAni(AntAni.Walk);
                    SetNextPos();
                    break;
                }
                else if (MineCheck(antDirList[i]))
                {
                    mineing = true;
                    SetAni(AntAni.Dance);
                    this.antDir = antDirList[i];
                    SetAniDir();
                    moveingCount = 0;
                    break;
                }
            }
            else
            {
                if (MineCheck(antDirList[i]))
                {
                    mineing = true;
                    SetAni(AntAni.Dance);
                    this.antDir = antDirList[i];
                    SetAniDir();
                    moveingCount = 0;
                    break;
                }
                else if (Checker(antDirList[i]))
                {
                    this.antDir = antDirList[i];
                    nextMove = false;
                    moveingCount++;
                    SetAni(AntAni.Walk);
                    SetNextPos();
                    break;
                }
            }
        }
        
    }

    private bool MineCheck(AntDir antDir)
    {
        //TileManager.Tile tile = TileManager.Tile.공백;
        switch (antDir)
        {
            case AntDir.Up:
            default:
                if (!TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x, location.y + 1)))
                {
                    if (TileManager.instance.DigCheck(new Vector2Int(location.x, location.y + 1)))
                    {
                        if(!CAntManager.Instance.NextAntLocationCheck(new Vector2Int(location.x, location.y + 1)))
                        {
                            nextLocation = location;
                            nextLocation.y++;
                            return true;
                        }

                    }
                }
                return false;
            case AntDir.Down:
                if(!TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x, location.y - 1)))
                {
                    //tile = TileManager.instance.GetTile(new Vector2Int(location.x, location.y - 1));
                    if (TileManager.instance.DigCheck(new Vector2Int(location.x, location.y - 1)))
                    {
                        if(!CAntManager.Instance.NextAntLocationCheck(new Vector2Int(location.x, location.y - 1)))
                        {
                            nextLocation = location;
                            nextLocation.y--;
                            return true;
                        }
                    }
                }
                return false;
            case AntDir.Right:
                if(!TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x + 1, location.y)))
                {
                   // tile = TileManager.instance.GetTile(new Vector2Int(location.x + 1, location.y));
                    if (TileManager.instance.DigCheck(new Vector2Int(location.x + 1, location.y)))
                    {
                        if(!CAntManager.Instance.NextAntLocationCheck(new Vector2Int(location.x + 1, location.y)))
                        {
                            nextLocation = location;
                            nextLocation.x++;
                            return true;
                        }
                    }
                }
                return false;
            case AntDir.Left:
                if(!TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x - 1, location.y)))
                {
                   // tile = TileManager.instance.GetTile(new Vector2Int(location.x - 1, location.y));
                    if (TileManager.instance.DigCheck(new Vector2Int(location.x - 1, location.y)))
                    {
                        if(!CAntManager.Instance.NextAntLocationCheck(new Vector2Int(location.x - 1, location.y)))
                        {
                            nextLocation = location;
                            nextLocation.x--;
                            return true;
                        }
                    }
                }
                return false;
        }
    }

    protected override void SetAntDirList()
    {
        int ran = Random.Range(0, 2);
        antDirList.Clear();
        if(nextMove)
        {
            antDirList.Add(antDir);
        }

        bool horizontalDir = HorizontalDirRate();

        if(antDir == AntDir.Up || antDir == AntDir.Down)
        {
            if (horizontalDir)
            {
                if (ran == 0)
                {
                    antDirList.Add(AntDir.Left);
                    antDirList.Add(AntDir.Right);
                }
                else
                {
                    antDirList.Add(AntDir.Left);
                    antDirList.Add(AntDir.Right);
                }
                if(!nextMove)
                {
                    antDirList.Add(antDir == AntDir.Up? AntDir.Up : AntDir.Down);
                }
                antDirList.Add(antDir == AntDir.Up ? AntDir.Down : AntDir.Up);
            }
            else
            {
                if(!nextMove)
                {
                    antDirList.Add(antDir == AntDir.Up ? AntDir.Up : AntDir.Down);
                }

                if (ran == 0)
                {
                    antDirList.Add(AntDir.Left);
                    antDirList.Add(AntDir.Right);
                }
                else
                {
                    antDirList.Add(AntDir.Left);
                    antDirList.Add(AntDir.Right);
                }

                antDirList.Add(antDir == AntDir.Up ? AntDir.Down : AntDir.Up);

            }
        }
        else
        {
            if (horizontalDir)
            {
                if(!nextMove)
                {
                    antDirList.Add(antDir == AntDir.Right ? AntDir.Right : AntDir.Left);
                }
                if (ran == 0)
                {
                    antDirList.Add(AntDir.Up);
                    antDirList.Add(AntDir.Down);
                }
                else
                {
                    antDirList.Add(AntDir.Down);
                    antDirList.Add(AntDir.Up);
                }
                antDirList.Add(antDir == AntDir.Right ? AntDir.Left : AntDir.Right);

            }
            else
            {
                if (ran == 0)
                {
                    antDirList.Add(AntDir.Up);
                    antDirList.Add(AntDir.Down);
                }
                else
                {
                    antDirList.Add(AntDir.Down);
                    antDirList.Add(AntDir.Up);
                }
                if(!nextMove)
                {
                    antDirList.Add(antDir == AntDir.Right ? AntDir.Right : AntDir.Left);
                }
                antDirList.Add(antDir == AntDir.Right ? AntDir.Left : AntDir.Right);

            }
        }
    }

    protected void SetHoneyAntDirList(Vector2Int honeyPos)
    {
        antDirList.Clear();

        int x = honeyPos.x - location.x;
        int y = honeyPos.y - location.y;

       // Debug.Log("거리 " + x + " " + y);

        if(Mathf.Abs(x) >= Mathf.Abs(y))
        {
            if(x > 0)
            {
                antDirList.Add(AntDir.Right);
                if(y > 0)
                {
                    antDirList.Add(AntDir.Up);
                    antDirList.Add(AntDir.Down);
                }
                else
                {
                    antDirList.Add(AntDir.Down);
                    antDirList.Add(AntDir.Up);
                }
                antDirList.Add(AntDir.Left);
            }
            else
            {
                antDirList.Add(AntDir.Left);
                if (y > 0)
                {
                    antDirList.Add(AntDir.Up);
                    antDirList.Add(AntDir.Down);
                }
                else
                {
                    antDirList.Add(AntDir.Down);
                    antDirList.Add(AntDir.Up);
                }
                antDirList.Add(AntDir.Right);
            }


        }
        else
        {
            if (y > 0)
            {
                antDirList.Add(AntDir.Up);
                if (x > 0)
                {
                    antDirList.Add(AntDir.Right);
                    antDirList.Add(AntDir.Left);
                }
                else
                {
                    antDirList.Add(AntDir.Left);
                    antDirList.Add(AntDir.Right);
                }
                antDirList.Add(AntDir.Down);
            }
            else
            {
                antDirList.Add(AntDir.Down);
                if (x > 0)
                {
                    antDirList.Add(AntDir.Right);
                    antDirList.Add(AntDir.Left);
                }
                else
                {
                    antDirList.Add(AntDir.Left);
                    antDirList.Add(AntDir.Right);
                }
                antDirList.Add(AntDir.Up);
            }

            
        }

    }

    private bool HorizontalDirRate()
    {
        return Random.Range(0, 100f) < 60f;
    }

    public override void Dance()
    {
    }
}
