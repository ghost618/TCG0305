using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;

public class MenuManager : MonoBehaviour
{
    public Text UserRankInfo;
    // Start is called before the first frame update
    void Start()
    {        
        /*try
        {
            SqlAccess sql = new SqlAccess();
            DataSet ds = sql.Select("rank", new string[] { "userName", "win","total","exp" },
                new string[] { "userName" }, new string[] { "=" }, new string[] { userModel.getName() });
            if (ds != null)
            {
                DataTable table = ds.Tables[0];
                foreach (DataRow dataRow in table.Rows)
                {
                    foreach (DataColumn dataColumn in table.Columns)
                    {
                        //Debug.Log(dataRow[dataColumn]);
                        UserRankInfo.text += "\n" + dataRow[dataColumn].ToString();
                    }
                }
            }
            sql.Close();
        }
        catch
        {
            Debug.Log("数据库操作出错");
        }*/
    }

    
}
