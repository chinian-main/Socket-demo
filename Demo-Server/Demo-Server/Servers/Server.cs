using Demo_Server.Controller;
using Demo_Server.DAO;
using Demo_Server.Servers;
using SocketGameProtocol;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Demo_Server
{
    class Server
    {
        private Socket socket;
        private UDPServer udpServer;
        private Thread udpThread;
        private List<Client> clientList = new List<Client>();
        private List<Room> roomList = new List<Room>();
        private ControllerManager controllerManager;

        public Server(int port)
        {
            controllerManager = new ControllerManager(this);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            socket.Listen(0);
            Console.WriteLine("【TCP服务已经启动】");
            StartAccept();

            udpServer = new UDPServer(6667, this, controllerManager);

        }
        void StartAccept()
        {
            socket.BeginAccept(AcceptCallback,null);
        }
        void AcceptCallback(IAsyncResult iar)
        {
            Socket client = socket.EndAccept(iar);
            clientList.Add(new Client(client,this,udpServer));
            socket.BeginAccept(AcceptCallback, null);
        }
        
        public void HandleRequest(MainPack pack ,Client client)
        {
            controllerManager.HandleRequest(pack,client);
        }
        public void RemoveClient(Client client)
        {
            clientList.Remove(client);
        }
        public MainPack CreateRoom(Client client, MainPack pack)
        {
            try
            {
                Room room = new Room(client, pack.Roompack[0],this);
                roomList.Add(room);
                foreach(PlayerPack p in room.GetPlayerInfo())
                {
                    pack.Playerpack.Add(p);
                }
                pack.ReturnCode = ReturnCode.Succeed;
                return pack;
            }
            catch
            {
                pack.ReturnCode = ReturnCode.Filled;
                return pack;
            }
            
        }
        public MainPack FindRoom()
        { MainPack pack = new MainPack();
               pack.ActionCode = ActionCode.FindRoom;
            try
            {
                if (roomList.Count == 0)
                {
                    pack.ReturnCode = ReturnCode.NotRoom;
                    return pack;
                }
                
                foreach (Room item in roomList)
                {
                 
                    pack.Roompack.Add(item.GetRoomInfo);

                }
                pack.ReturnCode = ReturnCode.Succeed;
            }
            catch
            {
                pack.ReturnCode = ReturnCode.Filled;
            }
            return pack;
        }
        public MainPack JoinRoom(Client client,MainPack pack)
        {
            foreach(Room r in roomList)
            {
                if (r.GetRoomInfo.Roomname.Equals(pack.Str))
                {
                    //有房间
                    if (r.GetRoomInfo.State == 0)
                    {
                        //可以加入房间
                        r.Join(client);

                        pack.Roompack.Add(r.GetRoomInfo);
                        foreach(PlayerPack p in r.GetPlayerInfo())
                        {
                            pack.Playerpack.Add(p);
                        }
                        pack.ReturnCode = ReturnCode.Succeed;
                        return pack;
                    }
                    else
                    {
                        //不可加入
                        pack.ReturnCode = ReturnCode.Filled;
                        return pack;
                    }
                }
            }
            pack.ReturnCode = ReturnCode.NotRoom;
            return pack;
        }
        public MainPack ExitRoom(Client client,MainPack pack)
        {
            if(client.GetRoom == null)
            {
                pack.ReturnCode = ReturnCode.Filled;
                return pack;
            }
            client.GetRoom.Exit(this,client);
            pack.ReturnCode = ReturnCode.Succeed;
            return pack;
        }
        public void RemoveRoom(Room room)
        {
            roomList.Remove(room);
        }
        public void Chat(Client client,MainPack pack)
        {
            pack.Str = client.GetUserInfo.UserName + ":" + pack.Str;
            client.GetRoom.Broadcast(client, pack);
        }
        public Client ClientFromUserName(string username)
        {
            foreach (Client c in clientList)
            {
                if (c.GetUserInfo.UserName.Equals( username))
                {
                    return c;
                }
            }
            return null;
        }
        public bool SetIEP(EndPoint ipEnd, string user)
        {
            foreach (Client c in clientList)
            {
                if (c.GetUserInfo.UserName == user)
                {
                    c.IEP = ipEnd;
                    return true;
                }
            }
            return false;
        }
    }
}
