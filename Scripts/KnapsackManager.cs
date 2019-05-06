using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;


public class KnapsackManager : MonoBehaviour
{
    public string GetAllcardurl = "http://127.0.0.1:8080/card/getAll";
    public int cardcount = 0;
    public gridpanel gridPanel;
    public gridpanel gridpanelDeck;
    private Dictionary<int, Card> CardList;
    //private Dictionary<int, Card> CardListDeck = new Dictionary<int, Card>(); 
    private static KnapsackManager instance;

    public static KnapsackManager Instance
    {
        get
        {
            return instance;
        }
    }
    //获取tooltip组件
    public Tooltip tooltip;
    private bool isShow = false;

    private void Start()
    {
        tooltip.Hide();
    }

    void Awake()
    {
        instance = this;
        this.LoadData();
        gridimage.OnClickL = null;
        gridimage.OnClickR = null;
        gridimage.OnEnter = null;
        gridimage.OnExit = null;

        //添加进入退出事件和点击事件
        gridimage.OnEnter += gridimage_OnEnter;
        gridimage.OnExit += gridimage_OnExit;
        //鼠标左键点击事件
        gridimage.OnClickL += gridimage_OnClickL;
        gridimage.OnClickR += gridimage_OnClickR;
    }

    private void Update()
    {
        if(isShow == true) //屏幕坐标转为ui坐标
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GameObject.Find("KnapsackUI").transform as RectTransform,
                Input.mousePosition, null, out position);

            //设置提示框位置并显示
            tooltip.SetLocationPosition(position);
            tooltip.Show();
        }
    }

    
    private void gridimage_OnEnter(Transform obj)
    {
        Card card = cardModel.GetCard(obj.name);
        if(card == null)
        {
            return;  //获取对应gridimage的card
        }
        string text = GetTiptoolText(card);
        tooltip.UpdateToolTip(text);
        isShow = true;
        Debug.Log(text);
        
    }

    private void gridimage_OnExit()
    {
        isShow = false;
        tooltip.Hide();
    }

    private void gridimage_OnClickL(Transform obj)
    {
        if (obj.name.ToString().Contains("gridimage"))
        {
            Card card = cardModel.GetCard(obj.name);
            if (card == null)
            {
                Debug.Log("没有获取到卡片对象");
                return;  //获取对应gridimage的card
            }
            StoreItemDeck(card.idcards);
        }
        
    }

    private void gridimage_OnClickR(Transform obj)
    {
        DeleteItemDeck();
        if (obj.name.ToString().Contains("battledeck"))
        {
            cardModel.DeleteCard(obj.name);
            Debug.Log(obj.parent);
            obj.transform.DetachChildren();
        }
        
        //Debug.Log(obj.transform);
        
        
    }
    private string GetTiptoolText(Card card)
    {
        if (card == null)
            return "";
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("<color=red>{0}</color>\n\n", card.cardname);
        switch (card.cardtype)
        {
            case 0:
                Card pc = card as Card;
                sb.AppendFormat("战斗力：{0}\n\n", pc.cardpoint);
                break;
            case 1:
                Card pf = card as Card;
                sb.AppendFormat("战斗力：{0}\n\n", pf.cardpoint);
                break;
            default:
                break;
        }
        sb.AppendFormat("<size=20><color=white>卡片id：{0}" +
            "</color></size>\n\n<color=yellow><size=20>描述：{1}</size></color>",
            card.idcards, card.cardinfo);
        return sb.ToString();
    }


    public void StoreItem(int cardid)
    {
        if (!CardList.ContainsKey(cardid))
        {
            return;
        }

        Transform emptyGrid = gridPanel.GetEmptyGrid();
        if (emptyGrid == null)
        {
            Debug.Log("背包已满");
            return;
        }
        Card card = CardList[cardid];

        GameObject cardPrefab = Resources.Load<GameObject>("Prefabs/cardimage");

        cardPrefab.GetComponent<cardimage>().UpdateItem(card.cardname);
        cardPrefab.GetComponent<cardimage>().UpdatePoint(card.cardpoint);

        GameObject cardGo = GameObject.Instantiate(cardPrefab);

        cardGo.transform.SetParent(emptyGrid);

        cardGo.transform.localPosition = Vector3.zero;

        cardGo.transform.localScale = Vector3.one;

        //存储信息到cardModel中的字典中去
        cardModel.StoreItem(emptyGrid.name, card);
    }


    //储存对战卡组的卡牌
    public void StoreItemDeck(int cardid)
    {
        if (!CardList.ContainsKey(cardid))
        {
            return;
        }

        Transform emptyGrid = gridpanelDeck.GetEmptyGrid();
        if (emptyGrid == null)
        {
            Debug.Log("背包已满");
            return;
        }
        Card card = CardList[cardid];

        GameObject cardPrefab = Resources.Load<GameObject>("Prefabs/cardimage");
        
        cardPrefab.GetComponent<cardimage>().UpdateItem(card.cardname);
        cardPrefab.GetComponent<cardimage>().UpdatePoint(card.cardpoint);

        GameObject cardGo = GameObject.Instantiate(cardPrefab);
        //Debug.Log(emptyGrid);

        cardGo.transform.SetParent(emptyGrid);

        cardGo.transform.localPosition = Vector3.zero;

        cardGo.transform.localScale = Vector3.one;

        //存储信息到cardModel中的字典中去
        cardModel.StoreItem(emptyGrid.name, card);
    }
    //删除对战卡牌
    public void DeleteItemDeck()
    {
        //gridpanelDeck.grids = null;
    }


    //从服务器加载卡片数据

    private void LoadData()
    {

        CardList = new Dictionary<int, Card>();        

        //仓库所有卡牌加载
        List<Card> cards = httpscript.PostGetAllCardHttp(GetAllcardurl, "");
        foreach(Card card in cards)
        {
            CardList.Add(card.idcards, card);
            Debug.Log(card.idcards + " " + card.cardname + " " + card.cardpoint);
            cardcount++;
        }

    }

}

