using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roomimage : MonoBehaviour
{
    public Text roomnumber;
    //更新房间号
    public void updatetext(string number)
    {
        roomnumber.text = number;
    }
}
