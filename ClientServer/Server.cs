using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic; 
using System.Text; 

namespace ClientServer
{
    public class Server
    {
        public int BufferSize { get; }
        public int Port { get; }
        public string HostEntry { get; }
        public Queue<string> CommandQueue { get; set; }
        public Socket Socket { get; private set; }
        IPHostEntry Host { get; }
        IPAddress Address { get; }
        IPEndPoint EndPoint { get; }
        int Listeners { get; set; }

        public Server(int bufferSize, int port, string hostEntry)
        {
            BufferSize = bufferSize;
            Port = port;
            HostEntry = hostEntry;
            
            // initial server setup
            Host = Dns.GetHostEntry(HostEntry);
            Address = Host.AddressList[0];
            EndPoint = new IPEndPoint(Address, Port);

            Listeners = 5;
            CommandQueue = new Queue<string>();
            Socket = new Socket(Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp); 
            StartServer(); 
        }    
        public void StartServer()
        {
            try
            {
                Socket.Bind(EndPoint);
                Socket.Listen(Listeners);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public void BeginListening()
        {

        }
    }
}
