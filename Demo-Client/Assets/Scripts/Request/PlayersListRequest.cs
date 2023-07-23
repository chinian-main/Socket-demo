using SocketGameProtocol;
using UnityEngine;

public class PlayersListRequest : BaseRequest
{
    private MainPack pack=null;
    public RoomPanel roomPanel;
    override protected void Awake()
    {
        actionCode=SocketGameProtocol.ActionCode.PlayerList;
        base.Awake();
    }
    private void Update() {
        if(pack!=null){
            roomPanel.UpdatePlayerList(this.pack);
            pack=null;
        }
    }
    public override void OnResponse(MainPack pack)
    {
        this.pack=pack;
        base.OnResponse(pack);
    }
}