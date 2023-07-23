using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;
using UnityEngine.UI;

public class LogonPanel : BasePanel
{
    public LogonRequest logonRequest;
    public InputField user, pwd;
    public Button logonBtn;
    public Button switchBtn;

    private void OnLogonClick()
    {
        if (user.text == "" || pwd.text == "")
        {
            Debug.LogWarning("用户名密码不能为空");
            return;
        }
        logonRequest.SendRequest(user.text, pwd.text);
    }
    private void Start()
    {
        logonBtn.onClick.AddListener(OnLogonClick);
        switchBtn.onClick.AddListener(SwitchToLogin);
    }
    private void SwitchToLogin()
    {
        uIMag.PopPanel();

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
    public void OnResponse(MainPack pack)
    {
        switch (pack.ReturnCode)
        {
            case ReturnCode.Succeed:
                Debug.Log("注册成功");
                uIMag.ShowMessage("注册成功");
                uIMag.PushPanel(PanelType.Login);
                break;
            case ReturnCode.Filled:
                Debug.Log("注册失败");
                uIMag.ShowMessage("注册失败");
                break;
        }

        // base.OnResponse(pack);
    }
}