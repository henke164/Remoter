using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Remoter.Server.Services
{
    public class TcpServer
    {
        private TcpClient _client;
        private NetworkStream _clientStream;

        public void Start()
        {
            var ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000);
            var listener = new TcpListener(ip);
            listener.Start();

            _client = listener.AcceptTcpClient();
            _clientStream = _client.GetStream();
        }

        public void SendImage(byte[] imageData, int width, int height)
        {
            if (_client == null || !_client.Connected)
            {
                return;
            }

            try
            {
                if (!WaitForHandshake(1))
                {
                    return;
                }
                var metaData = $"{imageData.Length}|";
                var metaDataBytes = Encoding.UTF8.GetBytes(metaData);
                _clientStream.Write(metaDataBytes, 0, metaData.Length);

                if (!WaitForHandshake(2))
                {
                    return;
                }

                _clientStream.Write(imageData, 0, imageData.Length);
            }
            catch
            {
                Console.WriteLine("Data loss");
            }
        }

        private bool WaitForHandshake(int handshakeIndex)
        {
            var buffer = new byte[1];
            _clientStream.Read(buffer, 0, buffer.Length);

            return buffer[0] == handshakeIndex;
        }
    }
}
