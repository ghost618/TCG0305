using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duel
{
    public int[] score { get; set; }
    public int[] point { get; set; }
    public List<int>[] handCards { get; set; }
    public List<int>[] decks { get; set; }
    public List<int>[] frontCards { get; set; }
    public List<int>[] behindCards { get; set; }
    public int[] functionCards { get; set; }
    public int CurPlayer { get; set; }

    public Duel(int[] score, int[] point, List<int>[] handCards, 
        List<int>[] decks, List<int>[] frontCards, List<int>[] behindCards, int []functionCards,int CurPlayer)
    {
        this.score = score;
        this.point = point;
        this.handCards = handCards;
        this.decks = decks;
        this.frontCards = frontCards;
        this.behindCards = behindCards;
        this.functionCards = functionCards;
        this.CurPlayer = CurPlayer;
    }
}
