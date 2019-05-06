using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using BestHTTP.WebSocket;
using System;
using BestHTTP.Examples;
using UnityEngine.UI;
using System.Text;
using Newtonsoft.Json;

public class BattleManager : MonoBehaviour
{
    private string url = "ws://47.101.219.5:8080/websocket/" + userModel.getName();
    public string getinitUrl = "http://127.0.0.1:8080/pvp/get-init";
    public string CardinfoUrl = "http://127.0.0.1:8080/card/getAll";
    public string CardPointUrl = "http://127.0.0.1:8080/pvp/getCardPoint";
    public string CurrrntRoundWinnerUrl = "http://127.0.0.1:8080/pvp/getwinner";
    public string LastWinnerUrl = "http://127.0.0.1:8080/pvp/getlastwinner";
    public string getnextUrl = "http://127.0.0.1:8080/pvp/get-next";
    public string getscoreUrl = "http://127.0.0.1:8080/pvp/getScore";
    public string UpdateHCUrl = "http://127.0.0.1:8080/pvp/updateHandCard";
    public string HandCardCountUrl = "http://127.0.0.1:8080/pvp/getCardCount";

    public gridpanel PcPanel;
    public gridpanel PfPanel;
    public gridpanel EcPanel;
    public gridpanel EfPanel;
    public gridpanel HcPanel;
    public Text PlayerHp;
    public Text EnemyHp;
    public Text PlayerPoint;
    public Text EnemyPoint;
    public Text ResultTip;
    public Text FunctionText;
    public Text EnemyCardCount;
    public Text PlayerCardCount;

    public static Battle battleinfo_init;
    public static Battleinfo battleinfo = new Battleinfo("a","b", 0,new Operation(0,0,"a",0));
    public static Battleinfo ReceiveBattleinfo = new Battleinfo("a","b", 0, new Operation(0, 0, "a", 0));
    public static string battlemessage = JsonConvert.SerializeObject(ReceiveBattleinfo);
    public static ArrayList BattleMessageList = new ArrayList();
    //public string confirmmessage = "";
    private Dictionary<int, Card> CardList;
    private static int userNo;
    private static int EnemyNo;
    private bool isEnd = false;
    private bool EnemyisEnd = false;
    private bool isEndForCheckHc = false;
    private  int PlayerScore = 2;
    private  int EnemyScore = 2;
    private bool loadnewround = true;
    private bool checkhand = true;
    public static string Winner;
    public static string winnername;
    
    private WebSocket webSocket;

    private static BattleManager instance;

    public static BattleManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Start()
    {
        //加载战场和卡牌信息
        loadData();
        //初始化和建立websocket链接
        init();
        Connect();
        InitCard();       
    }

    //public float timer = 0.5f;
    public float timer_for_tip = 2.0f;

    private void Update()
    {
        
        if (ReceiveBattleinfo.operation.opera == "End" && ReceiveBattleinfo.receiver == userModel.getName())
        {
            EnemyisEnd = true;                            
        }

        if (ReceiveBattleinfo.operation.opera == "End" && ReceiveBattleinfo.sender == userModel.getName())
        {
            isEnd = true;
        }

        //检查手牌区以确认是否结束回合
        if (!isEnd && checkhand)
        {
            checkHc();
            
        }
            

        //战局结算
        if (isEnd && EnemyisEnd)
        {
            if (loadnewround)
            {
                //重启新回合
                string initbattleinfo_next = httpscript.PostHttp(getnextUrl, "roomindex=" + GameHallManager.index);
                battleinfo_init = JsonConvert.DeserializeObject<Battle>(initbattleinfo_next);
                loadnewround = false;
                
                Debug.Log(initbattleinfo_next);
            }                       
            
            //Debug.Log("进入战局结算");
            //显示这一回合的获胜者，保持两秒
            string winnername = httpscript.PostHttp(CurrrntRoundWinnerUrl, "roomindex=" + GameHallManager.index);
            string winnertext = winnername + "是本轮胜者";
            ResultTip.text = winnertext;
            timer_for_tip -= Time.deltaTime;
            if (timer_for_tip <= 0)
            {
                ResultTip.text = "";
                timer_for_tip = 2.0f;
                isEnd = false;
                EnemyisEnd = false;
                isEndForCheckHc = false;

                loadnewround = true;
                //初始化卡片信息展示区
                FunctionText.text = "";
                if(winnername == battleinfo_init.players[userNo].username)
                {
                    EnemyScore--;
                }else if(winnername == battleinfo_init.players[EnemyNo].username)
                {
                    PlayerScore--;
                }
                            
                if (PlayerScore != 0 && EnemyScore != 0)
                {
                   
                    CleanZone();
                    InitCard();
                }
                else
                {
                    winnername = httpscript.PostHttp(LastWinnerUrl, "roomindex=" + GameHallManager.index);
                    ResultTip.text =  winnername+ "是最终胜者";
                    Winner = ResultTip.text;
                    Application.LoadLevel(7);
                }

            }
                       
            
        }
        //更新接受到的信息
        if (BattleMessageList.Count>0)
        {
            battlemessage = BattleMessageList[0].ToString();
            BattleMessageList.Remove(BattleMessageList[0]);
            ResultTip.text = "";
            ReceiveBattleinfo = JsonConvert.DeserializeObject<Battleinfo>(battlemessage);
            if (ReceiveBattleinfo.sender == battleinfo_init.players[EnemyNo].username)
            {
                UpdateCard();
                //魔法卡效果
                switch (ReceiveBattleinfo.operation.id)
                {                    
                    case 13:
                        CleanHCZone();
                        ReInitHandCard();
                        break;
                    case 15:
                        CleanCloseZone();
                        break;
                    default:
                        break;
                }

                if (!isEnd && !EnemyisEnd)
                {
                    ResultTip.text = "你的回合";
                }
                else if (isEnd && !EnemyisEnd)
                {
                    ResultTip.text = "等待对方出完牌";
                }

                try
                {
                    FunctionText.text = GetCardText(CardList[ReceiveBattleinfo.operation.id]);
                }
                catch
                {
                    Debug.Log("发送了放弃信息，因此没有卡片信息可以显示");
                }
                
            }else if (ReceiveBattleinfo.sender == battleinfo_init.players[userNo].username)
            {
                //魔法卡效果实现
                switch (ReceiveBattleinfo.operation.id)
                {
                    case 11:
                        CleanHCZone();
                        ReInitHandCard();
                        break;
                    case 12:
                        CleanHCZone();
                        ReInitHandCard();
                        break;
                    case 13:
                        CleanHCZone();
                        ReInitHandCard();
                        break;
                    case 14:
                        CleanHCZone();
                        ReInitHandCard();
                        break;
                    case 15:
                        CleanCloseZone();
                        break;
                    default:
                        break;
                }

                try
                {
                    FunctionText.text = GetCardText(CardList[ReceiveBattleinfo.operation.id]);
                }
                catch
                {
                    Debug.Log("发送了放弃信息，因此没有卡片信息可以显示");
                }
            }
            
            //获取当前双方卡片点数
            PlayerPoint.text = httpscript.PostHttp(CardPointUrl, "username=" + battleinfo_init.players[userNo].username +
                "&roomindex=" + GameHallManager.index);
            EnemyPoint.text = httpscript.PostHttp(CardPointUrl, "username=" + battleinfo_init.players[EnemyNo].username +
                "&roomindex=" + GameHallManager.index);

            //获取双方剩余手牌数量
            PlayerCardCount.text = "剩余手牌：" + httpscript.PostHttp(HandCardCountUrl, "player=" + battleinfo_init.players[userNo].username +
                "&roomindex=" + GameHallManager.index);
            EnemyCardCount.text = "剩余手牌：" + httpscript.PostHttp(HandCardCountUrl, "player=" + battleinfo_init.players[EnemyNo].username +
                "&roomindex=" + GameHallManager.index);
        }
    

    }

    private void Awake()
    {
        instance = this;
        gridimage.OnClickL = null;
        gridimage.OnClickR = null;
        gridimage.OnEnter = null;
        gridimage.OnExit = null;
        //添加鼠标点击事件
        gridimage.OnClickL += Hc_OnClickL;
    }

    private void loadData()
    {
        //获取战斗信息
        string initbattleinfo_init = httpscript.PostHttp(getinitUrl, "roomindex=" + GameHallManager.index);
        battleinfo_init = JsonConvert.DeserializeObject<Battle>(initbattleinfo_init);
        //获取所有种类卡牌信息
        CardList = new Dictionary<int, Card>();
        List<Card> cards = httpscript.PostGetAllCardHttp(CardinfoUrl, "");
        foreach(Card card in cards)
        {
            CardList.Add(card.idcards, card);
        }
    }

    private void CleanZone()
    {
        for(int i = 0; i < 5; i++)
        {
            cardModel.DeleteCard(EfPanel.grids[i].name);
            EfPanel.grids[i].transform.DetachChildren();

            cardModel.DeleteCard(EcPanel.grids[i].name);
            EcPanel.grids[i].transform.DetachChildren();

            cardModel.DeleteCard(PfPanel.grids[i].name);
            PfPanel.grids[i].transform.DetachChildren();

            cardModel.DeleteCard(PcPanel.grids[i].name);
            PcPanel.grids[i].transform.DetachChildren();
        }
        for(int i = 0; i < 8; i++)
        {
            cardModel.DeleteCard(HcPanel.grids[i].name);
            HcPanel.grids[i].transform.DetachChildren();
        }
    }

    //清除近战单位
    private void CleanCloseZone()
    {
        for (int i = 0; i < 5; i++)
        {
            cardModel.DeleteCard(EcPanel.grids[i].name);
            EcPanel.grids[i].transform.DetachChildren();

            cardModel.DeleteCard(PcPanel.grids[i].name);
            PcPanel.grids[i].transform.DetachChildren();
        }
    }

    //清空手牌区
    private void CleanHCZone()
    {
        for (int i = 0; i < 8; i++)
        {
            cardModel.DeleteCard(HcPanel.grids[i].name);
            HcPanel.grids[i].transform.DetachChildren();
        }
    }

    //更新手牌区，用于魔法卡效果
    private void ReInitHandCard()
    {
        int[] NewHC;
        string HCMessage;
        HCMessage = httpscript.PostHttp(UpdateHCUrl, "player=" + battleinfo_init.players[userNo].username +
            "&roomindex=" + GameHallManager.index);
        NewHC = new int[JsonConvert.DeserializeObject<int[]>(HCMessage).Length];
        NewHC = JsonConvert.DeserializeObject<int[]>(HCMessage);
        for(int i = 0; i < NewHC.Length; i++)
        {
            PutCard(NewHC[i], HcPanel.GetEmptyGrid());
        }
    }

    private void InitCard()
    {
        //确认用户在传回信息的顺序和初始化手牌
        if (userModel.getName() == battleinfo_init.players[0].username)
        {
            userNo = 0;
            EnemyNo = 1;
            for (int i = 0; i < battleinfo_init.duel.handCards[userNo].Count; i++)
            {
                PutCard(battleinfo_init.duel.handCards[userNo][i], HcPanel.GetEmptyGrid());
            }
            //初始化双方生命值
            PlayerHp.text = PlayerScore.ToString();
            EnemyHp.text = EnemyScore.ToString();
        }
        else
        {
            userNo = 1;
            EnemyNo = 0;
            for (int i = 0; i < battleinfo_init.duel.handCards[userNo].Count; i++)
            {               
                PutCard(battleinfo_init.duel.handCards[userNo][i], HcPanel.GetEmptyGrid());
            }
            PlayerHp.text = PlayerScore.ToString();
            EnemyHp.text = EnemyScore.ToString();
        }
        //确定当前出牌玩家
        if (battleinfo_init.duel.CurPlayer == userNo)
        {
            ReceiveBattleinfo.operation.opera = "Wait";
            Debug.Log("Wait");
            ResultTip.text = "你的回合";
            ReceiveBattleinfo.sender = battleinfo_init.players[EnemyNo].username;
            ReceiveBattleinfo.receiver = battleinfo_init.players[userNo].username;
        }
        else if(battleinfo_init.duel.CurPlayer == EnemyNo)
        {
            ReceiveBattleinfo.operation.opera = "Start";
            Debug.Log("Start");            
            ReceiveBattleinfo.receiver = battleinfo_init.players[EnemyNo].username;
            ReceiveBattleinfo.sender = battleinfo_init.players[userNo].username;
        }
        //初始化卡片点数
        PlayerPoint.text = httpscript.PostHttp(CardPointUrl, "username=" + battleinfo_init.players[userNo].username +
                    "&roomindex=" + GameHallManager.index);
        EnemyPoint.text = httpscript.PostHttp(CardPointUrl, "username=" + battleinfo_init.players[EnemyNo].username +
            "&roomindex=" + GameHallManager.index);

        //初始化手牌数量
        PlayerCardCount.text = "剩余手牌：" + httpscript.PostHttp(HandCardCountUrl, "player=" + battleinfo_init.players[userNo].username +
                "&roomindex=" + GameHallManager.index);
        EnemyCardCount.text = "剩余手牌：" + httpscript.PostHttp(HandCardCountUrl, "player=" + battleinfo_init.players[EnemyNo].username +
            "&roomindex=" + GameHallManager.index);

        battlemessage = JsonConvert.SerializeObject(ReceiveBattleinfo);
    }

    //上传己方发出的卡片
    private void UploadCard()
    {
        battleinfo.sender = userModel.getName();
        battleinfo.receiver = battleinfo_init.players[EnemyNo].username;
        battleinfo.roomindex = GameHallManager.index_for_user;
        battleinfo.operation.opera = "Wait";

        Send(JsonConvert.SerializeObject(battleinfo));
        if(!EnemyisEnd)
            ReceiveBattleinfo.operation.opera = "Start";
        
    }

    //更新对手发出的卡片
    private void UpdateCard()
    {
        if (ReceiveBattleinfo.operation.id != 0)
        {
            if (ReceiveBattleinfo.operation.type == 0)
            {
                PutCard(ReceiveBattleinfo.operation.id, EcPanel.GetEmptyGrid());
            }else if(ReceiveBattleinfo.operation.type == 1)
            {
                PutCard(ReceiveBattleinfo.operation.id, EfPanel.GetEmptyGrid());
            }   

        }
           
    }

    //检查手牌区
    private void checkHc()
    {
        isEndForCheckHc = true;
        for(int i = 0; i < 8; i++)
        {
            if (cardModel.GetCard(HcPanel.grids[i].name)!=null)
            {
                isEndForCheckHc = false;
            }
        }
        if (isEndForCheckHc)
        {
            Battleinfo battleinfo_GiveUp = new Battleinfo(battleinfo_init.players[userNo].username, battleinfo_init.players[EnemyNo].username,
                GameHallManager.index_for_user, new Operation(0, 0, "End", 0));
            Send(JsonConvert.SerializeObject(battleinfo_GiveUp));
            checkhand = false;
        }
    }

    private void Hc_OnClickL(Transform obj)
    {
        if (!isEnd)
        {
        if (ReceiveBattleinfo.receiver==battleinfo_init.players[userNo].username||EnemyisEnd)
        if (ReceiveBattleinfo.operation.opera == "Wait"|| EnemyisEnd)
        {
            if (obj.name.ToString().Contains("HcGrid"))
            {
                Card card = cardModel.GetCard(obj.name);
                if (card == null)
                {
                    Debug.Log("没有获取到卡片对象");
                    return;  //获取对应gridimage的card
                }
                if (card.cardtype == 0)
                {
                    PutCard(card.idcards, PcPanel.GetEmptyGrid());
                    cardModel.DeleteCard(obj.name);
                    obj.transform.DetachChildren();
                }
                else if (card.cardtype == 1)
                {
                    PutCard(card.idcards, PfPanel.GetEmptyGrid());
                    cardModel.DeleteCard(obj.name);
                    obj.transform.DetachChildren();
                }else if(card.cardtype == 2&&card.idcards == 15)
                        {
                            cardModel.DeleteCard(obj.name);
                            obj.transform.DetachChildren();
                        }

                battleinfo.operation.position = 0;
                battleinfo.operation.id = card.idcards;
                battleinfo.operation.type = card.cardtype;
                UploadCard();
            }

        }

        }
        
        
    }

    public void PutCard(int cardid, Transform EmptyGrid)
    {
        if (!CardList.ContainsKey(cardid))
        {
            return;
        }

        Transform emptyGrid = EmptyGrid;
        if (emptyGrid == null)
        {
            Debug.Log("区域已满");
            return;
        }
        Card card = CardList[cardid];

        GameObject cardPrefab = Resources.Load<GameObject>("Prefabs/cardimage");

        cardPrefab.GetComponent<cardimage>().UpdateItem(card.cardname);
        cardPrefab.GetComponent<cardimage>().UpdatePoint(card.cardpoint);

        GameObject cardGo = GameObject.Instantiate(cardPrefab);
        //Debug.Log(emptyGrid);

        cardGo.transform.SetParent(emptyGrid);

        cardGo.transform.localPosition = Vector3.zero;

        cardGo.transform.localScale = Vector3.one;

        //存储信息到cardModel中的字典中去
        cardModel.StoreItem(emptyGrid.name, card);
    }

    private string GetCardText(Card card)
    {
        if (card == null)
            return "";
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("<color=red>{0}</color>\n\n", card.cardname);
        switch (card.cardtype)
        {
            case 0:
                Card pc = card as Card;
                sb.AppendFormat("战斗力：{0}\n\n", pc.cardpoint);
                break;
            case 1:
                Card pf = card as Card;
                sb.AppendFormat("战斗力：{0}\n\n", pf.cardpoint);
                break;
            default:
                break;
        }
        sb.AppendFormat("<size=20><color=white>卡片id：{0}" +
            "</color></size>\n\n<color=yellow><size=20>描述：{1}</size></color>",
            card.idcards, card.cardinfo);
        return sb.ToString();
    }

    public void OnGiveUpThisRound()
    {
        isEnd=true;
        Battleinfo battleinfo_GiveUp = new Battleinfo(battleinfo_init.players[userNo].username, battleinfo_init.players[EnemyNo].username,
                GameHallManager.index_for_user, new Operation(0, 0, "End", 0));
        Send(JsonConvert.SerializeObject(battleinfo_GiveUp));
    }

    private void init()
    {
        webSocket = new WebSocket(new Uri(url));
        webSocket.OnOpen += OnOpen;
        webSocket.OnMessage += OnMessageReceived;
        webSocket.OnError += OnError;
        webSocket.OnClosed += OnClosed;
    }

    private void antiInit()
    {
        webSocket.OnOpen = null;
        webSocket.OnMessage = null;
        webSocket.OnError = null;
        webSocket.OnClosed = null;
        webSocket = null;
    }

    public void Connect()
    {
        webSocket.Open();
    }

    private byte[] getBytes(string message)
    {

        byte[] buffer = Encoding.Default.GetBytes(message);
        return buffer;
    }

    public void Send(string str)
    {
        webSocket.Send(str);
    }

    public void Close()
    {
        webSocket.Close();
    }

    #region WebSocket Event Handlers

    /// <summary>
    /// Called when the web socket is open, and we are ready to send and receive data
    /// </summary>
    void OnOpen(WebSocket ws)
    {
        Debug.Log("connected");

    }

    /// <summary>
    /// Called when we received a text message from the server
    /// </summary>
    void OnMessageReceived(WebSocket ws, string message)
    {
        Debug.Log(message);
        //battlemessage = message;
        BattleMessageList.Add(message);
    }

    /// <summary>
    /// Called when the web socket closed
    /// </summary>
    void OnClosed(WebSocket ws, UInt16 code, string message)
    {
        Debug.Log(message);

        antiInit();
        init();
    }

    private void OnDestroy()
    {
        if (webSocket != null && webSocket.IsOpen)
        {
            webSocket.Close();
            antiInit();
        }
    }

    /// <summary>
    /// Called when an error occured on client side
    /// </summary>
    void OnError(WebSocket ws, Exception ex)
    {
        string errorMsg = string.Empty;
#if !UNITY_WEBGL || UNITY_EDITOR
        if (ws.InternalRequest.Response != null)
            errorMsg = string.Format("Status Code from Server: {0} and Message: {1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
#endif
        Debug.Log(errorMsg);

        antiInit();
        init();
    }

    #endregion
}
