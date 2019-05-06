using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Net;
using System.IO;
using System;

public class loginmanager : MonoBehaviour
  {
    string Token;//令牌信息
    public string LogUrl = "http://127.0.0.1:8080/user/login";
    //toggle
    public Toggle PwdisShow;//是否显示密码
    //注册信息
    public InputField inputUser;
    public InputField inputPwd;

    private void Start()
    {
        PwdisShow.onValueChanged.AddListener(ToggleClick);
        inputPwd.contentType = InputField.ContentType.Password;
    }

    public void ToggleClick(bool isShow)
    {
        inputPwd.contentType = isShow ? InputField.ContentType.Password : InputField.ContentType.Standard;
        inputPwd.Select();
    }

    public void onLoginBtn()
    {
        string username = inputUser.text.Trim();
        string password = inputPwd.text.Trim();
        string reponsecontent = httpscript.PostHttp(LogUrl, "username="+username + "&password="+ password);
        Debug.Log(reponsecontent);
        Token = reponsecontent;
        if (reponsecontent != "failed")
        {
            Application.LoadLevel(2);
            userModel.storeName(username,Token);
        }
        else
        {
            Debug.Log("登陆失败");
        }
    }


}
