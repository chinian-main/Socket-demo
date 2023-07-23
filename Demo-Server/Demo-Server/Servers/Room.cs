using Google.Protobuf.Collections;
using SocketGameProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Demo_Server
{
    class Room
    {
        private RoomPack roomInfo;
        private Server server;
        private List<Client> clientList = new List<Client>();

        public Room(Client client,RoomPack pack,Server server)
        {
            this.roomInfo = pack;
            clientList.Add(client);
            roomInfo.Currentnum = 1;
            client.GetRoom = this;
            this.server = server;
        }
        /// <summary>
        /// 返回房间信息
        /// </summary>
        public RoomPack GetRoomInfo { get {
                roomInfo.Currentnum = clientList.Count;
                return roomInfo;
            }
        }
        public RepeatedField<PlayerPack> GetPlayerInfo()
        {
            RepeatedField<PlayerPack> packs = new RepeatedField<PlayerPack>();
            foreach(Client client in clientList)
            {
                PlayerPack player = new PlayerPack();
                player.Playername = client.GetUserInfo.UserName;
                packs.Add(player);
            }
            return packs;
        }
        /// <summary>
        /// 广播给除了自己（client）以外的其他人(client)
        /// </summary>
        /// <param name="client"></param>
        /// <param name="pack"></param>
        public void Broadcast(Client client,MainPack pack)
        { 
            foreach(Client c in clientList)
            {
                if (c.Equals(client))
                {
                    continue;
                }
                c.Send(pack);
            }
        }
        public void BroadcastTo(Client client,MainPack pack)
        {
            //广播数据
            foreach(Client c in clientList)
            {
                if (c.Equals(client))
                {
                    continue;
                }
                c.SendTo(pack);
            }
        }
        public void Join(Client client)
        {
            clientList.Add(client);
            if (roomInfo.Maxnum == clientList.Count)
            {
                //房间人满了
                roomInfo.State = 1;
            }
          
            client.GetRoom = this;
            MainPack pack = new MainPack();
            pack.ActionCode = ActionCode.PlayerList;
            this.roomInfo.Currentnum++;
            foreach (PlayerPack player in GetPlayerInfo())
            {
                pack.Playerpack.Add(player);
            }
            Broadcast(client, pack);
        }
        public void Exit(Server server, Client client)
        {   
            MainPack pack = new MainPack();
            if (roomInfo.State == 2)//游戏已经开始了
            {
                ExitGame(client);
            }
           else //游戏未开始
            {
                if (client == clientList[0])
                {//房主离开房间
                    client.GetRoom = null;
                    pack.ActionCode = ActionCode.Exit;
                    Broadcast(client, pack);
                    server.RemoveRoom(this);
                    return;
                }
                else
                {
                    //其他成员退出
                    clientList.Remove(client);
                    pack.ActionCode = ActionCode.UpdatePlayer;
                    foreach(var item in clientList)
                    {
                        PlayerPack playerPack = new PlayerPack();
                        playerPack.Playername = item.GetUserInfo.UserName;
                        playerPack.Hp = item.GetUserInfo.HP;
                        pack.Playerpack.Add(playerPack);
                    }
                    pack.Str = client.GetUserInfo.UserName;
                    Broadcast(client, pack);
                }
               
            }
           
        }
        public ReturnCode StartGame(Client client)
        {
            if (client != clientList[0])
            {
                return ReturnCode.Filled;
            }

            Thread startTime = new Thread(CountDown);
            startTime.Start();
            return ReturnCode.Succeed;
        }
        private void CountDown()
        {
            MainPack pack = new MainPack();
            pack.ActionCode = ActionCode.Chat;
            pack.Str = "房主已经启动游戏";

            Broadcast(null, pack);
            Thread.Sleep(1000);
            for (int i = 5; i >=1; i--)
            {
                pack.Str = i.ToString();
                Broadcast(null, pack);
                Thread.Sleep(1000);
            }
            pack.ActionCode = ActionCode.Starting;
            
            foreach(var item in clientList)
            {
                PlayerPack player = new PlayerPack();
                item.GetUserInfo.HP = 100;
                player.Playername = item.GetUserInfo.UserName;
                player.Hp = item.GetUserInfo.HP;
                pack.Playerpack.Add(player);
            }

            Broadcast(null, pack);

        }
       /* IEnumerator Countdown()
        {

        }*/
       public void ExitGame(Client client)
        {
            MainPack pack = new MainPack();
            if (client == clientList[0])
            {
                //房主退出
                client.GetRoom = null;
                pack.ActionCode = ActionCode.ExitGame;
                pack.Str = "r";
                Broadcast(client, pack);
                server.RemoveRoom(this);
            }
            else
            {
                //其他玩家
                clientList.Remove(client);
                client.GetRoom = null;
                pack.ActionCode = ActionCode.UpdatePlayer;
                foreach (var item in clientList)
                {
                    PlayerPack playerPack = new PlayerPack();
                    playerPack.Playername = item.GetUserInfo.UserName;
                    playerPack.Hp = item.GetUserInfo.HP;
                    pack.Playerpack.Add(playerPack);
                }
                pack.Str = client.GetUserInfo.UserName;
                Broadcast(client, pack);
            }
        }
    }
}
