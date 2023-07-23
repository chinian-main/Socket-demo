using UnityEngine;
using UnityEngine.UI;
using SocketGameProtocol;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using System;

public class RoomListPanel : BasePanel
{

    public Button back, find, create;
    public InputField roomname;
    public GameObject roomitem;
    public Slider numSlider;
    public CreateRoomRequest createRoomRequest;
    public FindRoomRequest findRoomRequest;
    public Transform roomListTransform;
    public JoinRoomRequest joinRoomRequest;
    // public GameObject roomitem;
    private void Start()
    {
        back.onClick.AddListener(OnBackClick);
        find.onClick.AddListener(OnFindClick);
        create.onClick.AddListener(OnCreateRoomClick);

    }
    private void OnBackClick()
    {

    }
    public void FindRoomResponse(MainPack pack)
    {
        switch (pack.ReturnCode)
        {
            case ReturnCode.Succeed:
                uIMag.ShowMessage("查询成功!一共有" + pack.Roompack.Count + "个房间");
                break;
            case ReturnCode.Filled:
                uIMag.ShowMessage("查询失败");
                break;
            case ReturnCode.NotRoom:
                uIMag.ShowMessage("没有房间");

                break;
            default:
                // Debug.Log("");
                uIMag.ShowMessage("房间不存在");
                break;
        }
        UpdateRoomList(pack);
    }
    private void ClearAllRoom()
    {
        for (int i = 0; i < roomListTransform.childCount; i++)
        {
            Destroy(roomListTransform.GetChild(i).gameObject);
        }
    }
    private void UpdateRoomList(MainPack pack)
    {
        //情况房间列表
        ClearAllRoom();
        foreach (var room in pack.Roompack)
        {
            RoomItem item = Instantiate(roomitem, Vector3.zero, Quaternion.identity, roomListTransform).GetComponent<RoomItem>();
            item.transform.localScale = Vector3.one;
            // item.gameObject.transform.SetParent(roomListTransform);
            item.SetRoomInfo(room.Roomname, room.Currentnum, room.Maxnum, room.State);
            item.AddListenerJoinClick(() =>
            {
                JoinRoom(room.Roomname);
            });
        }
    }
    public void JoinRoom(string name)
    {
        joinRoomRequest.SendRequest(name);
    }
    private void OnFindClick()
    {
        if (findRoomRequest == null) Debug.Log("he");
        findRoomRequest.SendRequest();
    }
    private void OnCreateRoomClick()
    {
        if (roomname.text == "")
        {
            uIMag.ShowMessage("房间名不能为空");
            return;
        }
        createRoomRequest.SendRequest(roomname.text, (int)numSlider.value);
    }
    public void CreateRoomResponse(MainPack pack)
    {
        switch (pack.ReturnCode)
        {
            case ReturnCode.Succeed:
                uIMag.ShowMessage("创建成功");
                RoomPanel panel = uIMag.PushPanel(PanelType.Room).GetComponent<RoomPanel>();
                panel.UpdatePlayerList(pack);
                break;
            case ReturnCode.Filled:
                uIMag.ShowMessage("创建失败");
                break;
            default:
                Debug.Log("def");
                break;
        }
    }
    public void JoinRoomResponse(MainPack pack)
    {
        switch (pack.ReturnCode)
        {
            case ReturnCode.Succeed:
                uIMag.ShowMessage("加入房间成功");
                RoomPanel panel = uIMag.PushPanel(PanelType.Room).GetComponent<RoomPanel>();
                panel.UpdatePlayerList(pack);
                break;
            case ReturnCode.Filled:
                uIMag.ShowMessage("加入房间失败");
                break;
            default:
                Debug.Log("def");
                break;
        }
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
        findRoomRequest.SendRequest();
    }
    private void Exit()
    {
        gameObject.SetActive(false);
    }
}