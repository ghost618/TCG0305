using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class GameHallManager : MonoBehaviour
{
    public string JoinUrl = "http://127.0.0.1:8080/hall/join";
    public string RoomListUrl = "http://127.0.0.1:8080/hall/list";
    string Token = userModel.getToken();
    public static string index;
    public static int index_for_user;
    public static List<string> username;
    public gridpanel RoomPanel;
    private Dictionary<int, RoomStatus> RoomList;
    

    private static GameHallManager instance;

    public static GameHallManager Instance
    {
        get
        {
            return instance;
        }
    }


    private void setRoom(int roomIndex)
    {
        RoomStatus rm = RoomList[roomIndex];
        RoomPanel.grids[roomIndex].GetComponent<Roomimage>().updatetext(rm.roomIndex.ToString());

        RoomModel.StoreRoom(RoomPanel.grids[roomIndex].name, rm);
    }

    private void updateRoomInfo(int roomIndex)
    {
        RoomStatus rm = RoomList[roomIndex];
        RoomModel.StoreRoom(RoomPanel.grids[roomIndex].name, rm);
        //Debug.Log(RoomModel.GetRoomStatus(RoomPanel.grids[roomIndex].name).username[0]);
    }
    private void Start()
    {
        //Debug.Log(httpscript.PostHttp(JoinUrl, "token=" + Token + "&roomIndex=" + roomindex));
        //RoomModel.InitRoom();
        for(int i = 0; i < RoomList.Keys.Count; i++)
        {
            this.setRoom(i);

        }
        
    }
    public float timer = 2.0f;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            instance = this;
            this.loadRoom();
            timer = 2.0f;
        }
    }

    private void Awake()
    {
        instance = this;
        this.loadRoom();
        gridimage.OnClickL = null;
        gridimage.OnClickR = null;
        gridimage.OnEnter = null;
        gridimage.OnExit = null;
        //添加鼠标点击事件
        gridimage.OnClickL += RoomImage_OnClickL;
    }

    

    private void RoomImage_OnClickL(Transform obj)
    {
        
        
        //输出点击的房间号
        Debug.Log(RoomModel.GetRoomStatus(obj.name).roomIndex);
        //获取房间号
        index_for_user = RoomModel.GetRoomStatus(obj.name).roomIndex;
        string roomIndex = RoomModel.GetRoomStatus(obj.name).roomIndex.ToString();
        //加入房间
        string response = httpscript.PostHttp(JoinUrl, "token=" + Token + "&roomIndex=" + roomIndex);
        //点击房间后先更新房间状态
        loadRoom();
        updateRoomInfo(index_for_user);
        username = RoomModel.GetRoomStatus(obj.name).username;
        index = roomIndex;
        if(response!="")
        Application.LoadLevel(6);
    }

    private void loadRoom()
    {
        RoomList = new Dictionary<int, RoomStatus>();
        List<RoomStatus> Rooms = httpscript.PostGetRoomListHttp(RoomListUrl, "");
        
        foreach(RoomStatus room in Rooms)
        {
            RoomList.Add(room.roomIndex, room);
        }
        
    }
}
