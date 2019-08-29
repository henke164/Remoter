using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Remoter.Server.Services.Abstractions
{
    public abstract class TcpServer : IDisposable
    {
        protected TcpClient Client;
        protected NetworkStream Stream;

        public void Dispose()
        {
            Client.Dispose();
            Stream.Dispose();
        }

        public void Start(int port)
        {
            Task.Run(() => { 
                var ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                var listener = new TcpListener(ip);
                listener.Start();

                Client = listener.AcceptTcpClient();
                Stream = Client.GetStream();
            });
        }
    }
}
