using Remoter.Server.Services.Abstractions;
using System;
using System.Text;

namespace Remoter.Server.Services
{
    public class ImageSender : TcpServer
    {
        public void SendImage(byte[] imageData, int width, int height)
        {
            if (Client == null || !Client.Connected)
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
                Stream.Write(metaDataBytes, 0, metaData.Length);

                if (!WaitForHandshake(2))
                {
                    return;
                }

                Stream.Write(imageData, 0, imageData.Length);
            }
            catch
            {
                Console.WriteLine("Data loss");
                Stream.Close();
            }
        }

        private bool WaitForHandshake(int handshakeIndex)
        {
            var buffer = new byte[1];
            Stream.Read(buffer, 0, buffer.Length);

            return buffer[0] == handshakeIndex;
        }
    }
}
