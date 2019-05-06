using System.Collections;

using System.Collections.Generic;

using UnityEngine;

public class cardModel
{
    public static Dictionary<string, Card> gridItem = new Dictionary<string, Card>();

    public static void StoreItem(string cardname, Card card)
    {
        if (gridItem.ContainsKey(cardname))
        {
            DeleteCard(cardname);
        }
        gridItem.Add(cardname, card);
    }

    public static Card GetCard(string cardname)
    {
        if (gridItem.ContainsKey(cardname))
        {
            return gridItem[cardname];
        }
        else
        {
            return null;
        }
    }

    public static void DeleteCard(string cardname)
    {
        if (gridItem.ContainsKey(cardname))
        {
            gridItem.Remove(cardname);
        }
    }
}
