using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static int mapW = 21; // 맵가로
    public static int mapH = 10;  // 맵세로

    public static TileManager instance;

    public enum Tile
    {
        공백, 시작, 끝, 흙, 데드존, 바위, 꿀, 나뭇잎, 꽃줄기, 마늘, 거미줄, 방울, 응가, 애벌레
    };

    public struct GroundWall
    {
        public bool left;
        public bool right;
        public bool up;
        public bool down;
    }
    //true면갈수있는길!!

    public Tilemap tileMap;
    public Tilemap tileItem;
    public Tilemap tileSelect;

    public Tile[,] tileArray = new Tile[mapW, mapH];
    public float[,] colArray = new float[mapW, mapH];
    public float[,] shiledArray = new float[mapW, mapH];
    public GroundWall[,] dirtWall = new GroundWall[mapW, mapH];
    bool[,] hasTorch = new bool[mapW, mapH];
    public GameObject[] torch;


    [SerializeField] private GameObject ant;

    #region[타일들]
    [Header("빈타일")]
    public TileBase emptyTile;

    [Header("시작타일")]
    public TileBase startTile;

    [Header("끝타일")]
    public TileBase endTile;

    [Header("데드존타일")]
    public TileBase deadTile;

    [Header("위험구역타일")]
    public TileBase dangerTile;

    [Header("바위타일")]
    public TileBase[] rockTile;
    public TileBase donMoverockTile;

    [Header("흙타일")]
    public TileBase dirtTile;
    public TileBase[] roadTile;

    [Header("꿀타일")]
    public TileBase honeyTile;

    [Header("나뭇잎타일")]
    public TileBase[] leafTile;
    public bool soloLeaf;

    [Header("꽃줄기타일")]
    public TileBase[] flowerStemTile;

    [Header("마늘타일")]
    public TileBase[] garlicTile;

    [Header("거미줄타일")]
    public TileBase[] spiderWebTile;

    [Header("방울타일")]
    public TileBase[] bubbleTile;

    [Header("응가타일")]
    public TileBase[] pooTile;

    [Header("애벌레타일")]
    public TileBase larvaTile;
    #endregion

    #region[Awkae]
    private void Awake()
    {
        tileSelect.transform.parent.gameObject.SetActive(true);
        instance = this;
    }
    #endregion

    #region[Start]
    void Start()
    {
        Init();
    }
    #endregion

    #region[Update]
    private void Update()
    {
        UpdateObjectTile();
        RoadTileUpDate();
    }
    #endregion

    #region[Init]
    void Init()
    {
        for (int x = 0; x < mapW; x++)
            for (int y = 0; y < mapH; y++)
                SetObjectTile(new Vector2Int(x, y),false);

        for (int x = 0; x < mapW; x++)
            for (int y = 0; y < mapH; y++)
                SetObjectTile(new Vector2Int(x, y),true);
    }
    #endregion

    #region[SetTile]
    public void SetTile(Vector2Int v, Tile tileType)
    {
        SetTile(tileMap, new Vector3Int(v.x, v.y, 0), tileType);
    }

    public void SetTile(Vector3Int v, Tile tileType)
    {
        SetTile(new Vector3Int(v.x, v.y, 0), tileType);
    }

    public void SetTile(Tilemap tMap, Vector2Int v, Tile tileType)
    {
        SetTile(tMap, new Vector3Int(v.x, v.y, 0), tileType);
    }

    public void SetTile(Tilemap tMap, Vector3Int v, Tile tileType)
    {
        switch (tileType)
        {
            case Tile.공백:
                if (RectIn(v))
                    tMap.SetTile(v, emptyTile);
                break;
            case Tile.시작:
                if (RectIn(v))
                    tMap.SetTile(v, startTile);
                break;
            case Tile.끝:
                if (RectIn(v))
                    tMap.SetTile(v, endTile);
                break;
            case Tile.흙:
                if (RectIn(v))
                    tMap.SetTile(v, dirtTile);
                break;
            case Tile.데드존:
                if (RectIn(v))
                    tMap.SetTile(v, deadTile);
                break;
            case Tile.바위:
                if (RectIn(v))
                {
                    if(tMap == tileItem)
                        tMap.SetTile(v, rockTile[UnityEngine.Random.Range(0, rockTile.Length)]);
                    else if (tMap == tileSelect)
                        tMap.SetTile(v, rockTile[0]);
                }
                break;
            case Tile.꿀:
                if (RectIn(v))
                    tMap.SetTile(v, honeyTile);
                break;
            case Tile.나뭇잎:
                if (!soloLeaf)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector3Int emp = v + new Vector3Int(i % 2, -(i / 2), 0);
                        if (RectIn(emp))
                            tMap.SetTile(emp, leafTile[i]);
                    }
                }
                else if (RectIn(v))
                    tMap.SetTile(v, leafTile[4]);
                break;
            case Tile.꽃줄기:
                for (int i = 0; i < 2; i++)
                {
                    Vector3Int emp = v + new Vector3Int(i, 0, 0);
                    if (RectIn(emp))
                        tMap.SetTile(emp, flowerStemTile[i]);
                }
                break;
            case Tile.마늘:
                for (int i = 0; i < 4; i++)
                {
                    Vector3Int emp = v + new Vector3Int(i % 2, -(i / 2), 0);
                    if (RectIn(emp))
                        tMap.SetTile(emp, garlicTile[i]);
                }
                break;
            case Tile.거미줄:
                for (int i = 0; i < 4; i++)
                {
                    Vector3Int emp = v + new Vector3Int(i % 2, -(i / 2), 0);
                    if (RectIn(emp))
                        tMap.SetTile(emp, spiderWebTile[i]);
                }
                break;
            case Tile.방울:
                for (int i = 0; i < 4; i++)
                {
                    Vector3Int emp = v + new Vector3Int(i % 2, -(i / 2), 0);
                    if (RectIn(emp))
                        tMap.SetTile(emp, bubbleTile[i]);
                }
                break;
            case Tile.응가:
                for (int i = 0; i < 2; i++)
                {
                    Vector3Int emp = v + new Vector3Int(i, 0, 0);
                    if (RectIn(emp))
                        tMap.SetTile(emp, pooTile[i]);
                }
                break;
            case Tile.애벌레:
                if (RectIn(v))
                    tMap.SetTile(v, larvaTile);
                break;
        }
    }

    bool RectIn(Vector3Int v)
    {
        return RectIn(v.x, v.y);
    }

    bool RectIn(int x, int y)
    {
        return (0 <= x && x < mapW && 0 <= y && y < mapH) || true;
    }
    #endregion

    #region[GetTile]
    public Tile GetTile(Vector2Int v)
    {
        return GetTile(new Vector3Int(v.x, v.y, 0));
    }

    public Tile GetTile(Tilemap tMap, Vector2Int v)
    {
        return GetTile(tMap, new Vector3Int(v.x, v.y, 0));
    }

    public Tile GetTile(Vector3Int v)
    {
        return GetTile(tileMap, new Vector3Int(v.x, v.y, 0));
    }

    public Tile GetTile(Tilemap tMap, Vector3Int v)
    {
        string s;
        try
        {
            s = tMap.GetTile(v).name;
        }
        catch { s = "Empty"; }

        return GetTile(s);
    }

    public Tile GetTile(string s)
    {
        switch (s)
        {
            case "Empty":
                return Tile.공백;
            case "Start":
                return Tile.시작;
            case "End":
                return Tile.끝;
            case "Dirt":
            case "Road0-0":
            case "Road0-1":
            case "Road0-2":
            case "Road0-3":
            case "Road1-0":
            case "Road1-1":
            case "Road2-0":
            case "Road2-1":
            case "Road2-2":
            case "Road2-3":
            case "Road3-0":
            case "Road3-1":
            case "Road3-2":
            case "Road3-3":
            case "Road4":
                return Tile.흙;
            case "DangerZone":
            case "DangerZone0":
            case "DangerZone1":
            case "DeadZone":
                return Tile.데드존;
            case "Rock":
            case "Rock0":
            case "Rock1":
            case "SRock":
                return Tile.바위;
            case "Honey":
                return Tile.꿀;
            case "Leaf":
            case "Leaf0":
            case "Leaf1":
            case "Leaf2":
            case "Leaf3":
                return Tile.나뭇잎;
            case "FlowerStem":
            case "FlowerStem0":
            case "FlowerStem1":
                return Tile.꽃줄기;
            case "Garlic":
            case "Garlic0":
            case "Garlic1":
            case "Garlic2":
            case "Garlic3":
                return Tile.마늘;
            case "SpiderWeb":
            case "SpiderWeb0":
            case "SpiderWeb1":
            case "SpiderWeb2":
            case "SpiderWeb3":
                return Tile.거미줄;
            case "Bubble":
            case "Bubble0":
            case "Bubble1":
            case "Bubble2":
            case "Bubble3":
                return Tile.방울;
            case "Poo":
            case "Poo0":
            case "Poo1":
            case "Poo2":
            case "Poo3":
                return Tile.응가;
            case "Larva":
                return Tile.애벌레;
        }
        return Tile.공백;
    }
    #endregion

    #region[타일의 이름출력]
    public string GetTileName(Tile tile)
    {
        switch (tile)
        {
            case Tile.공백:
                return "Empty";
            case Tile.시작:
                return "Start";
            case Tile.끝:
                return "End";
            case Tile.흙:
                return "Dirt";
            case Tile.데드존:
                return "DeadZone";
            case Tile.바위:
                return "Rock";
            case Tile.꿀:
                return "Honey";
            case Tile.나뭇잎:
                return "Leaf";
            case Tile.꽃줄기:
                return "FlowerStem";
            case Tile.마늘:
                return "Garlic";
            case Tile.거미줄:
                return "SpiderWeb";
            case Tile.방울:
                return "Bubble";
            case Tile.응가:
                return "Poo";
            case Tile.애벌레:
                return "Larva";
        }
        return "Empty";
    }
    #endregion

    #region[벽뚫기]
    public void DirtWallOpen(Vector2Int v1, Vector2Int v2)
    {
        Vector2Int v = v2 - v1;
        if (v == Vector2.left)
        {
            dirtWall[v1.x, v1.y].left = true;
            if (v2.x >= 0 && v2.x < mapW && v2.y >= 0 && v2.y < mapH)
                dirtWall[v2.x, v2.y].right = true;
        }
        else if (v == Vector2.right)
        {
            dirtWall[v1.x, v1.y].right = true;
            if (v2.x >= 0 && v2.x < mapW && v2.y >= 0 && v2.y < mapH)
                dirtWall[v2.x, v2.y].left = true;
        }
        else if (v == Vector2.up)
        {
            dirtWall[v1.x, v1.y].up = true;
            if (v2.x >= 0 && v2.x < mapW && v2.y >= 0 && v2.y < mapH)
                dirtWall[v2.x, v2.y].down = true;
        }
        else if (v == Vector2.down)
        {
            dirtWall[v1.x, v1.y].down = true;
            if (v2.x >= 0 && v2.x < mapW && v2.y >= 0 && v2.y < mapH)
                dirtWall[v2.x, v2.y].up = true;
        }
    }
    #endregion

    #region[마우스 좌표를 그리드좌표로 변경]
    public Vector2Int mousePosToGridPos(Vector2 v)
    {
        v = Camera.main.ScreenToWorldPoint(v);
        Vector2 offset = new Vector2(tileMap.transform.position.x, tileMap.transform.position.y);
        v -= offset;
        return new Vector2Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y));
    }

    public Vector2 GridPos(Vector2 v)
    {
        Vector2 offset = new Vector2(tileMap.transform.position.x, tileMap.transform.position.y);
        v += offset;
        return new Vector2(v.x + 0.5f, v.y + 0.5f);
    }
    #endregion

    #region[채광여부체크]
    public bool DigCheck(Vector2Int v)
    {
        Tile tile = GetTile(tileMap,v);
        Tile tile2 = GetTile(tileItem, v);
        if (tile2 == Tile.나뭇잎)
            return true;
        if (tile2 == Tile.꿀)
            return true;
        if (tile2 == Tile.응가)
            return true;
        if (tile2 == Tile.바위)
            return false;

        if (0 <= v.x && v.x < mapW && 0 <= v.y && v.y < mapH)
            if (colArray[v.x, v.y] > 0)
                return false;

        if (tile == Tile.공백)
            return true;
        if (tile == Tile.흙)
            return true;

        return true;
    }

    public bool DirtWallCheck(Vector2Int v1,Vector2Int v2) //v1에서 v2로
    {
        Vector2 emp =  v2 - v1;
        bool flag = v1.x < 0 || v1.x >= mapW || v1.y < 0 || v1.y >= mapH;

        if (flag)
            return false;

        if (emp == Vector2.left)
            return dirtWall[v1.x, v1.y].left;
        else if (emp == Vector2.right)
            return dirtWall[v1.x, v1.y].right;
        else if (emp == Vector2.up)
            return dirtWall[v1.x, v1.y].up;
        else
            return dirtWall[v1.x, v1.y].down;

    }

    public Tile TileCheck(Vector2Int v)
    {
        //Debug.Log("Tile Check Pos " + v);
        Tile tile = GetTile(v);

        Tile tileitem = GetTile(tileItem, v);
        if (tileitem == Tile.나뭇잎)
        {
            ActObjectTile(v);
            return tileitem;
        }
        else if (tileitem == Tile.응가)
        {
            ActObjectTile(v);
            return tileitem;
        }
        else if (tileitem == Tile.바위)
        {
            return tileitem;
        }

        return tile;
    }

    #endregion

    #region[위치에 도착시 발동]
    public void ArrivePos(Vector2Int v)
    {
        Tile tile = GetTile(v);
        if (tile == Tile.데드존)
        {
            ActObjectTile(v);
        }
    }
    #endregion

    #region[오브젝트 효과발동시 처리]
    public void ActObjectTile(Vector2Int v)
    {
        Tile tile = GetTile(tileItem,v);

        if (tile == Tile.나뭇잎)
        {
            string s = tileMap.GetTile(new Vector3Int(v.x, v.y, 0)).name;
            int ax = v.x;
            int ay = v.y;

            switch (s)
            {
                case "Leaf":
                    break;
                case "Leaf0":
                    break;
                case "Leaf1":
                    ax -= 1;
                    break;
                case "Leaf2":
                    ay += 1;
                    break;
                case "Leaf3":
                    ax -= 1;
                    ay += 1;
                    break;

            }

            if (!soloLeaf)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2Int emp = new Vector2Int(ax, ay) + new Vector2Int(i % 2, -(i / 2));
                    colArray[emp.x, emp.y]--;
                }
            }
            else
                colArray[ax, ay]--;
        }
        else if (tile == Tile.거미줄)
        {
            Debug.Log("거미줄발동");
        }
        else if (tile == Tile.응가)
        {
            Debug.Log("응가발동");
        }
    }

    #endregion

    #region[오브젝트 설치시 처리함]
    public void SetObjectTile(Vector2Int v,bool flag)
    {
        if (0 > v.x || v.x >= mapW || 0 > v.y || v.y >= mapH)
            return;
        Tile tile = GetTile(tileItem, v);

        if(flag)
        {
            if (tile == Tile.마늘)
            {
                colArray[v.x, v.y] = GameManager.instance.garlicTime;
            }
            else if (tile == Tile.응가)
            {
                colArray[v.x, v.y] = GameManager.instance.pooTime;
            }
            else if (tile == Tile.꿀)
            {
                colArray[v.x, v.y] = GameManager.instance.honeyTime;
            }
            else if (tile == Tile.나뭇잎)
            {
                colArray[v.x, v.y] = GameManager.instance.leafNum;
            }
            else if (tile == Tile.방울)
            {
                shiledArray[v.x, v.y] = GameManager.instance.bubbleTime;
            }
            else if (tile == Tile.바위)
            {
                colArray[v.x, v.y] = 1;
            }
            else if (tile == Tile.꽃줄기)
            {
                bool left = GetTile(tileItem, new Vector2Int(v.x - 1, v.y)) != Tile.바위;
                bool right = GetTile(tileItem, new Vector2Int(v.x + 1, v.y)) != Tile.바위;

                if (v.x - 1 >= 0 && GetTile(tileMap, new Vector2Int(v.x - 1, v.y)) == Tile.흙)
                {
                    dirtWall[v.x - 1, v.y].right = left;
                    dirtWall[v.x, v.y].left = left;
                }

                if (v.x + 1 < mapW && GetTile(tileMap, new Vector2Int(v.x + 1, v.y)) == Tile.흙)
                {
                    dirtWall[v.x + 1, v.y].left = right;
                    dirtWall[v.x, v.y].right = right;
                }
            }
        }
        else
        {
            tile = GetTile(v);
            if (tile == Tile.흙 || tile == Tile.시작 || tile == Tile.끝)
            {
                bool left = GetTile(new Vector2Int(v.x - 1, v.y)) != Tile.공백 && GetTile(tileItem, new Vector2Int(v.x - 1, v.y)) != Tile.바위 && GetTile(new Vector2Int(v.x - 1, v.y)) != Tile.데드존;
                bool right = GetTile(new Vector2Int(v.x + 1, v.y)) != Tile.공백 && GetTile(tileItem, new Vector2Int(v.x + 1, v.y)) != Tile.바위 && GetTile(new Vector2Int(v.x + 1, v.y)) != Tile.데드존;
                bool up = GetTile(new Vector2Int(v.x, v.y + 1)) != Tile.공백 && GetTile(tileItem, new Vector2Int(v.x, v.y + 1)) != Tile.바위 && GetTile(new Vector2Int(v.x, v.y + 1)) != Tile.데드존;
                bool down = GetTile(new Vector2Int(v.x, v.y - 1)) != Tile.공백 && GetTile(tileItem, new Vector2Int(v.x, v.y - 1)) != Tile.바위 && GetTile(new Vector2Int(v.x, v.y - 1)) != Tile.데드존;
                dirtWall[v.x, v.y].left = left;
                dirtWall[v.x, v.y].right = right;
                dirtWall[v.x, v.y].up = up;
                dirtWall[v.x, v.y].down = down;
            }
        }
    }

    #endregion

    #region[오브젝트가 존재하면 실시간처리]
    public void UpdateObjectTile()
    {
        for (int x = -1; x < mapW + 1; x++)
            for (int y = -1; y < mapH + 1; y++)
                UpdateObjectTile(false,new Vector2Int(x, y));

        for (int x = -1; x < mapW + 1; x++)
            for (int y = -1; y < mapH + 1; y++)
                UpdateObjectTile(true, new Vector2Int(x, y));
    }

    public void UpdateObjectTile(bool flag,Vector2Int v)
    {
        Tile tile;
        if (0 > v.x || v.x >= mapW || 0 > v.y || v.y >= mapH)
            tile = Tile.데드존;

        if (flag)
        {
            tile = GetTile(tileItem, v);
            if (tile == Tile.마늘 || tile == Tile.응가 || tile == Tile.꿀)
            {
                colArray[v.x, v.y] -= Time.deltaTime;
                if (colArray[v.x, v.y] <= 0)
                {
                    SetTile(tileItem, v, Tile.공백);
                    SetTile(v, Tile.흙);
                }
            }
            else if (tile == Tile.나뭇잎)
            {
                if (colArray[v.x, v.y] <= 0)
                {
                    SetTile(tileItem, v, Tile.공백);
                    SetTile(v, Tile.흙);
                }
            }
            else if (tile == Tile.방울)
            {
                shiledArray[v.x, v.y] -= Time.deltaTime;
                if (shiledArray[v.x, v.y] <= 0)
                {
                    SetTile(tileItem, v, Tile.공백);
                    SetTile(v, Tile.흙);
                }
            }
            else if (tile == Tile.데드존)
            {
                if (CAntManager.Instance.AntLocationCheck(v))
                {
                    SoundManager.instance.se05away.Play();
                    CAnt ant = CAntManager.Instance.GetAnt(v);
                    CAntManager.Instance.DeadAnd(ant);
                }
            }
            else if (tile == Tile.끝)
            {
                if (CAntManager.Instance.AntLocationCheck(v))
                {
                    SoundManager.instance.se05away.Play();
                    CAnt ant = CAntManager.Instance.GetAnt(v);
                    try
                    {
                        CMineAnt emp0 = (CMineAnt)ant;
                        if (emp0)
                        {
                            GameManager.instance.nowMineNormalAnt += 1;
                            //Debug.Log("채굴개미 : " + GameManager.instance.nowMineNormalAnt);
                        }
                    }
                    catch { }

                    try
                    {
                        CNormalAnt emp1 = (CNormalAnt)ant;
                        if (emp1)
                        {
                            GameManager.instance.nowNormalAnt += 1;
                            //Debug.Log("일반개미 : " + GameManager.instance.nowNormalAnt);
  
                        }
                    }
                    catch { }
                    CAntManager.Instance.DeadAnd(ant);
                }
            }
        }
        else
        {
            tile = GetTile(tileMap, v);
            if (tile == Tile.데드존)
            {
                if (CAntManager.Instance.AntLocationCheck(v))
                {
                    SoundManager.instance.se05away.Play();
                    CAnt ant = CAntManager.Instance.GetAnt(v);
                    CAntManager.Instance.DeadAnd(ant);
                }
            }
        }
    }
    #endregion

    #region[길 이미지업데이트]
    private void RoadTileUpDate()
    {
        for (int x = 0; x < mapW; x++)
        {
            for (int y = 0; y < mapH; y++)
            {
                if (GetTile(new Vector2Int(x, y)) == Tile.흙)
                {
                    bool left = (GetTile(new Vector2Int(x - 1, y)) == Tile.공백 || GetTile(tileItem,new Vector2Int(x - 1, y)) == Tile.바위 || ((GetTile(new Vector2Int(x - 1, y)) == Tile.흙 || GetTile(new Vector2Int(x - 1, y)) == Tile.데드존) && !dirtWall[x, y].left));
                    bool right = (GetTile(new Vector2Int(x + 1, y)) == Tile.공백 || GetTile(tileItem, new Vector2Int(x + 1, y)) == Tile.바위 || ((GetTile(new Vector2Int(x + 1, y)) == Tile.흙 || GetTile(new Vector2Int(x + 1, y)) == Tile.데드존) && !dirtWall[x, y].right));
                    bool up = (GetTile(new Vector2Int(x, y + 1)) == Tile.공백 || GetTile(tileItem, new Vector2Int(x, y + 1)) == Tile.바위 || ((GetTile(new Vector2Int(x, y + 1)) == Tile.흙 || GetTile(new Vector2Int(x, y + 1)) == Tile.데드존) && !dirtWall[x, y].up));
                    bool down = (GetTile(new Vector2Int(x, y - 1)) == Tile.공백 || GetTile(tileItem, new Vector2Int(x, y - 1)) == Tile.바위 || ((GetTile(new Vector2Int(x, y - 1)) == Tile.흙 || GetTile(new Vector2Int(x, y - 1)) == Tile.데드존) && !dirtWall[x, y].down));
                    //false면 뚫린길
                    int type = 0;
                    if (!left && right && up && down)
                        type = 2;
                    else if (left && !right && up && down)
                        type = 0;
                    else if (left && right && !up && down)
                        type = 3;
                    else if (left && right && up && !down)
                        type = 1;

                    else if (!left && !right && up && down)
                        type = 4;  //─
                    else if (left && right && !up && !down)
                        type = 5;   //│

                    else if (!left && right && up && !down)
                        type = 7;  //┐
                    else if (left && !right && up && !down)
                        type = 6;   //┌
                    else if (left && !right && !up && down)
                        type = 9;   //└
                    else if (!left && right && !up && down)
                        type = 8;   //┘
                    else if (!left && !right && !up && down)
                        type = 12;  //┴
                    else if (!left && !right && up && !down)
                        type = 10;  //┬
                    else if (!left && right && !up && !down)
                        type = 11;  //┤
                    else if (left && !right && !up && !down)
                        type = 13;  //├

                    else if (!left && !right && !up && !down)
                        type = 14;  //┼

                    if(!hasTorch[x,y] && false)
                    {
                        Vector2 emp = GridPos(new Vector2(x, y));
                        if (type == 4)
                        {
                            Instantiate(torch[0], new Vector3(emp.x, emp.y, 0), Quaternion.identity);
                            hasTorch[x, y] = true;
                        }
                        else if(type == 5)
                        {
                            if(UnityEngine.Random.Range(0,100) > 50)
                                Instantiate(torch[1], new Vector3(emp.x, emp.y, 0), Quaternion.identity);
                            else
                                Instantiate(torch[2], new Vector3(emp.x, emp.y, 0), Quaternion.identity);

                            hasTorch[x, y] = true;
                        }
                    }

                    tileMap.SetTile(new Vector3Int(x, y, 0), roadTile[type]);

                }
            }
        }
    }
    #endregion

    #region[꿀탐색]
    public Vector2Int GetHoneyPos(Vector2Int v)
    {
        List<Vector2Int> list = new List<Vector2Int>();
        for (int x = 0; x < mapW; x++)
        {
            for (int y = 0; y < mapH; y++)
            {
                if (GetTile(tileItem,new Vector2Int(x, y)) == Tile.꿀 && Vector2Int.Distance(v, new Vector2Int(x, y)) <= GameManager.instance.honeyRange)
                    list.Add(new Vector2Int(x, y));
            }
        }

        if (list.Count > 0)
        {
            float minLen = Vector2Int.Distance(v, list[0]);
            int a = 0;
            for(int i = 0; i < list.Count; i++)
            {
                if (minLen > Vector2Int.Distance(v, list[i]))
                {
                    minLen = Vector2Int.Distance(v, list[i]);
                    a = i;
                }
            }
            return list[a];
        }
        else
            //암묵적으로 -100,-100이면 꿀이없다고하겠습니다!!
            return new Vector2Int(-100,-100);
    }
    #endregion
}
