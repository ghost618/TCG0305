using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStatus :Room
{
   public int ready { get;set; }
   public List<string> username { get; set; }
   public RoomStatus(int roomIndex,int ready, List<string> username):
    base(roomIndex){
        this.ready = ready;
        this.username = username;
    }
    
}
