using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operation 
{
    public int id { get; set; }
    public int type { get; set; }
    public string opera { get; set; }
    public int position { get; set; }

    public Operation(int id,int type,string opera,int position)
    {
        this.id = id;
        this.type = type;
        this.opera = opera;
        this.position = position;
    }
}
