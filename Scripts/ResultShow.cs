using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;

public class ResultShow : MonoBehaviour
{
    public Text result;
    public Text ResultList;
    // Start is called before the first frame update
    void Start()
    {
        result.text = BattleManager.Winner;
        /*try
        {
            SqlAccess sql = new SqlAccess();
            DataSet ds = sql.Select("rank", new string[] { "userName", "win", "total", "exp" },
                new string[] { "userName" }, new string[] { "=" }, new string[] { userModel.getName() });
            if (ds != null)
            {
                DataTable table = ds.Tables[0];
                foreach (DataRow dataRow in table.Rows)
                {
                    foreach (DataColumn dataColumn in table.Columns)
                    {
                        //Debug.Log(dataRow[dataColumn]);
                        //UserRankInfo.text += "\n" + dataRow[dataColumn].ToString();
                        if(BattleManager.winnername == userModel.getName())
                        {
                            if (dataColumn.ToString() == "win")
                            {
                                int count = System.Convert.ToInt32(dataRow[dataColumn]);
                                count += 1;
                                ds = sql.UpdateInto("rank", new string[] { "win", "total" }, new string[] { count.ToString(), count.ToString() },
                                   "userName", userModel.getName());
                            }
                        }
                        else
                        {
                            if (dataColumn.ToString() == "total")
                            {
                                int count = System.Convert.ToInt32(dataRow[dataColumn]);
                                count += 1;
                                ds = sql.UpdateInto("rank", new string[] { "total" }, new string[] {count.ToString() },
                                   "userName", userModel.getName());
                            }
                        }
                        
                        if (dataColumn.ToString() == "exp")
                        {
                            int count = System.Convert.ToInt32(dataRow[dataColumn]);
                            count += 10;
                            ds = sql.UpdateInto("rank", new string[] { "exp" }, new string[] { count.ToString() },
                               "userName", userModel.getName());
                        }
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
    public float timmer = 3.0f;
    // Update is called once per frame
    void Update()
    {
        timmer -= Time.deltaTime;
        if (timmer <= 0)
        {
            Application.LoadLevel(6);
        }
    }
}
