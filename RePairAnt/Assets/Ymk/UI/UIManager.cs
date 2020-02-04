using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using TMPro;

public class UIManager : MonoBehaviour
{
    int palette = 25;

    Vector2Int gridMousePos;
    Vector2Int beforegridMousePos;

    public static UIManager instance;

    [HideInInspector]
    TileManager.Tile nowSelect = TileManager.Tile.공백; //현재선택중인 오브젝트
    int nowSelectButton = -1;

    public TileManager.Tile[] items = new TileManager.Tile[4]; //아이템오브젝트
    public GameObject[] itemButton = new GameObject[4];
    public GameObject winUI;
    public GameObject loseUI;
    public TextMeshProUGUI mineAntView;
    public TextMeshProUGUI normalAntView;

    Image[] itemImg = new Image[4];
    Image[] selectImg = new Image[4];
    GameObject[] itemCool = new GameObject[4];
    TextMeshProUGUI[] itemCoolText = new TextMeshProUGUI[4];
    float[] cooltime = new float[4];

    public TextMeshProUGUI timeUItext;

    //정지UI
    public GameObject stopUI;
    bool stopflag = true;

    #region[Awake]
    void Awake()
    {
        instance = this;
    }
    #endregion

    #region[Start]
    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            itemCool[i] = itemButton[i].transform.GetChild(0).gameObject;
            selectImg[i] = itemButton[i].transform.GetChild(1).GetComponent<Image>();
            itemImg[i] = itemButton[i].GetComponent<Button>().image;
            itemCoolText[i] = itemCool[i].GetComponent<TextMeshProUGUI>();
        }

        for (int i = 0; i < 4; i++)
        {
            //공백이면 버튼 비활성화
            if (items[i] == TileManager.Tile.공백)
                itemButton[i].SetActive(false);
            //아닐경우 버튼에 이미지와 명령어를 추가
            else
            {
                itemButton[i].transform.name = TileManager.instance.GetTileName(items[i]);
                string s = itemButton[i].transform.name;

                #region[버튼효과적용]
                if (i == 0)
                {
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerDown;
                    entry.callback.AddListener((eventData) => {
                        if (cooltime[0] <= 0)
                        {
                            ChangeItem(s);
                            nowSelectButton = 0;
                        }
                    });
                    itemButton[i].GetComponent<Button>().GetComponent<EventTrigger>().triggers.Add(entry);
                 //   itemButton[i].GetComponent<Button>().onClick.AddListener(() => { if (cooltime[0] <= 0) ChangeItem(s); });
                   // itemButton[i].GetComponent<Button>().onClick.AddListener(() => { if (cooltime[0] <= 0) nowSelectButton = 0; });
                }
                else if (i == 1)
                {
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerDown;
                    entry.callback.AddListener((eventData) => {
                        if (cooltime[1] <= 0)
                        {
                            ChangeItem(s);
                            nowSelectButton = 1;
                        }
                    });
                    itemButton[i].GetComponent<Button>().GetComponent<EventTrigger>().triggers.Add(entry);
                  //  itemButton[i].GetComponent<Button>().onClick.AddListener(() => { if (cooltime[1] <= 0) ChangeItem(s); });
                   // itemButton[i].GetComponent<Button>().onClick.AddListener(() => { if (cooltime[1] <= 0) nowSelectButton = 1; });
                }
                else if (i == 2)
                {
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerDown;
                    entry.callback.AddListener((eventData) => {
                        if (cooltime[2] <= 0)
                        {
                            ChangeItem(s);
                            nowSelectButton = 2;
                        }
                    });
                    itemButton[i].GetComponent<Button>().GetComponent<EventTrigger>().triggers.Add(entry);
                   // itemButton[i].GetComponent<Button>().onClick.AddListener(() => { if (cooltime[2] <= 0) ChangeItem(s); });
                   // itemButton[i].GetComponent<Button>().onClick.AddListener(() => { if (cooltime[2] <= 0) nowSelectButton = 2; });
                }
                else if (i == 3)
                {
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerDown;
                    entry.callback.AddListener((eventData) => {
                        if (cooltime[3] <= 0)
                        {
                            ChangeItem(s);
                            nowSelectButton = 3;
                        }
                    });
                    itemButton[i].GetComponent<Button>().GetComponent<EventTrigger>().triggers.Add(entry);
                    //itemButton[i].GetComponent<Button>().onClick.AddListener(() => { if (cooltime[3] <= 0) ChangeItem(s); });
                   // itemButton[i].GetComponent<Button>().onClick.AddListener(() => { if (cooltime[3] <= 0) nowSelectButton = 3; });
                }
                #endregion

                string PATH = "Sprite/";
                if(TileManager.instance.soloLeaf && items[i] == TileManager.Tile.나뭇잎)
                    PATH += "SoloLeaf";
                else if (items[i] == TileManager.Tile.바위)
                    PATH += "CaveObject";
                else
                    PATH += s;

                Sprite[] sprite = Resources.LoadAll<Sprite>(PATH);
                if (items[i] == TileManager.Tile.바위)
                {
                    itemImg[i].sprite = sprite[3];
                }
                else
                {
                    if (sprite.Length > 1)
                        itemImg[i].sprite = sprite[sprite.Length - 1];
                    else
                        itemImg[i].sprite = sprite[0];
                }
            }
        }
    }
    #endregion

    #region[Update]
    void Update()
    {
        gridMousePos = TileManager.instance.mousePosToGridPos(Input.mousePosition);
        if (beforegridMousePos != gridMousePos || true)
            RenderSelect(gridMousePos, nowSelect);
        beforegridMousePos = gridMousePos;
        SetObject(gridMousePos, nowSelect);
        RenderUI();
    }

    void RenderUI()
    {
        mineAntView.text = GameManager.instance.nowMineNormalAnt + " / " + GameManager.instance.mineAntTargetAmount;
        normalAntView.text = GameManager.instance.nowNormalAnt + " / " + GameManager.instance.normalAntTargetAmount;
        int t = (int)GameManager.instance.timeLimit/60;
        if(t > 0)
            timeUItext.text = t + "<size=120>m</size>";
        else
            timeUItext.text = (int)GameManager.instance.timeLimit + "<size=120>s</size>";
        for (int i = 0; i < 4; i++)
        {
            if (cooltime[i] > 0)
            {
                itemCool[i].SetActive(true);
                itemCoolText[i].text = (int)(cooltime[i]) + "";
                itemImg[i].color = Color.gray;
                cooltime[i] -=   Time.deltaTime;
            }
            else
            {
                itemImg[i].color = Color.white;
                itemCool[i].SetActive(false);
            }
            if(i == nowSelectButton)
                selectImg[i].enabled = true;
            else
                selectImg[i].enabled = false;
        }

        //정지메뉴켜기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameStop(stopflag);
        }
    }
    #endregion

    #region[선택구역표시]
    void RenderSelect(Vector2Int v,TileManager.Tile tile)
    {
        Color color;

        //색깔 파레트 초기화
        for (int x = -palette; x <= palette; x++)
        {
            for (int y = -palette; y <= palette; y++)
            {
                TileManager.instance.SetTile(TileManager.instance.tileSelect, new Vector2Int(x, y), TileManager.Tile.공백);
                TileManager.instance.tileSelect.SetTileFlags(new Vector3Int(x, y, 0), TileFlags.None);
                color = TileManager.instance.tileSelect.GetColor(new Vector3Int(x, y, 0));
                TileManager.instance.tileSelect.SetColor(new Vector3Int(x, y, 0), new Color(1, 1, 1, color.a));
            }
        }

        //설치할 오브젝트를 표시
        TileManager.instance.SetTile(TileManager.instance.tileSelect, v, tile);

        //선택 파레트 중 설치할 오브젝트가 표시된 부분의 색조정
        for (int x = -palette; x <= palette; x++)
        {
            for (int y = -palette; y <= palette; y++)
            {
                TileManager.Tile emp = TileManager.instance.GetTile(TileManager.instance.tileSelect, new Vector3Int(x, y, 0));
                if(emp != TileManager.Tile.공백)
                {
                    TileManager.instance.tileSelect.SetTileFlags(new Vector3Int(x, y, 0), TileFlags.None);
                    color = TileManager.instance.tileSelect.GetColor(new Vector3Int(x, y, 0));
                    if (SetCheck(new Vector2Int(x, y),tile))
                        //설치가능하면 녹색
                        TileManager.instance.tileSelect.SetColor(new Vector3Int(x, y, 0), new Color(0, 1, 0, color.a));
                    else
                        //설치불가능하면 적색
                        TileManager.instance.tileSelect.SetColor(new Vector3Int(x, y, 0), new Color(1, 0, 0, color.a));
                }
            }
        }
    }
    #endregion

    #region[오브젝트설치]
    void SetObject(Vector2Int v, TileManager.Tile tile)
    {
        if(Input.GetMouseButtonUp(0))
        {
            bool flag = true; //설치가능여부
            for (int x = -palette; x <= palette; x++)
            {
                for (int y = -palette; y <= palette; y++)
                {
                    TileManager.Tile emp = TileManager.instance.GetTile(TileManager.instance.tileSelect, new Vector3Int(x, y, 0));
                    //설치표시공간에
                    if (emp != TileManager.Tile.공백)
                    {
                        //오브젝트가 있으면 설치를 못하게함
                        if (!SetCheck(new Vector2Int(x, y), tile))
                        {
                            //설치불가!!
                            flag = false;
                            goto Exit;
                        }
                    }
                }
            }
        Exit:

            if (v.x < 0 || v.x >= TileManager.mapW || v.y < 0 || v.y >= TileManager.mapH)
                flag = false;

            if (!flag)
                CancelButton();

            if (tile == TileManager.Tile.공백)
                flag = false;

            if (flag)
            {
                if (tile == TileManager.Tile.흙)
                    TileManager.instance.SetTile(TileManager.instance.tileMap, v, tile);
                else
                    TileManager.instance.SetTile(TileManager.instance.tileItem, v, tile);
                for (int x = -palette; x <= palette; x++)
                {
                    for (int y = -palette; y <= palette; y++)
                    {
                        TileManager.Tile emp = TileManager.instance.GetTile(TileManager.instance.tileSelect, new Vector3Int(x, y, 0));
                        //설치된공간에
                        if (emp != TileManager.Tile.공백)
                        {
                            SoundManager.instance.se01.Play();
                            if (tile == TileManager.Tile.꽃줄기)
                                TileManager.instance.SetTile(TileManager.instance.tileMap, new Vector2Int(x, y), TileManager.Tile.흙);
                            TileManager.instance.SetObjectTile(new Vector2Int(x, y),true);
                            SetItemCoolTime(nowSelectButton, nowSelect);
                            nowSelectButton = -1;
                            nowSelect = TileManager.Tile.공백;
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region[설치가능여부검사]
    public bool SetCheck(Vector2Int v, TileManager.Tile tileType)
    {
        TileManager.Tile tile = TileManager.instance.GetTile(TileManager.instance.tileMap, new Vector3Int(v.x, v.y, 0));
        TileManager.Tile item = TileManager.instance.GetTile(TileManager.instance.tileItem, new Vector3Int(v.x, v.y, 0));

        if (tileType == TileManager.Tile.나뭇잎 || tileType == TileManager.Tile.응가 || tileType == TileManager.Tile.바위)
        {
            if (CAntManager.Instance.AntLocationCheck(v))
                return false;

            if (CAntManager.Instance.NextAntLocationCheck(v))
                return false;
        }

        if (tileType == TileManager.Tile.꽃줄기)
        {
            //꽃줄기는 흙 + 바위 + 위험구역에 설치가능
            if ((tile == TileManager.Tile.흙 && item == TileManager.Tile.공백) || (item == TileManager.Tile.바위 && !TileManager.instance.tileItem.GetTile(new Vector3Int(v.x,v.y,0)).name.Equals("SRock")) || (item == TileManager.Tile.데드존 && !TileManager.instance.tileItem.GetTile(new Vector3Int(v.x, v.y, 0)).name.Equals("DeadZone")))
                return true;
        }
        else if (tileType == TileManager.Tile.바위)
        {
            //바위는 공백에 설치가능
            if (tile == TileManager.Tile.공백 && item == TileManager.Tile.공백)
                return true;
        }
        else
        {
            //나머지는 흙에만 설치가능
            if (tile == TileManager.Tile.흙 && item == TileManager.Tile.공백)
                return true;
        }
        return false;
    }
    #endregion

    #region[아이템쿨타임세팅]
    public void SetItemCoolTime(int n,string name)
    {
        SetItemCoolTime(n, TileManager.instance.GetTile(name));
    }

    public void SetItemCoolTime(int n, TileManager.Tile tile)
    {
        if (n < 0)
            return;
        if (tile == TileManager.Tile.꿀)
            cooltime[n] = GameManager.instance.honeyCoolTime;
        else if (tile == TileManager.Tile.나뭇잎)
            cooltime[n] = GameManager.instance.leafCoolTime;
        else if (tile == TileManager.Tile.꽃줄기)
            cooltime[n] = GameManager.instance.flowerStemCoolTime;
        else if (tile == TileManager.Tile.마늘)
            cooltime[n] = GameManager.instance.garlicCoolTime;
        else if (tile == TileManager.Tile.거미줄)
            cooltime[n] = GameManager.instance.spiderWebCoolTime;
        else if (tile == TileManager.Tile.방울)
            cooltime[n] = GameManager.instance.bubbleCoolTime;
        else if (tile == TileManager.Tile.응가)
            cooltime[n] = GameManager.instance.pooCoolTime;
        else if (tile == TileManager.Tile.애벌레)
            cooltime[n] = GameManager.instance.larvaCoolTime;
        else if (tile == TileManager.Tile.바위)
            cooltime[n] = GameManager.instance.rockCoolTime;
    }
    #endregion

    #region[선택아이템을 바꿔주는 기능]
    public void ChangeItem(int i)
    {
        Debug.Log("");
        if (cooltime[i] <= 0)
        {
            ChangeItem(transform.name);
            nowSelectButton = i;
        }
    }

    public void ChangeItem(string name)
    {
        nowSelect = TileManager.instance.GetTile(name);
    }
    #endregion

    #region[취소버튼]
    public void CancelButton()
    {
        nowSelectButton = -1;
        nowSelect = TileManager.Tile.공백;
    }
    #endregion

    #region[게임정지]
    public void GameStop(bool c)
    {
        Time.timeScale = c ? 0 : 1;
        stopUI.SetActive(c);
        stopflag = !stopflag;
    }
    #endregion

    #region[타이틀로]
    public void GoTitle()
    {
        Time.timeScale = 1;
        GameManager.instance.GoTitle();
    }
    #endregion
}
