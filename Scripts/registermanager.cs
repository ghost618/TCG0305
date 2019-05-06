using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;

public class registermanager : MonoBehaviour
{   //URL信息
    public string RegUrl = "http://127.0.0.1:8080/user/doreg";
    //注册信息
    public InputField inputUser;
    public InputField inputPwd;
    public InputField inputEmail;

    

    public void onYesBtn()
    {
        string username = inputUser.text.Trim();
        string password = inputPwd.text.Trim();
        string email = inputEmail.text.Trim();
        string reponsecontent = httpscript.PostHttp(RegUrl, "username=" + username + "&password=" + password + "&email=" + email);
        Debug.Log(reponsecontent);
        if (reponsecontent != "failed")
        {
            Debug.Log("注册成功");
            try {
                SqlAccess sql = new SqlAccess();
                DataSet ds = sql.InsertInto("rank", new string[] { "userName" }, new string[] { username });
                sql.Close();
            }
            catch
            {
                Debug.Log("数据库操作出错");
            }
            
        }
        else
        {
            Debug.Log("注册失败");
        }
    }

    
}
