using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userModel 
{
    public static string user;
    public static string myToken;
    public static void storeName(string name,string Token)
    {
        user = name;
        myToken = Token;
    }
    public static string getName()
    {
        return user;
    }
    public static string getToken()
    {
        return myToken;
    }
}
