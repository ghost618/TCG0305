using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck 
{   
    //public string userName { get; set; }
    public int []cardId { get; set; }
    public int []count { get; set; }

    public Deck(int []cardId,int []count)
    {
        //this.userName = userName;
        this.cardId = cardId;
        this.count = count;
    }

}
