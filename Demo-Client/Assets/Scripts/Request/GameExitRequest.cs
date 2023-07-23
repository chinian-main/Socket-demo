using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;

public class GameExitRequest : BaseRequest
{

    private MainPack pack=null;
    protected override void Awake() {
        requestCode= RequestCode.Game;
        actionCode=ActionCode.ExitGame;
        base.Awake();
    }

    public void SendRequest(){
        MainPack pack= new MainPack();
        pack.RequestCode=requestCode;
        pack.ActionCode=actionCode;
        pack.Str="r";
        base.SendRequest(pack);
    }
    private void Update() {
        if(pack!=null){
            Debug.Log("接受到了退出的信息");
            face.GameExit();

            pack=null;
        }
    }
    public override void OnResponse(MainPack pack)
    {
    
        this.pack=pack;
    }
}
