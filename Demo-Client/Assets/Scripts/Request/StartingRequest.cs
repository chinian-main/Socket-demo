using System.Linq;
using SocketGameProtocol;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingRequest : BaseRequest
{
    MainPack isStart = null;
    public RoomPanel roomPanel;
    protected override void Awake()
    {
        actionCode = ActionCode.Starting;
        base.Awake();
    }
    bool flag =false;
    private void Update()
    {
        if (flag &&isStart != null)
        {
            flag=false;
            SceneManager.LoadSceneAsync("Game").completed += (_) =>
            {
                Debug.Log("加载完成");
                face.AddPlayer(isStart);
                roomPanel.GameStarting(isStart);
                isStart = null;
            };

        }
    }
    public override void OnResponse(MainPack pack)
    {
        isStart = pack;
        flag=true;
    }

}