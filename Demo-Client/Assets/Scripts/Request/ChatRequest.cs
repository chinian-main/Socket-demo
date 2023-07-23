using SocketGameProtocol;
using UnityEngine;

public class ChatRequest : BaseRequest {
    private string chatStr=null;
    public RoomPanel roomPanel;

    protected override void Awake() {
        requestCode=SocketGameProtocol.RequestCode.Room;
        actionCode=SocketGameProtocol.ActionCode.Chat;
        base.Awake();
    }
    public void SendRequest(string str){
        MainPack pack=new MainPack();
        pack.RequestCode=requestCode;
        pack.ActionCode=actionCode;
        pack.Str=str;
        base.SendRequest(pack);
    }
    private void Update(){
        if(chatStr!=null){
            roomPanel.ChatResponse(chatStr);
            chatStr=null;
        }
    }
    public override void OnResponse(MainPack pack)
    {
        chatStr = pack.Str;
    }
}