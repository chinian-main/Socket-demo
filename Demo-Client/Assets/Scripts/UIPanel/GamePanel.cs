using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public GameObject item;
    public Transform listTransform;
    public Text Timetext;
    public Button exitBtn;
    public GameExitRequest gameExitRequest;
    private float curtime;
    private float startTime;
    private Dictionary<string,PlayerInfoItem> itemList=new Dictionary<string, PlayerInfoItem>();

    private void Start(){
        startTime=Time.time;
        exitBtn.onClick.AddListener(OnExitClick);
    }
    private void OnExitClick(){
        gameExitRequest.SendRequest();
        face.GameExit();
        
    }
    public void UpdateValue(string id,int v){
        if(itemList.TryGetValue(id,out PlayerInfoItem item)){
            item.Up(v);
        }
        else {
            Debug.Log("玩家不存在");
        }
    }
    public void UpdateList(MainPack packs){
        for(int i=0;i<listTransform.childCount;i++){
            Destroy(listTransform.GetChild(i).gameObject);
        }
        itemList.Clear();
        foreach (var p in packs.Playerpack)
        {
            GameObject g= Instantiate(item,Vector3.zero,Quaternion.identity,listTransform);
            g.transform.SetParent(listTransform);
            PlayerInfoItem pInfo= g.GetComponent<PlayerInfoItem>();
            pInfo.Set(p.Playername,p.Hp);
            itemList.Add(p.Playername,pInfo);
        }
    }
     private void FixedUpdate() {
         Mathf.Clamp(Time.time-startTime,0,300);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        Enter();
    }
    public override void OnPasue()
    {
        base.OnPasue();
        Exit();
    }
    public override void OnExit()
    {
        base.OnExit();
        Exit();
    }
    public override void OnRecovery()
    {
        base.OnRecovery();
        Enter();
    }
    private void Enter()
    {
        gameObject.SetActive(true);
    }
    private void Exit()
    {
        gameObject.SetActive(false);
    }
}
