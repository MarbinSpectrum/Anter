using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMineAnt : CAnt
{
    private float actionTime = 0f;
    [SerializeField] private float oneTime = 2f;

    private bool mineing = false;

    private int moveingCount = 0;

    public override void SetAnt(Vector2Int enterAnt, AntDir antDir)
    {
        location = enterAnt;
        locationPos = TileManager.instance.GridPos(location);
        this.antDir = antDir;
        transform.position = locationPos;
        nextLocationPos = locationPos;
        SetAni(AntAni.Walk);
        FirstTarget();
        actionTime = 0f;
    }

    public override void SetAni(AntAni antAni)
    {
        for (AntAni i = 0; i <= AntAni.Walk; i++)
        {
            if (antAni == AntAni.Dance)
            {
                continue;
            }
            else
            {
                this.antAni.SetBool(i.ToString(), antAni == i);
            }
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
                    TileManager.instance.SetTile(nextLocation, TileManager.Tile.흙);
                    actionTime = 0f;
                    mineing = false;
                    SetAni(AntAni.Walk);

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
            SetAntDirList();
            CheckNextBlock();
        }
        else if (tile == TileManager.Tile.응가)
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
        if(dirtWall)
        {
            SetAntDirList();
            CheckNextBlock();
        }
        else if(tile == TileManager.Tile.응가)
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

    protected override void CheckNextBlock()
    {
        for (int i = 0; i < antDirList.Count; i++)
        {
            if (Checker(antDirList[i]))
            {
                this.antDir = antDirList[i];
                moveingCount++;
                break;
            }
            else if (MineCheck(antDirList[i]))
            {
                mineing = true;
                SetAni(AntAni.Idle);
                this.antDir = antDirList[i];
                moveingCount = 0;
                break;
            }
            /*
            int ran = Random.Range(0, 10);
            if(ran < 8)
            {
                if (Checker(antDirList[i]))
                {
                    this.antDir = antDirList[i];
                    moveingCount++;
                    break;
                }
                else if (MineCheck(antDirList[i]))
                {
                    mineing = true;
                    SetAni(AntAni.Idle);
                    this.antDir = antDirList[i];
                    moveingCount = 0;
                    break;
                }
            }
            else
            {
                if (MineCheck(antDirList[i]))
                {
                    mineing = true;
                    SetAni(AntAni.Idle);
                    this.antDir = antDirList[i];
                    moveingCount = 0;
                    break;
                }
                else if (Checker(antDirList[i]))
                {
                    this.antDir = antDirList[i];
                    moveingCount++;
                    break;
                }
            }
            */
        }
        SetNextPos();
    }

    private bool MineCheck(AntDir antDir)
    {
        TileManager.Tile tile = TileManager.Tile.공백;
        switch (antDir)
        {
            case AntDir.Up:
            default:
                if (!TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x, location.y + 1)))
                {
                    Debug.Log("땅판다 Up");
                   // tile = TileManager.instance.GetTile(new Vector2Int(location.x, location.y + 1));
                    if (TileManager.instance.DigCheck(new Vector2Int(location.x, location.y + 1)))
                    {
                        nextLocation = location;
                        nextLocation.y++;
                        return true;
                    }
                }
                return false;
            case AntDir.Down:
                if(!TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x, location.y - 1)))
                {
                    Debug.Log("땅판다 Down");
                    //tile = TileManager.instance.GetTile(new Vector2Int(location.x, location.y - 1));
                    if (TileManager.instance.DigCheck(new Vector2Int(location.x, location.y - 1)))
                    {
                        nextLocation = location;
                        nextLocation.y--;
                        return true;
                    }
                }
                return false;
            case AntDir.Right:
                if(!TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x + 1, location.y)))
                {
                    Debug.Log("땅판다 Right");
                   // tile = TileManager.instance.GetTile(new Vector2Int(location.x + 1, location.y));
                    if (TileManager.instance.DigCheck(new Vector2Int(location.x + 1, location.y)))
                    {
                        nextLocation = location;
                        nextLocation.x++;
                        return true;
                    }
                }
                return false;
            case AntDir.Left:
                if(!TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x - 1, location.y)))
                {
                    Debug.Log("땅판다 Left");
                   // tile = TileManager.instance.GetTile(new Vector2Int(location.x - 1, location.y));
                    if (TileManager.instance.DigCheck(new Vector2Int(location.x - 1, location.y)))
                    {
                        nextLocation = location;
                        nextLocation.x--;
                        return true;
                    }
                }
                return false;
        }
    }

    protected override void SetAntDirList()
    {
        int ran = Random.Range(0, 2);
        antDirList.Clear();
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

                antDirList.Add(antDir == AntDir.Up? AntDir.Up : AntDir.Down);
                antDirList.Add(antDir == AntDir.Up ? AntDir.Down : AntDir.Up);
            }
            else
            {
                antDirList.Add(antDir == AntDir.Up ? AntDir.Up : AntDir.Down);

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
                antDirList.Add(antDir == AntDir.Right ? AntDir.Right : AntDir.Left);
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
                antDirList.Add(antDir == AntDir.Right ? AntDir.Right : AntDir.Left);
                antDirList.Add(antDir == AntDir.Right ? AntDir.Left : AntDir.Right);

            }
        }
    }

    private bool HorizontalDirRate()
    {
        return Random.Range(0, 100f) < 60f;
    }

}
