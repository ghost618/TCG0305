using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserVO 
{
    public string username { get; set; }
    public int roomindex { get; set; }

    public UserVO(string username,int roomindex)
    {
        this.username = username;
        this.roomindex = roomindex;
    }
}
