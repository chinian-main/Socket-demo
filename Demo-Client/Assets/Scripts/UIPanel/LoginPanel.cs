using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    public LoginRequest loginRequest;
    public InputField user, pwd;
    public Button loginBtn;
    public Button switchBtn;


    private void Start()
    {
        loginBtn.onClick.AddListener(OnLoginClick);
        switchBtn.onClick.AddListener(SwitchToLogon);
    }
    private void SwitchToLogon()
    {
        uIMag.PushPanel(PanelType.Logon);
    }
    private void OnLoginClick()
    {
        if (user.text == "" || pwd.text == "")
        {
            Debug.LogWarning("用户名密码不能为空");
            return;
        }
        loginRequest.SendRequest(user.text, pwd.text);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        Enter();
    }
    public override void OnPasue()
    {
        base.OnPasue();
        Exit();
    }
    public override void OnExit()
    {
        base.OnExit();
        Exit();
    }
    public override void OnRecovery()
    {
        base.OnRecovery();
        Enter();
    }
    private void Enter()
    {
        gameObject.SetActive(true);
    }
    private void Exit()
    {
        gameObject.SetActive(false);
    }
    public void OnResponse(MainPack pack){
          switch (pack.ReturnCode)
        {
            case ReturnCode.Succeed:
                uIMag.ShowMessage("登录成功");
                face.UserName=user.text;
                uIMag.PushPanel(PanelType.RoomList);
                break;
            case ReturnCode.Filled:
                uIMag.ShowMessage("登录失败");
                break;
        }
    
    }
}