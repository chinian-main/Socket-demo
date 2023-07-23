using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Protobuf;
using SocketGameProtocol;

namespace Demo_Server
{
    class Message
    {
        private byte[] buffer = new byte[1024];
        private int startindex;

        public byte[] Buffer { get => buffer;  }
        public int Startindex { get => startindex;  }
        public int Remsize
        {//剩余空间
            get
            {
                return buffer.Length - startindex;
            }
        }
        public void ReadBuffer(int len,Action<MainPack> HandleRequest)
        {
            startindex += len;
          
            while (true)
            {  
                if (startindex < 4) return;
                int count = BitConverter.ToInt32(buffer,0);//取出四个字节
                if (startindex >= (count + 4))
                {
                    MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, 4, count);
                    HandleRequest(pack);
                    Array.Copy(buffer, count + 4, buffer, 0, startindex - count - 4);
                    startindex -= (count + 4);
                } 
                else
                {
                    break;
                }
            }

        }
        public static byte[] PackData(MainPack pack)
        {
            byte[] data = pack.ToByteArray();//包体
            byte[] head = BitConverter.GetBytes(data.Length);//包头
            return head.Concat(data).ToArray();
        }

        public  static Byte[] PackDataUDP(MainPack pack)
        {
            //UDP打包不需要包头
            return pack.ToByteArray();
        }
    }
}
