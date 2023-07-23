using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelType
{
    Message,
    Start,
    Login,
    Logon,
    RoomList,
    Room,
    Game,
    GameOver,


}
public class UIManager : BaseManager
{
    public UIManager(GameFace face) : base(face) { }
    private Dictionary<PanelType, BasePanel> panelDict = new Dictionary<PanelType, BasePanel>();
    private Dictionary<PanelType, string> panelPath = new Dictionary<PanelType, string>();
    private Stack<BasePanel> panelStack = new Stack<BasePanel>();
    private Transform canvasTransform;
    private MessagePanel messagePanel;
    public override void OnInit()
    {
        base.OnInit();
        InitPanel();
        canvasTransform = GameObject.Find("Canvas").transform;
        PushPanel(PanelType.Message);
        PushPanel(PanelType.Start);

    }

    //UI显示在面板上
    public BasePanel PushPanel(PanelType panelType)
    {
        if (panelDict.TryGetValue(panelType, out BasePanel panel))
        {
            if (panelStack.Count > 0)
            {
                BasePanel topPanel = panelStack.Peek();
                topPanel.OnPasue();
            }
            panelStack.Push(panel);
            panel.OnEnter();
            return panel;
        }
        else
        {
            BasePanel panel1 = SpawnPanel(panelType);
            if (panelStack.Count > 0)
            {
                BasePanel topPanel = panelStack.Peek();
                topPanel.OnPasue();
            }
            panelStack.Push(panel1);
            panel1.OnEnter();
            return panel1;
        }

    }
    //关闭当前UI
    public void PopPanel()
    {
        if (panelStack.Count == 0) return;
        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();
        BasePanel panel = panelStack.Peek();
        panel.OnRecovery();
    }

    //实例化UI
    public BasePanel SpawnPanel(PanelType panelType)
    {
        if (panelPath.TryGetValue(panelType, out string path))
        {
            GameObject prefab = Resources.Load(path) as GameObject;
            GameObject obj = GameObject.Instantiate(prefab, canvasTransform);
            BasePanel panel = obj.GetComponent<BasePanel>();
            
            panel.SetUIMag = this;
            panelDict.Add(panelType, panel);
            return panel;
        }
        else
        {
            return null;
        }
    }
    //初始化
    public void InitPanel()
    {
        string panelpath = "Panel/";
        string[] path = new string[]{
            "MessagePanel",
            "StartPanel",
            "LoginPanel",
            "LogonPanel",
            "RoomListPanel",
            "RoomPanel",
            "GamePanel"
        };
        for (int i = 0; i < path.Length; i++)
        {
            panelPath.Add((PanelType)i, panelpath + path[i]);
        }
    }
    public void SetMessagePanel(MessagePanel messagePanel)
    {
        this.messagePanel = messagePanel;
    }
    public void ShowMessage(string str, bool sync = false)
    {
        messagePanel.ShowMessage(str, sync);
    }
}
