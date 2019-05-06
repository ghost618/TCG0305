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


public class RoomManager : MonoBehaviour
{
    bool flag = true;
    public Button ReadyBtn;

    public string ReadyUrl = "http://127.0.0.1:8080/room/ready";
    public string CancelUrl = "http://127.0.0.1:8080/room/cancel";
    public string QuitRoomUrl = "http://127.0.0.1:8080/hall/quit-room";
    public string GetReadyUrl = "http://127.0.0.1:8080/room/getready";
    public string RoomListUrl = "http://127.0.0.1:8080/hall/list";

    private string ChatUrl = "ws://47.101.219.5:8080/chat/" + userModel.getName();

    private Dictionary<int, RoomStatus> RoomList;

    private static int UserNo;
    private static int EnemyNo;

    public Text PlayerName;
    public Text EnemyName;
    public Text Tips;

    public Text ChatText_you;

    private static int TextCount = 0;

    public InputField inputField;

    private WebSocket webSocket;

    private ArrayList MessageList = new ArrayList();

    private void LoadRoomData()
    {
        RoomList = new Dictionary<int, RoomStatus>();
        List<RoomStatus> Rooms = httpscript.PostGetRoomListHttp(RoomListUrl, "");
        foreach (RoomStatus room in Rooms)
        {
            RoomList.Add(room.roomIndex, room);
        }
    }

    public void OnReadyBtn()
    {
        if (flag)
        {
            ReadyBtn.GetComponentInChildren<Text>().text = "Cancel";
            flag = !flag;
            httpscript.PostHttp(ReadyUrl, "roomindex=" + GameHallManager.index);

        }
        else
        {
            ReadyBtn.GetComponentInChildren<Text>().text = "Ready";
            flag = !flag;
            httpscript.PostHttp(CancelUrl, "roomindex=" + GameHallManager.index);
        }
    }

    public void OnBackBtn()
    {
        Close();
        httpscript.PostHttp(QuitRoomUrl, "token=" + userModel.getToken());
    }

    //判断房间里玩家是否准备完成
    private string GetReady()
    {
        string response = httpscript.PostHttp(GetReadyUrl, "roomindex=" + GameHallManager.index);
        return response;
    }
    public float timer = 2.0f;

    private void Start()
    {
        init();
        Connect();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 2.0f;
            LoadRoomData();
            //更新房间内两个玩家的用户名 
            try {
                if (!RoomList[GameHallManager.index_for_user].username[0].Equals(userModel.getName()))
                {
                    UserNo = 1;
                    EnemyNo = 0;
                    EnemyName.text = RoomList[GameHallManager.index_for_user].username[EnemyNo];
                    PlayerName.text = RoomList[GameHallManager.index_for_user].username[UserNo];
                }
                else
                {
                    UserNo = 0;
                    EnemyNo = 1;
                    PlayerName.text = RoomList[GameHallManager.index_for_user].username[UserNo];
                    if (RoomList[GameHallManager.index_for_user].username.Count == 2)
                        EnemyName.text = RoomList[GameHallManager.index_for_user].username[EnemyNo];
                }
            }catch
            {
                Debug.Log("用户为空，数组访问越界");
            }
            
            //更新房间内的准备状态，全都准备则跳转至游戏界面
            if (GetReady() == "true")
            {
                Close();
                Application.LoadLevel(4);
            }
            else
            {
                Tips.text = "正在等待所有玩家准备就绪";
            }
        }
        //消息队列处理接收的信息
        if (MessageList.Count > 0)
        {
            string Msg = MessageList[0].ToString();
            MessageList.Remove(MessageList[0]);
            try
            {
                if (TextCount > 4)
                {
                    ChatText_you.text = "";
                    TextCount = 0;
                }
                ChatMessage ReceivedMsg = JsonConvert.DeserializeObject<ChatMessage>(Msg);
                if (ReceivedMsg.receiver == RoomList[GameHallManager.index_for_user].username[EnemyNo])
                {
                    ChatText_you.text += RoomList[GameHallManager.index_for_user].username[UserNo] + ": " +
                        ReceivedMsg.message + "\n";
                    TextCount++;
                }
                else if (ReceivedMsg.receiver == RoomList[GameHallManager.index_for_user].username[UserNo])
                {
                    ChatText_you.text += RoomList[GameHallManager.index_for_user].username[EnemyNo] + ": " +
                        ReceivedMsg.message + "\n";
                    TextCount++;
                }                

            }
            catch
            {
                Debug.Log(Msg);
            }
            
        }
    }

    public void OnSendBtn()
    {
        string msg = inputField.text.Trim();
        try {
            ChatMessage message = new ChatMessage(RoomList[GameHallManager.index_for_user].username[UserNo],
          EnemyName.text = RoomList[GameHallManager.index_for_user].username[EnemyNo], msg);
            string MessageToOther = JsonConvert.SerializeObject(message);
            Debug.Log(MessageToOther);
            Send(MessageToOther);
        }
        catch
        {
            Debug.Log("发送信息出现错误");
        }
        
        inputField.text = "";
    }

    private void init()
    {
        webSocket = new WebSocket(new Uri(ChatUrl));
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
        MessageList.Add(message);
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

