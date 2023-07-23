using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Button join;
    public Text title, num, state;
    public event Action JoinClickEvent=null;
    private void Start()
    {
        join.onClick.AddListener(OnJoinClick);
    }
    private void OnJoinClick()
    {
        JoinClickEvent?.Invoke();
    }
    public void AddListenerJoinClick(Action action){
        JoinClickEvent+=action;
    }
    public void RemoveListenerJoinClick(Action action){
        JoinClickEvent-=action;
    }
    public void SetRoomInfo(string title,int curnum,int maxnum,int state){
        this.title.text=title;
        this.num.text=curnum+"/"+maxnum;
        switch(state){
            case 0:
                this.state.text="等待加入";
                break;
            case 1:
                this.state.text="房间已满";
                break;
            case 2:
                this.state.text="游戏中";
                break;
        }
    }
}