using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomModel 
{
    public static Dictionary<string, RoomStatus> RoomItem = new Dictionary<string, RoomStatus>();

    public static void StoreRoom(string roomName, RoomStatus RoomStatus)
    {
        if (RoomItem.ContainsKey(roomName))
        {
            DeleteRoomStatus(roomName);
        }
        RoomItem.Add(roomName, RoomStatus);
    }

    public static RoomStatus GetRoomStatus(string roomName)
    {
        if (RoomItem.ContainsKey(roomName))
        {
            return RoomItem[roomName];
        }
        else
        {
            return null;
        }
    }

    public static void DeleteRoomStatus(string roomName)
    {
        if (RoomItem.ContainsKey(roomName))
        {
            RoomItem.Remove(roomName);
        }
    }
}
