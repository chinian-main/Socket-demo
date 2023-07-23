using Demo_Server.DAO;
using Demo_Server.Servers;
using MySql.Data.MySqlClient;
using SocketGameProtocol;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Demo_Server
{
    class Client
    {
        private Socket socket;
        private Socket udpClient;
        private EndPoint remoteEp;
        private Message message;
        private UserData userData;
        private Server server;
        public UDPServer udpServer;
        public Room GetRoom
        {
            set; get;
        }
        public MySqlConnection GetSqlConnection
        {
            get => conn;
        }
        public UserData GetUserData
        {
            get { return userData; }
        }

        private MySqlConnection conn;
        private string connStr = "server=localhost;Port=3306;Database=test;UserID=root;Password=143144";
        public UserInfo GetUserInfo
        {
            get;set;
        }
        public EndPoint IEP
        {
            get
            {
                return remoteEp;
            }
            set
            {
                remoteEp = value;
            }
        }
        public class UserInfo
        {
            public string UserName
            {
                get;set;
            }
            public int HP
            {
                get;set;
            }
            public PosPack Pos
            {
                get;set;
            }
        }

        public Client(Socket socket,Server server,UDPServer us)
        {
            userData = new UserData();
            message = new Message();
            GetUserInfo = new UserInfo();


            this.udpServer = us;
            this.server=server;
            this.socket = socket;
            ConnectMysql();
            StartReceive();
        }
        private void ConnectMysql()
        {
            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();
                Console.WriteLine("连接成功");
            }
            catch (Exception e)
            {
                Console.WriteLine("连接数据库");
            }
        }
        void StartReceive()
        {
            socket.BeginReceive(message.Buffer,message.Startindex,message.Remsize,SocketFlags.None, ReceiveCallback,null);
        }
        void ReceiveCallback(IAsyncResult iar)
        {
            try
            {
                if (socket == null || socket.Connected == false) return;
                 int len = socket.EndReceive(iar);
                if(len==0)
                {
                    Close();
                    return;
                }
                message.ReadBuffer(len,HandleRequest);
                StartReceive();
            }
            catch
            {
                Close();
            }
           
        }
        public void Send(MainPack pack)
        {
            Console.WriteLine("发送:"+pack.ToString());
            if(pack!=null)
            socket.Send(Message.PackData(pack));
        }
        public void SendTo(MainPack pack)
        {
            if (IEP == null) return;
            udpServer.SendTo(pack, IEP);
        }
        void  HandleRequest(MainPack pack)
        {
            server.HandleRequest(pack, this);
        }
        public bool Logon(MainPack pack)
        {
            return this.GetUserData.Logon(pack,conn);
        }
        public bool Login(MainPack pack)
        {
            return this.GetUserData.Login(pack, conn);
        }
        private void Close()
        {
            if (GetRoom != null)
            {
                GetRoom.Exit(server,this);
            }
            server.RemoveClient(this);
            socket.Close();
            conn.Close();

        }
    
     
    }
}
