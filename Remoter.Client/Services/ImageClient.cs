using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Remoter.Client.Services
{
    public class ImageClient
    {
        private NetworkStream _stream;

        public void Connect()
        {
            var client = new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"), 3000);
            _stream = client.GetStream();
        }

        public void ListenForScreenshots(Action<byte[]> onImageDataReceived)
        {
            try
            {
                MakeHandshake(1);

                var bytes = new byte[100];

                _stream.Read(bytes, 0, bytes.Length);

                var package = Encoding.UTF8.GetString(bytes);

                var data = package.Split('|');

                var imageDataSize = int.Parse(data[0]);
                var imageBytes = new byte[imageDataSize];

                MakeHandshake(2);

                _stream.Read(imageBytes, 0, imageBytes.Length);

                onImageDataReceived(imageBytes);
            }
            catch
            {
            }

            Task.Run(() => ListenForScreenshots(onImageDataReceived));
        }
        
        private void MakeHandshake(int handshakeIndex)
        {
            _stream.Write(new byte[1] { (byte)handshakeIndex }, 0, 1);
        }
    }
}
