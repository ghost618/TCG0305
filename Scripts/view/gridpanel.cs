using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridpanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public Transform[] grids;

    public int index;
    public int GetEmptyGridIndex()
    {
        return index;
    }
    private void setindex(int i)
    {
        index = i;
    }

    public Transform GetEmptyGrid()
    {

        for (int i = 0; i < grids.Length; i++)
        {

            if (grids[i].childCount == 0)
            {
                setindex(i);
                return grids[i];

            }

        }

        return null;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
