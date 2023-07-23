using System;
using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;

public class GameFace : MonoBehaviour
{

    public Canvas UI;
    private ClientManager clientManager;
    private UIManager uIManager;
    private RequestManager requestManager;
    private PlayerManager playerManager;
    public string UserName
    {
        set; get;
    }
    public MyPool BulletPool{
        get=>playerManager.BulletPool;
    }
    private static GameFace _instance;
    public static GameFace Face
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("GameFace").GetComponent<GameFace>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(this.UI.gameObject);
        _instance = this;
        clientManager = new ClientManager(this);
        requestManager = new RequestManager(this);
        uIManager = new UIManager(this);
        playerManager = new PlayerManager(this);
        uIManager.OnInit();
        requestManager.OnInit();
        clientManager.OnInit();
        playerManager.OnInit();

    }
    private void OnDestroy()
    {
        clientManager.OnDestroy();
        requestManager.OnDestroy();
        uIManager.OnDestroy();
        playerManager.OnDestroy();
    }
    void Update()
    {

    }
    public void HandleResponse(MainPack pack)
    {
        requestManager.HandleResponse(pack);
    }
    public void Send(MainPack pack)
    {
        clientManager.Send(pack);
    }
    public void AddRequest(BaseRequest request)
    {
        requestManager.AddRequest(request);
    }
    public void RemoveRequest(ActionCode action)
    {
        requestManager.RemoveRequest(action);
    }
    public void ShowMessage(string str, bool sync = false)
    {
        uIManager.ShowMessage(str, sync);
    }
    public void SetSelfID(string id)
    {
        playerManager.CurPlayerID = id;
    }
    public void AddPlayer(MainPack packs)
    {
        playerManager.AddPlayer(packs);
    }
    public void RemovePlayer(string id)
    {
        playerManager.RemovePlayer(id);
    }
    public void GameExit(){
        playerManager.GameExit();
        uIManager.PopPanel();
        uIManager.PopPanel();
    }
    public void UpdateState(MainPack pack){
        playerManager.UpdateState(pack);
    }
    public void SpawnBullet(MainPack pack){
        playerManager.spawnBullet(pack);
    }

    public  void SendTo(MainPack pack)
    {
        pack.User=UserName;
        clientManager.SendTo(pack);
    }
}
