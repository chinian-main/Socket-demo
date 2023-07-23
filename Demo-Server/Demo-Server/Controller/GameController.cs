using SocketGameProtocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_Server.Controller
{
    class GameController:BaseController
    {
        public GameController()
        {
            requestCode = RequestCode.Game;

        }
        public MainPack ExitGame(Server server, Client client, MainPack pack)
        {
            client.GetRoom.ExitGame(client);
            return null;//不需要反馈
        }
        public MainPack UpState( Client client, MainPack pack)
        {
            client.GetRoom.BroadcastTo(client, pack);
            return null;
        }
        public MainPack Fire(Server server, Client client,MainPack pack)
        {
            client.GetRoom.BroadcastTo(client, pack);
            return null;
        }
    }
}
