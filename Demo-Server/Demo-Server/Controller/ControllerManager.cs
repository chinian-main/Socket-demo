using SocketGameProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Demo_Server.Controller
{
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controlDict = new Dictionary<RequestCode, BaseController>();
        private Server server;
        public ControllerManager(Server server)
        {
            this.server = server;
            UserController userController = new UserController();
            RoomController roomController = new RoomController();
            GameController gameController = new GameController();
            controlDict.Add(roomController.GetRequestCode, roomController);
            controlDict.Add(userController.GetRequestCode,userController);
            controlDict.Add(gameController.GetRequestCode, gameController);
        }
        public void HandleRequest(MainPack pack, Client client, bool isUDP = false)
        {
            if(controlDict.TryGetValue(pack.RequestCode,out BaseController controller))
            {
                string metname = pack.ActionCode.ToString();
              
                MethodInfo method = controller.GetType().GetMethod(metname);
                if (method == null)
                {
                    Console.WriteLine("没有对应的方法");
                    return;
                }
                object[] objs;
                if (isUDP)//UDP
                {
                    objs = new object[] { client, pack };
                    method.Invoke(controller, objs);
                }else
                {  
                    Console.WriteLine(metname);
                    objs = new object[] { this.server, client, pack };
                    object ret = method.Invoke(controller, objs);
                    if (ret != null)
                    {
                        client.Send(ret as MainPack);
                        Console.WriteLine("发送数据");
                    }
                }
                
               
            }
            else
            {
                Console.WriteLine("没有对应的Controller处理");
            }
        }
    }
}
