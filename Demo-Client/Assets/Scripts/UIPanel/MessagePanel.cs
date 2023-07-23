using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : BasePanel
{
    public Text text;
    string msg = null;
    public override void OnEnter()
    {
        base.OnEnter();
        text.CrossFadeAlpha(0, 0.1f, false);
        uIMag.SetMessagePanel(this);
        
    }
    private void Update()
    {
        if (msg != null)
        {   //
            Debug.Log("updata");
            ShowText(msg);
            msg=null;
        }
    }
    public void ShowMessage(string str, bool isSync = false)
    {
        // if (isSync)
        // {
        //     //异步显示
        //     msg=str;
        // }
        // else {
            ShowText(str);
        // }
    }
    private void ShowText(string str)
    {
        Debug.Log("显示");
        text.text = str;
        text.CrossFadeAlpha(1.0f, 0.5f, false);
        Invoke("HideText", 1f);
    }
    private void HideText()
    {
        Debug.Log("隐藏");
        text.CrossFadeAlpha(0, 1f, false);

    }
}