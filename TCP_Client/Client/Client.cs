using System;
using System.Net.Sockets;

namespace TCP_Client.Client
{
    class Client 
    {
        private string serverIP;
        private int serverPort;
        private TcpClient tcpClient;

        public Client(string serverIP,int serverPort)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
            ConnectToServer();
        }


        private void ConnectToServer()
        {
            Console.WriteLine("Try to connect to server...");
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(serverIP, serverPort);
                Console.WriteLine("Client connected");
            }
            catch (Exception)
            {
                Console.WriteLine("Can't connected to "+serverIP + ":"+serverPort+" server");
                throw;
            }
        }


        private void ReadFromServer()
        {

        }
    }
}
