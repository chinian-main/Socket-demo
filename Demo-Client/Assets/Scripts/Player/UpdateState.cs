using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateState : MonoBehaviour {
    private UpdateStateRequest updateStateRequest;
    private Transform gunTransfrom;
    private void Awake() {
        if(updateStateRequest==null){
            updateStateRequest=transform.gameObject.AddComponent<UpdateStateRequest>();
        }
        gunTransfrom=transform.Find("HandGun/Gun");
    }
    private void Start() {
        
        
        InvokeRepeating("UpdateStateFunc",1,1f/10f);
    }
    private void UpdateStateFunc(){

        Vector2 pos=transform.position;
        float PlayerRot=transform.eulerAngles.z;
        float gunRot = gunTransfrom.eulerAngles.z;
        
        updateStateRequest.SendRequest(pos,PlayerRot,gunRot);
    }
}