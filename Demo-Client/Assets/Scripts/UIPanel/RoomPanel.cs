using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : BasePanel
{
    public Button back, send, start;
    public InputField inputtext;
    public Scrollbar scrollbar;
    public Transform content;
    public Text chatText;
    public GameObject userItemObj;
    public ExitRoomRequest exitRoomRequest;
    public ChatRequest chatRequest;
    public StartGameRequest startGameRequest;
    private void Start()
    {
        back.onClick.AddListener(OnBackClick);
        send.onClick.AddListener(OnSendClick);
        start.onClick.AddListener(OnStartGame);
    }
    private void OnStartGame(){
        startGameRequest.SendRequest ();
    }
    private void OnBackClick()
    {
        exitRoomRequest.SendRequest();
        // uIMag.PopPanel();
    }

    public void ExitRoomResponse()
    {
        uIMag.PopPanel();
    }
    public void ChatResponse(string str)
    {
        Debug.Log("yes");
        chatText.text += str + "\n";
    }
    public void StartGameResponse(MainPack pack)
    {
        switch (pack.ReturnCode)
        {
            case ReturnCode.Succeed:
                uIMag.ShowMessage("游戏开始");
                break;
            case ReturnCode.Filled:
                uIMag.ShowMessage("开始游戏失败,你不是房主");
                break;
        }
    }
    public void GameStarting(MainPack packs ){
        GamePanel gamePanel= uIMag.PushPanel(PanelType.Game).GetComponent<GamePanel>();
        gamePanel.UpdateList(packs);
    }
    private void OnSendClick()
    {
        if (inputtext.text == "")
        {
            uIMag.ShowMessage("发送文本不能为空");
            return;
        }
        chatRequest.SendRequest(inputtext.text);
        chatText.text += "我：" + inputtext.text + "\n";
        inputtext.text = "";
    }

    //刷新玩家列表
    public void UpdatePlayerList(MainPack pack)
    {
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        foreach (PlayerPack item in pack.Playerpack)
        {
            UserItem useritem = Instantiate(userItemObj, Vector3.zero, Quaternion.identity, content).GetComponent<UserItem>();
            useritem.transform.localScale = Vector3.one;
            useritem.SetUserInfo(item.Playername);

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
    }
    private void Exit()
    {
        gameObject.SetActive(false);
    }
}
