using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocol;
public class FireRequest : BaseRequest
{
    private MainPack pack=null;
    protected override void Awake() {
        requestCode=RequestCode.Game;
        actionCode =ActionCode.Fire;
        base.Awake();
    }
    public void SendRequest(Vector2 pos,float rot,Vector2 mousePos)
    {
        MainPack pack=new MainPack();
        BulletPack bulletPack=new BulletPack();
        bulletPack.PosX=pos.x;
        bulletPack.PosY=pos.y;
        bulletPack.MousePosX=mousePos.x;
        bulletPack.MousePosY=mousePos.y;
        bulletPack.RotZ=rot;
        pack.BulletPack=bulletPack;
        pack.RequestCode=requestCode;
        pack.ActionCode=actionCode;
        base.SendRequest(pack);
    }
    private void Update() {
        if(pack!=null){
            face.SpawnBullet(pack);
            pack=null;
        }
    }
    public override void OnResponse(MainPack pack)
    {
       this.pack=pack;
    }
}
