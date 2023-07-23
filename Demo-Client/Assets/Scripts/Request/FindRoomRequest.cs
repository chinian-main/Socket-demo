using SocketGameProtocol;
using UnityEngine;

public class FindRoomRequest : BaseRequest
{
    public RoomListPanel roomListPanel;
    private MainPack pack = null;
    protected override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.FindRoom;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            roomListPanel.FindRoomResponse(pack);
            pack = null;
        }
    }
    public void SendRequest()
    {
        
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.Str = "r";
        base.SendRequest(pack);
    }
    public override void OnResponse(MainPack pack)
    {
        this.pack = pack;
    }
}