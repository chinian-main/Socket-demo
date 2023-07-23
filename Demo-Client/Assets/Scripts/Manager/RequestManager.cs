using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;

public class RequestManager:BaseManager{
    public RequestManager(GameFace face):base(face){}
    private Dictionary<ActionCode,BaseRequest> requestDict=new Dictionary<ActionCode, BaseRequest>();

    public void AddRequest(BaseRequest request){
        requestDict.Add(request.GetActionCode,request);
    }
    public void RemoveRequest(ActionCode action){
        requestDict.Remove(action);
    }
    public void HandleResponse(MainPack pack){
        // Debug.Log("开始处理响应");
        if(requestDict.TryGetValue(pack.ActionCode,out BaseRequest request)){
           request.OnResponse(pack); 
        }
        else{
            Debug.LogWarning("找不到对应的处理"+pack);
        }
    }
}    
