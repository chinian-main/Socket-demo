using SocketGameProtocol;
using UnityEngine;

public class LogonRequest : BaseRequest 
{
    public LogonPanel logonPanel;
    private MainPack pack;
    protected override void Start(){
        requestCode=SocketGameProtocol.RequestCode.Uesr;
        actionCode=SocketGameProtocol.ActionCode.Logon;
        base.Start();
    }
    public override void OnResponse(MainPack pack)
    {
        this.pack=pack;
        // base.OnResponse(pack);
    }
    private void Update() {
        if(pack!=null){
            logonPanel.OnResponse(pack);
            pack=null;
        }
    }
    public  void SendRequest(string username,string password)
    {
        MainPack pack=new MainPack();
        pack.RequestCode=requestCode;
        pack.ActionCode=actionCode;
        LoginPack loginPack=new LoginPack();
        loginPack.Username=username;
        loginPack.Password=password;
        pack.LoginPack=loginPack;
        base.SendRequest(pack);
    }






   

}