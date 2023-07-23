using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UserItem : MonoBehaviour
{
    public Text userName;
    public void SetUserInfo(string username){
        this.userName.text=username;
    } 
}
