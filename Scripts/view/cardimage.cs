using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cardimage : MonoBehaviour
{
    
    public Text itemText;
    public Text Point;

    //本节以字体为例

    public void UpdateItem(string name)
    {

        itemText.text = name;

    }
    public void UpdatePoint(int point)
    {
        Point.text = point.ToString();
    }


    //实际多是图片，可以使用这个更新图片

    public Image itemImage;

    public void UpdateItemImage(Sprite icon)

    {

        itemImage.sprite = icon;

    }
    
}
