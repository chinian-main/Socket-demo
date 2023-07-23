using UnityEngine;
using SocketGameProtocol;
public class LoginRequest : BaseRequest
{
    public LoginPanel loginPanel;
    private MainPack pack = null;
    protected override void Awake()
    {
        requestCode = RequestCode.Uesr;
        actionCode = ActionCode.Login;
        base.Awake();
    }
    private void Update()
    {
        if (pack != null)
        {
            loginPanel.OnResponse(pack);
            pack=null;
        }
    }
    public override void OnResponse(MainPack pack)
    {
        //   loginPanel.OnResponse(pack);
        this.pack = pack;

    }
    public void SendRequest(string username, string password)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        LoginPack loginPack = new LoginPack();
        loginPack.Username = username;
        loginPack.Password = password;
        pack.LoginPack = loginPack;
        base.SendRequest(pack);
    }

}