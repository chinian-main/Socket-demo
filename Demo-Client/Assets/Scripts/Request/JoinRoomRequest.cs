using SocketGameProtocol;
using UnityEngine;

public class JoinRoomRequest : BaseRequest
{
    MainPack pack=null;
    public RoomListPanel roomListPanel;
    protected override void Awake()
    {
        requestCode = SocketGameProtocol.RequestCode.Room;
        actionCode = SocketGameProtocol.ActionCode.JoinRoom;
        base.Awake();
    }
    public void SendRequest(string roomname)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.Str = roomname;
        base.SendRequest(pack);
    }

    
    public override void OnResponse(MainPack pack)
    {
        this.pack = pack;
        // base.OnResponse(pack);
    }
    private void Update()
    {
        if (pack != null)//异步
        {
            roomListPanel.JoinRoomResponse(pack);
            pack = null;
        }
    }
}