using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatMessage 
{
    public string sender { set; get; }
    public string receiver { set; get; }
    public string message { set; get; }

    public ChatMessage(string sender,string receiver,string message)
    {
        this.sender = sender;
        this.receiver = receiver;
        this.message = message;
    }
}
