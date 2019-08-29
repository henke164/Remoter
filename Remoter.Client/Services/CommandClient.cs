using Remoter.Shared.Models;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Remoter.Client.Services
{
    public class CommandClient : IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public void Connect()
        {
            _client = new TcpClient();
            _client.Connect(IPAddress.Parse("127.0.0.1"), 3001);
            _stream = _client.GetStream();
        }

        public void Dispose()
        {
            _stream.Dispose();
            _client.Dispose();
        }

        public void SendCommand(CommandMessageType type, string message)
        {
            var str = $"{(int)type}|{message}|";
            var package = Encoding.UTF8.GetBytes(str);
            _stream.Write(package, 0, package.Length);
        }
    }
}
