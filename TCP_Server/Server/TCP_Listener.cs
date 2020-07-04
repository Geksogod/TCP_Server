using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Linq;

namespace TCP_ListenerN.Server
{
    class TCP_Listener
    {
        private string ip;
        private int port;
        public bool serverIsStarted;
        private List<TcpClient> tcpClients = new List<TcpClient>();

        public TCP_Listener(string ip, int port)
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
                serverIsStarted = true;
            }
            catch (Exception q)
            {
                Console.WriteLine("Error with start server : " + q.Message);
                serverIsStarted = false;
                throw;
            }
            if (serverIsStarted)
            {
                ListenConnectedAsync(server);
                ListenDisconnectedAsync(server);
            }
        }


        private void CloseServer(TcpListener server)
        {
            server.Stop();
            serverIsStarted = false;
            Console.WriteLine("Server " + server.LocalEndpoint.ToString() + " closed");
        }

        private async void ListenConnectedAsync(TcpListener _listener)
        {
            while (true)
                await Task.Run(() =>
                {
                    var client = _listener.AcceptTcpClient();
                    tcpClients.Add(client);
                    Console.WriteLine(client.Client.ToString() + " Connected Index - " + tcpClients?.Count);
                    for (int i = 0; i < tcpClients.Count; i++)
                    {
                        if (!tcpClients[i].Connected)
                        {
                            Console.WriteLine(tcpClients[i].Client.ToString() + " Disconected");
                            tcpClients.RemoveAt(i);
                        }
                    }
                }
                );
        }

        private async void ListenDisconnectedAsync(TcpListener _listener)
        {
            while (true)
                await Task.Run(() =>
                {
                    for (int i = 0; i < tcpClients.Count; i++)
                    {
                        if (GetState(tcpClients[i]) == TcpState.Unknown || GetState(tcpClients[i]) == TcpState.Closed)
                        {
                            Console.WriteLine(tcpClients[i].ToString() + " disconnected");
                            tcpClients.RemoveAt(i);
                        }
                    }
                }
                );
        }

        public TcpState GetState(TcpClient tcpClient)
        {
            TcpState foo = TcpState.Unknown;
            IPGlobalProperties.GetIPGlobalProperties()
              ?.GetActiveTcpConnections()
              ?.Where(x => x.LocalEndPoint.Equals(tcpClient.Client.LocalEndPoint)).ToList().ForEach(delegate (TcpConnectionInformation i) { foo = i.State; });
            return foo;
        }

        public void SendMessage(string massage)
        {

        }
    }
}
