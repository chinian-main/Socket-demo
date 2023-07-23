using System;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    public UIManager uIMag;
    public GameFace face{
        get{
            return GameFace.Face;
        }
    }
    public UIManager SetUIMag{
        set{
            uIMag=value;
        }
    }
    public virtual void OnEnter(){

    }
    public virtual void OnPasue(){

    }
    public virtual void OnRecovery(){

    }
    public virtual void OnExit(){

    }
    
}