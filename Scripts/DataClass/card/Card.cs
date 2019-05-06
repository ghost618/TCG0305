using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Card 
{
    public int idcards { get; private set; }
    public string cardname { get; private set; }
    public string cardinfo { get; private set; }
    public int cardtype { get; protected set; }
    public int cardpoint { get; private set; }
    //public string cardicon { get; private set; }
    public Card(int idcards, string cardname, int cardpoint, int cardtype,string cardinfo)
    {
        this.idcards = idcards;
        this.cardname = cardname;
        this.cardinfo = cardinfo;
        this.cardtype = cardtype;
        this.cardpoint = cardpoint;
        //this.cardicon = cardicon;
    }
}
