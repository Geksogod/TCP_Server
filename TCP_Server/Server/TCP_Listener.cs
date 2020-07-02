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
            }
            catch (Exception q)
            {
                Console.WriteLine("Error with start server : " + q.Message);
                throw;
            }
            CloseServer(server);
        }

        private void CloseServer(TcpListener server)
        {
            server.Stop();
            Console.WriteLine("Server " + server.LocalEndpoint.ToString()+" closed");
        }
    }
}
