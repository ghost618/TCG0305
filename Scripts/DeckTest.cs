using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEngine.UI;

public class DeckTest : MonoBehaviour
{
    public string deckurl = "http://127.0.0.1:8080/deck/getdeck1";
    public string NewDeckurl = "http://127.0.0.1:8080/deck/updatedeck";
    public string DeleteDeckurl = "http://127.0.0.1:8080/deck/deletedeck";
    public string GetAllcardurl = "http://127.0.0.1:8080/card/getAll";
    string user = userModel.getName();

    public Text Tip;

    private int MagicCardCount;
    
    // Start is called before the first frame update
    void Start()
    {
        Deck dk = httpscript.PostDeckHttp(deckurl, "username=" + user, "application/x-www-form-urlencoded");
        Debug.Log(dk) ;
        
        Debug.Log(httpscript.PostHttp(deckurl, "username=" + user));
    }

    public void onCreateDeckBtn()
    {
        int[] newdeck = new int[20];
        MagicCardCount = 0;
        bool MagicOK = true;
        bool PersonOK = true;
        Hashtable LimitPersonCardCount = new Hashtable();
        try
        {
            for(int i = 0; i < 20; i++)
            {
                int type = cardModel.GetCard(KnapsackManager.Instance.gridpanelDeck.grids[i].name).cardtype;
                int id = cardModel.GetCard(KnapsackManager.Instance.gridpanelDeck.grids[i].name).idcards;

                switch (type){                    
                    case 2:
                        MagicCardCount++;
                        if (MagicCardCount <= 4)
                        {
                            newdeck[i] = id;                         
                        }
                        else
                        {
                            MagicOK = false;
                        }
                        
                        break;
                    default:
                        if (!LimitPersonCardCount.Contains(id))
                        {
                            LimitPersonCardCount.Add(id, 1);
                        }                        
                        else if (LimitPersonCardCount.Contains(id)){
                            LimitPersonCardCount[id] = Convert.ToInt32(LimitPersonCardCount[id])+1;
                        }
                        if (Convert.ToInt32(LimitPersonCardCount[id]) < 3)
                        {
                            newdeck[i] = id;
                        }
                        else
                        {
                            PersonOK = false;
                        }
                        break;
                }                                
                
            }
            
        }
        catch
        {
            print("卡组必须要有20张卡片");
            Tip.text = "卡组必须要有20张卡片";
        }
                        
        newDeck ndk1 = new newDeck(newdeck, user);
        string NewDeckData = JsonConvert.SerializeObject(ndk1);
        Debug.Log(NewDeckData);
        if (MagicOK && PersonOK)
        {
            Debug.Log(httpscript.PostCreateDeckHttp(NewDeckurl, NewDeckData));
            Tip.text = "卡组创建成功";
        }
        else
        {
            Debug.Log("请检查卡组配置，单位卡牌同种最多三张，魔法卡不能超过4张");
            Tip.text = "请检查卡组配置，单位卡牌同种最多三张，魔法卡不能超过4张";
        }
        
    }

    public void onDeleteDeckBtn()
    {
        Debug.Log(httpscript.PostHttp(DeleteDeckurl, "username=" + user));
        for(int i = 0; i < 20; i++)
        {
            cardModel.DeleteCard(KnapsackManager.Instance.gridpanelDeck.grids[i].name);
            KnapsackManager.Instance.gridpanelDeck.grids[i].transform.DetachChildren();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
