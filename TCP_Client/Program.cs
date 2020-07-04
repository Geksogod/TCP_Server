using System;

namespace TCP_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Client.Client tcpClient = new Client.Client("192.168.5.246",1444);

            Console.ReadKey();
        }
    }
}
