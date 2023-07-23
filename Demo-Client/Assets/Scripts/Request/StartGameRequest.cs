using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;

public class StartGameRequest : BaseRequest
{
    private MainPack pack=null;
    public RoomPanel roomPanel;
    protected override void Awake()
    {
        requestCode = SocketGameProtocol.RequestCode.Room;
        actionCode =SocketGameProtocol.ActionCode.StartGame;
        base.Awake();
    }
    public void SendRequest(){
        MainPack pack=new MainPack();
        pack.RequestCode=requestCode;
        pack.ActionCode=actionCode;
        pack.Str="r";
        base.SendRequest(pack);
    }
    private void Update(){
        if(pack!=null){
            roomPanel.StartGameResponse(pack);
            pack=null;
        }
    }
    public override void OnResponse(MainPack pack)
    {
        this.pack=pack;
    }

}
