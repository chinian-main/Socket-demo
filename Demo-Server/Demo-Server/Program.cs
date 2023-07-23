using System;

namespace Demo_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(6666);
            Console.ReadKey();
        }
    }
}
