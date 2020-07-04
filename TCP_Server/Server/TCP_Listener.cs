using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TCP_ListenerN.Server
{
    class TCP_Listener
    {
        private string ip;
        private int port;
        public bool serverIsStared;

        public TCP_Listener(string ip,int port)
        {
            this.ip = ip;
            this.port = port;
            StartServer();
        }

        private void StartServer()
        {
            Console.WriteLine("Try to start Server...");
            //IPAddress.Any;
            IPAddress ipAddress = IPAddress.Parse(ip);
            TcpListener server = new TcpListener(ipAddress, port);
            ServerInitialize(server);
        }

        private void ServerInitialize(TcpListener server)
        {
            try
            {
                server.Start();
                Console.WriteLine("Server started...");
                serverIsStared = true;
            }
            catch (Exception q)
            {
                Console.WriteLine("Error with start server : " + q.Message);
                serverIsStared = false;
                throw;
            }
            if (serverIsStared)
                Listen(server);
        }


        private void CloseServer(TcpListener server)
        {
            server.Stop();
            serverIsStared = false;
            Console.WriteLine("Server " + server.LocalEndpoint.ToString()+" closed");
        }

        private async void Listen(TcpListener _listener)
        {
            while (true)
            {
                var client = await _listener.AcceptTcpClientAsync().ConfigureAwait(false);
                Console.WriteLine(client.Client.ToString() + " Connected");
            }
        }

        public void SendMessage(string massage)
        {

        }
    }
}
