using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour,IEnableRecycled
{
    public event Action< GameObject> onRecycleEvent;
    public LayerMask layerMask;
    public  void Init() {
        Invoke("OnStartRecycle",2);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log(other.gameObject.layer.ToString()+","+layerMask.value);
        int flag=(layerMask.value>>other.gameObject.layer)&1;
        Debug.Log(flag);
        if(flag==1){
            Debug.Log("撞击后回收");
             OnStartRecycle();
        }   
    }
    
    public void OnStartRecycle()
    { 
        if(onRecycleEvent!=null){   
           // Debug.Log("开始回收");
            onRecycleEvent(this.gameObject); 
            onRecycleEvent=null;
        }
        else{
           // Debug.Log("不需要回收");
        }
    }

}
