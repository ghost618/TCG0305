using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newDeck 
{
    public int[] decks { get; set; }
    public string username { get; set; }

    public newDeck(int[] decks,string username)
    {
        this.decks = decks;
        this.username = username;
    }
}
