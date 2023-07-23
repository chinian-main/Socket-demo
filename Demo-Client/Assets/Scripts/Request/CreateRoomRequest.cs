using UnityEngine;
using SocketGameProtocol;
public class CreateRoomRequest : BaseRequest {

    public RoomListPanel roomListPanel;
    MainPack pack=null;
    protected override void Awake(){
        requestCode=RequestCode.Room;
        actionCode=ActionCode.CreateRoom;
        base.Awake();
    }

    private void Update() {
        if(pack!=null){
            roomListPanel.CreateRoomResponse(pack);
            pack=null;
        }
    }
    public void SendRequest(string roomname,int maxnum){
        MainPack pack=new MainPack();
        pack.RequestCode=requestCode;
        pack.ActionCode=actionCode;
        RoomPack room =new RoomPack();
        room.Roomname=roomname;
        room.Maxnum=maxnum;
        pack.Roompack.Add(room);
        base.SendRequest(pack);
    }
    public override void OnResponse(MainPack pack)
    {
        this.pack=pack;
    }
}