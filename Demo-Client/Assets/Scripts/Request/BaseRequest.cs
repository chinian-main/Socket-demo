using System;
using SocketGameProtocol;
using UnityEngine;

public class BaseRequest : MonoBehaviour
{
    protected RequestCode requestCode;
    protected ActionCode actionCode;

    protected GameFace face;
    public ActionCode GetActionCode { get => actionCode; }

    protected virtual void Awake()
    {
        face = GameFace.Face;
    
    }
    protected virtual void Start(){
         face.AddRequest(this);
    }
    protected virtual void OnDestroy()
    {
        face.RemoveRequest(actionCode);
    }
    public virtual void OnResponse(MainPack pack)
    {
        // face.HandleResponse(pack);
    }
    public virtual void SendRequest(MainPack pack)
    {
        face.Send(pack);
    }
    public virtual void SendRequestUDP(MainPack pack){
        face.SendTo(pack);
    }

}