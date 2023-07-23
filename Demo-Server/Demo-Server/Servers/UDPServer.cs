using Demo_Server.Controller;
using SocketGameProtocol;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Demo_Server.Servers
{
    class UDPServer
    {
        Socket udpServer;
        IPEndPoint bindEP;//本地监听IP
        EndPoint remoteEP;//远程IP

        Server server;
        ControllerManager controllerManager;
        Byte[] buffer = new Byte[1024];
        Thread receiveThread;

        public UDPServer(int port,Server server,ControllerManager controllerMgr)
        {
            this.server = server;
            this.controllerManager = controllerMgr;
            udpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            bindEP = new IPEndPoint(IPAddress.Any, port);
            remoteEP = (EndPoint)bindEP;
            udpServer.Bind(bindEP);
            receiveThread = new Thread(ReceiveMsg);
            receiveThread.Start();
       
         
            Console.WriteLine("【UDP服务已经启动】");


        }
        ~UDPServer()
        {
            if (receiveThread != null)
            {
                receiveThread.Abort();
                receiveThread = null;
            }
        }
        public void ReceiveMsg()
        {
            while (true)
            {
                try
                {
                    int len = udpServer.ReceiveFrom(buffer, ref remoteEP);
                    MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, 0, len);
                    HandleRequest(pack, remoteEP);
                }
                catch
                {

                }
            }
        }
        public void HandleRequest(MainPack  pack ,EndPoint ipEndPoint)
        {
            Client client = server.ClientFromUserName(pack.User);
            if (client.IEP == null)
            {
                client.IEP = ipEndPoint;
            }
            controllerManager.HandleRequest(pack, client,true);
        }
        public void SendTo(MainPack pack,EndPoint point)
        {
            byte[] buff = Message.PackDataUDP(pack);
            udpServer.SendTo(buff, buff.Length, SocketFlags.None, point);
        }
    }
}
