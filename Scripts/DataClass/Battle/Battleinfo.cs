using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battleinfo
{
    public Operation operation { set; get; }
    public string sender { get; set; }
    public string receiver { get; set; }
    public int roomindex { get; set; }

    public Battleinfo(string sender,string receiver,int roomindex,Operation operation)
    {
        this.receiver = receiver;
        this.sender = sender;
        this.roomindex = roomindex;
        this.operation = operation;
    }
}
