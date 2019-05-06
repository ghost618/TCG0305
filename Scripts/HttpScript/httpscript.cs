using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;


public class httpscript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //body是要传递的参数,格式"roleId=1&uid=2"
    //post的cotentType填写:"application/x-www-form-urlencoded"
    //soap填写:"text/xml; charset=utf-8"
    public static string PostHttp(string url, string body)
    {
        HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);

        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 20000;

        byte[] btBodys = Encoding.UTF8.GetBytes(body);
        httpWebRequest.ContentLength = btBodys.Length;
        httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

        HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
        string responseContent = streamReader.ReadToEnd();

        httpWebResponse.Close();
        streamReader.Close();
        httpWebRequest.Abort();
        httpWebResponse.Close();

        return responseContent;
    }
    //json数据传输 n
    public static Deck PostDeckHttp(string url, string body, string contentType)
    {
        HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);

        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 20000;

        byte[] btBodys = Encoding.UTF8.GetBytes(body);
        httpWebRequest.ContentLength = btBodys.Length;
        httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

        HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
        string  responseContent = streamReader.ReadToEnd();
        Deck dk = JsonConvert.DeserializeObject<Deck>(responseContent);

        httpWebResponse.Close();
        streamReader.Close();
        httpWebRequest.Abort();
        httpWebResponse.Close();

        return dk;
    }


    public static string PostCreateDeckHttp(string url, string body)
    {
        HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);

        httpWebRequest.ContentType = "application/json";
        
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 20000;

        byte[] btBodys = Encoding.UTF8.GetBytes(body);
        httpWebRequest.ContentLength = btBodys.Length;
        httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

        HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
        string responseContent = streamReader.ReadToEnd();
        //Debug.Log(responseContent);

        httpWebResponse.Close();
        streamReader.Close();
        httpWebRequest.Abort();
        httpWebResponse.Close();

        return responseContent;
    }
    public static List<Card> PostGetAllCardHttp(string url, string body)
    {
        HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);

        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 20000;

        byte[] btBodys = Encoding.UTF8.GetBytes(body);
        httpWebRequest.ContentLength = btBodys.Length;
        httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

        HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
        string responseContent = streamReader.ReadToEnd();
        List<Card> cards = JsonConvert.DeserializeObject<List<Card>>(responseContent);
        

        httpWebResponse.Close();
        streamReader.Close();
        httpWebRequest.Abort();
        httpWebResponse.Close();

        return cards;
    }

    public static List<RoomStatus> PostGetRoomListHttp(string url, string body)
    {
        HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);

        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 20000;

        byte[] btBodys = Encoding.UTF8.GetBytes(body);
        httpWebRequest.ContentLength = btBodys.Length;
        httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

        HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
        string responseContent = streamReader.ReadToEnd();
        List<RoomStatus> Rooms = JsonConvert.DeserializeObject<List<RoomStatus>>(responseContent);


        httpWebResponse.Close();
        streamReader.Close();
        httpWebRequest.Abort();
        httpWebResponse.Close();

        return Rooms;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
