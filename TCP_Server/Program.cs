using System;
using TCP_ListenerN.Server;

namespace TCP_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TCP_Listener listener = new TCP_Listener("192.168.5.246",1444);
            Console.ReadKey();
        }
    }
}
