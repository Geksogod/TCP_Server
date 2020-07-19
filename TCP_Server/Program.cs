using System;
using TCP_ListenerN.Server;
using GeksoFileLib.FileListener;

namespace TCP_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IniFileListener iniFile = new IniFileListener(@"D:\Projects\Pet\C#\TCP_Server\serverInfo.ini");
            string ip = iniFile.Read("ip","General");
            string port = iniFile.Read("port", "General");
            int portInt = Int32.Parse(port);
            TCP_Listener listener = new TCP_Listener(ip, portInt);
            Console.ReadKey();
        }
    }
}
