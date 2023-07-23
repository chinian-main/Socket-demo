
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SocketGameProtocol;
using UnityEngine;


public class ClientManager : BaseManager
{
    public ClientManager(GameFace face) : base(face) { }
    private Socket socket;
    private Message message;
    private Thread aucThread;
    private const string ip = "127.0.0.1";//服务端ip

    public override void OnInit()
    {
        base.OnInit();

        message = new Message();
        InitSocket();
        InitUDP();
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        message = null;
        CloseSocket();
    }
    private void InitSocket()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.Connect(ip, 6666);
            StartReceive();
            face.ShowMessage("连接成功");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            face.ShowMessage("连接失败");
        }
    }
    private void CloseSocket()
    {
        if (socket.Connected && socket != null)
        {
            socket.Close();
        }
    }
    private void StartReceive()
    {

        socket.BeginReceive(message.Buffer, message.Startindex, message.Remsize, SocketFlags.None, ReceiveCallback, null);
    }
    private void ReceiveCallback(IAsyncResult iar)
    {
        try
        {
            if (socket == null || socket.Connected == false) return;
            int len = socket.EndReceive(iar);
            if (len == 0)
            {
                CloseSocket();
                return;
            }
            Debug.Log("接受成功");

            message.ReadBuffer(len, HandleResponse);

            StartReceive();
        }
        catch
        {

        }
    }
    private void HandleResponse(MainPack pack)
    {
        face.HandleResponse(pack);
    }
    public void Send(MainPack pack)
    {
        if (socket.Connected == false || socket == null) return;
        socket.Send(Message.PackData(pack));
        Debug.Log("发送成功:" + pack.ToString());
    }



    //UDP协议
    private Socket udpClient;
    private IPEndPoint iPEndPoint;
    private EndPoint ePoint;
    private Byte[] buffer = new Byte[1024];
    private void InitUDP()
    {
        udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), 6667);
        ePoint = iPEndPoint;
        try
        {
            udpClient.Connect(ePoint);//udp连接只是试水，测试是否能不能发送消息
        }
        catch
        {
            Debug.Log("UDP连接失败");
            return;
        }
        Loom.RunAsync(() =>
        {
            aucThread = new Thread(ReceiveMsg);
            aucThread.Start();
        });

    }

    private void ReceiveMsg()
    {
        Debug.Log("UDP开始接受");
        while (true)
        {
            int len = udpClient.ReceiveFrom(buffer, ref ePoint);
            MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, 0, len);
            Debug.Log("接受数据" + pack.ActionCode.ToString() + pack);
            Loom.QueueOnMainThread((_)=>{
                HandleResponse(pack);
            },null);
            // HandleResponse(pack);
        }
    }
    public void SendTo(MainPack pack)
    {
        Byte[] sendBuff = Message.PackDataUDP(pack);
        udpClient.Send(sendBuff, sendBuff.Length, SocketFlags.None);
    }
}