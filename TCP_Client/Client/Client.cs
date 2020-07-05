using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TCP_Client.Client
{
    class Client
    {
        private string serverIP;
        private int serverPort;
        private TcpClient tcpClient;
        private const float TIMER_CONNECTION_CHECK = 3F;
        private bool isFirstTimeConnected;
        private float myTimer;


        public bool IsConnected
        {
            get
            {
                try
                {
                    if (tcpClient != null && tcpClient.Client != null && tcpClient.Client.Connected)
                    {
                        if (tcpClient.Client.Poll(0, SelectMode.SelectRead))
                        {
                            byte[] buff = new byte[1];
                            if (tcpClient.Client.Receive(buff, SocketFlags.Peek) == 0)
                            {
                                // Client disconnected
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        public Client(string serverIP, int serverPort)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
            ConnectToServer();
        }


        private async void ConnectToServer()
        {
            Console.WriteLine("Try to connect to server...");
            tcpClient = new TcpClient();
            while (true)
            {
                if (myTimer <= 0)
                {
                    myTimer = TIMER_CONNECTION_CHECK;
                    try
                    {
                        if (IsConnected)
                            continue;
                        if (isFirstTimeConnected)
                        {
                            tcpClient.Close();
                            isFirstTimeConnected = false;
                            Console.WriteLine("Client DISconnected");
                            tcpClient = new TcpClient();
                        }
                        await Task.Run(() => tcpClient.Connect(serverIP, serverPort));
                        isFirstTimeConnected = tcpClient.Connected;
                        Console.WriteLine("Client connected");
                    }
                    catch (Exception q)
                    {
                        Console.WriteLine("Can't connected to " + serverIP + ":" + serverPort + " server");
                    }
                }
                else
                {
                    myTimer -= 0.01f;
                }

            }
        }


        private void ReadFromServer()
        {

        }
    }
}
