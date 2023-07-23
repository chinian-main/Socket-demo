using SocketGameProtocol;
using UnityEngine;

public class ExitRoomRequest : BaseRequest
{
    private bool isExit=false;
    public RoomPanel roomPanel;
    protected override void Awake()
    {
        requestCode=SocketGameProtocol.RequestCode.Room;
        actionCode=SocketGameProtocol.ActionCode.Exit;
        base.Awake();
    }
    private void Update() {
        if(isExit){
            roomPanel.ExitRoomResponse();
            isExit=false;
        }
    }
    public void SendRequest(){
        MainPack pack=new MainPack();
        pack.RequestCode=requestCode;
        pack.ActionCode=actionCode;
        pack.Str="r";
        base.SendRequest(pack);
    }
    public override void OnResponse(MainPack pack)
    {
        isExit=true;
    }
}