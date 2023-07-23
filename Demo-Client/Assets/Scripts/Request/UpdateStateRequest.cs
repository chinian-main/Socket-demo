using SocketGameProtocol;
using UnityEngine;

public class UpdateStateRequest : BaseRequest
{
    // MainPack pack =null;
    protected override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode =ActionCode.UpState;
        base.Awake();
    }
    public void SendRequest(Vector2 pos,float playerRot,float GunRot){
        MainPack pack=new MainPack();
        PosPack posPack =new PosPack();
        PlayerPack playerPack = new PlayerPack();
        posPack.PosX=pos.x;
        posPack.PosY=pos.y;
        posPack.RotZ=playerRot;
        posPack.GunRotZ=GunRot;
        playerPack.Playername=face.UserName;
        playerPack.Pospack=posPack;
        pack.Playerpack.Add(playerPack);
        pack.RequestCode=requestCode;
        pack.ActionCode=actionCode;
        base.SendRequestUDP(pack);
    }
    private void Update() {
        // if(pack!=null){

        
        //     pack=null;
        // }
    }
    public override void OnResponse(MainPack pack)
    {  
          face.UpdateState(pack);
        // this.pack=pack;
    }
}