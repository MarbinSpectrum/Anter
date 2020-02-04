using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CNormalAnt : CAnt
{
    [SerializeField] private float speed;

    private bool slow = false;
    private bool highSpeed = false;
    public override void SetAnt(Vector2Int enterAnt)
    {
        location = enterAnt;
        locationPos = TileManager.instance.GridPos(location);
        transform.position = locationPos;
        nextLocationPos = locationPos;
        SetAni(AntAni.Walk);
        FirstTarget();
    }

    public override void Dance()
    {
        this.antAni.SetTrigger("Dance");
    }

    protected override void Update()
    {
        if(!antAni.GetCurrentAnimatorStateInfo(0).IsName("Normal_Dance"))
        {
            transform.Translate(GetDir() * speed * Time.deltaTime * (slow? 0.5f : 1f) * (highSpeed?1.5f:1f));
            LocationChange();
        }

    }
    private void FirstTarget()
    {
        TileManager.Tile tile = TileManager.Tile.공백;
        switch (antDir)
        {
            case AntDir.Up:
            default:
                if (transform.position.y >= nextLocationPos.y)
                {
                    if (TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x, location.y + 1)))
                    {
                        tile = TileManager.instance.TileCheck(new Vector2Int(location.x, location.y + 1));
                        if (TileMoveOnCheck(tile))
                        {
                            SetNextPos();
                            Debug.Log("UP  직");
                        }
                        else if (tile == TileManager.Tile.응가)
                        {
                            transform.position = locationPos;
                            antDir = AntDir.Down;
                            SetAniDir();
                            SetNextPos();
                        }
                        else
                        {
                            Debug.Log("UP  커브");
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
                    if (TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x, location.y - 1)))
                    {
                        tile = TileManager.instance.TileCheck(new Vector2Int(location.x, location.y - 1));
                        if (TileMoveOnCheck(tile))
                        {
                            Debug.Log("Down  직");
                            SetNextPos();
                        }
                        else if (tile == TileManager.Tile.응가)
                        {
                            transform.position = locationPos;
                            antDir = AntDir.Up;
                            SetAniDir();
                            SetNextPos();
                        }
                        else
                        {
                            Debug.Log("Down  커브");
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

                    if (TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x + 1, location.y)))
                    {
                        tile = TileManager.instance.TileCheck(new Vector2Int(location.x + 1, location.y));
                        if (TileMoveOnCheck(tile))
                        {
                            Debug.Log("Right  직");
                            SetNextPos();
                        }
                        else if (tile == TileManager.Tile.응가)
                        {
                            transform.position = locationPos;
                            antDir = AntDir.Left;
                            SetAniDir();
                            SetNextPos();
                        }
                        else
                        {
                            Debug.Log("Right  커브");
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
                    if (TileManager.instance.DirtWallCheck(location, new Vector2Int(location.x - 1, location.y)))
                    {
                        tile = TileManager.instance.TileCheck(new Vector2Int(location.x - 1, location.y));
                        if (TileMoveOnCheck(tile))
                        {
                            Debug.Log("LEFT  직");
                            SetNextPos();
                        }
                        else if (tile == TileManager.Tile.응가)
                        {
                            transform.position = locationPos;
                            antDir = AntDir.Right;
                            SetAniDir();
                            SetNextPos();
                        }
                        else
                        {
                            Debug.Log("LEFT  커브");
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


    protected override void SetNextPos()
    {
        base.SetNextPos();

        if (TileManager.instance.GetTile(TileManager.instance.tileItem, nextLocation) == TileManager.Tile.거미줄
                    || TileManager.instance.GetTile(TileManager.instance.tileItem, location) == TileManager.Tile.거미줄)
        {
            slow = true;
        }
        else
        {
            slow = false;
        }

        if (TileManager.instance.GetTile(TileManager.instance.tileItem,nextLocation) == TileManager.Tile.마늘
            || TileManager.instance.GetTile(TileManager.instance.tileItem, location) == TileManager.Tile.마늘)
        {
            highSpeed = true;
        }
        else
        {
            highSpeed = false;
        }
    }
}
