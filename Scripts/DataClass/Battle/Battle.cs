using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle 
{
    public int roomindex { get; set; }
    public List<UserVO> players { get; set; }
    public int ready { get; set; }
    public Duel duel { get; set; }

    public Battle(int roomindex, List<UserVO> players,int ready,Duel duel)
    {
        this.roomindex = roomindex;
        this.players = players;
        this.ready = ready;
        this.duel = duel;
    }
}
