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
        private int index;
        private Dictionary<int, TcpClient> tcpClients = new Dictionary<int, TcpClient>();

        public TCP_Listener(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            StartServer();
        }

        private void StartServer()
        {
            Console.WriteLine("Try to start Server...");
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
                    tcpClients.Add(++index, client);
                    Console.WriteLine(client.Client.ToString() + " Connected Index - " + index);
                    foreach (var tcp_Clients in tcpClients)
                    {
                        if (!tcp_Clients.Value.Connected)
                        {
                            Console.WriteLine(tcp_Clients.Value.ToString() + " Disconected");
                            tcpClients.Remove(tcp_Clients.Key);
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
                    foreach (var tcp_Clients in tcpClients)
                    {
                        if (!IsConnected(tcp_Clients.Value))
                        {
                            Console.WriteLine(tcp_Clients.Value.ToString() + " disconnected index - " + tcp_Clients.Key);
                            tcpClients.Remove(tcp_Clients.Key);
                        }
                    }
                }
                );
        }

        public bool IsConnected(TcpClient tcpClient)
        {
            try
            {
                NetworkStream stream = tcpClient.GetStream();

                byte[] data = Encoding.UTF8.GetBytes("");
                stream.Write(data, 0, data.Length);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ShowActiveTcpConnections()
        {
            Console.WriteLine("Active TCP Connections");
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            foreach (TcpConnectionInformation c in connections)
            {
                Console.WriteLine("{0} <==> {1}",
                    c.LocalEndPoint.ToString(),
                    c.RemoteEndPoint.ToString());
            }
        }

        public void SendMessage(string massage)
        {

        }
    }
}
