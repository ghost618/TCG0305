using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoom : Room
{
    public string Token { get; set;}
    public JoinRoom(int roomIndex,string Token):
        base(roomIndex){
            this.Token = Token;
        }
    
}
