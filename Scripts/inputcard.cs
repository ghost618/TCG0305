using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class inputcard : MonoBehaviour
{
    public string deckurl = "http://127.0.0.1:8080/deck/getdeck1";
    void Start()
    {
        for(int i = 1; i <= KnapsackManager.Instance.cardcount; i++)
        {
            KnapsackManager.Instance.StoreItem(i);
        }
        Deck n = httpscript.PostDeckHttp(deckurl, "username=" + userModel.getName(), "application/x-www-form-urlencoded");
        for (int i =0;i< n.cardId.Length; i++)
            for(int j = 0; j < n.count[i]; j++)
            {
                KnapsackManager.Instance.StoreItemDeck(i+1);
            }
    }
    public Text itemText;

    //本节以字体为例

    public void UpdateItem(string name)
    {

        itemText.text = name;
        
    }

    //实际多是图片，可以使用这个更新图片

    [HideInInspector]

    public Image itemImage;

    public void UpdateItemImage(Sprite icon)

    {

        itemImage.sprite = icon;

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            int cardid = Random.Range(1, 5);
            KnapsackManager.Instance.StoreItem(cardid);
            
        }

        /*if (Input.GetMouseButtonDown(0))
        {
            int cardid = Random.Range(1, 5);
            KnapsackManager.Instance.StoreItemDeck(cardid);
        }*/
        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (var item in cardModel.gridItem)
            {
                print(item);
            }
        }
    }

}
