using SocketGameProtocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_Server.Controller
{
    class UserController:BaseController
    {
        public UserController()
        {

            requestCode = RequestCode.Uesr;
        }
        public MainPack Logon(Server server,Client client,MainPack pack)
        {
            if (client.GetUserData.Logon(pack,client.GetSqlConnection))
            {
                pack.ReturnCode = ReturnCode.Succeed;
            }
            else
            {
                pack.ReturnCode = ReturnCode.Filled;
            }
            //server.HandleRequest(pack, client);
            return pack;
        }

        public MainPack Login(Server server, Client client, MainPack pack)
        {
            if (client.Login(pack))
            {
                pack.ReturnCode = ReturnCode.Succeed;
                client.GetUserInfo.UserName = pack.LoginPack.Username;
            }
            else
            {
                pack.ReturnCode = ReturnCode.Filled;
            }
            return pack;

        }
        
    }
}
