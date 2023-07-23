using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;

public class UpdatePlayerListRequest : BaseRequest
{
    MainPack pack = null;
    public GamePanel gamePanel;
    protected override void Awake()
    {
        requestCode=RequestCode.Game;
        actionCode=ActionCode.UpdatePlayer;
        base.Awake();
    }
    private void Update() {
        if(pack!=null)
        {
            gamePanel.UpdateList(pack);
            face.RemovePlayer(pack.Str);
            pack=null;
        }
    }
    public override void OnResponse(MainPack pack)
    {
        this.pack=pack;
    }
}
