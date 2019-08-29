using Remoter.Server.Models;
using Remoter.Server.Services.Abstractions;
using Remoter.Shared.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Remoter.Server.Services
{
    public class CommandListener : TcpServer
    {
        public Action<MouseEventDetails> OnMouseDown { get; set; }
        public Action<MouseEventDetails> OnMouseDrag { get; set; }
        public Action<MouseEventDetails> OnMouseUp { get; set; }

        public void ListenForCommands()
        {
            try
            {
                var bytes = new byte[100];

                Stream.Read(bytes, 0, bytes.Length);

                var package = Encoding.UTF8.GetString(bytes);

                var data = package.Split('|');

                var type = (CommandMessageType)int.Parse(data[0]);

                Console.WriteLine(type);

                switch (type)
                {
                    case CommandMessageType.MouseDown:
                        OnMouseDown?.Invoke(new MouseEventDetails
                        {
                            MouseButton = data[1],
                            X = int.Parse(data[2]),
                            Y = int.Parse(data[3])
                        });
                        break;
                    case CommandMessageType.MouseUp:
                        OnMouseUp?.Invoke(new MouseEventDetails
                        {
                            MouseButton = data[1]
                        });
                        break;
                    case CommandMessageType.MouseDrag:
                        OnMouseUp?.Invoke(new MouseEventDetails
                        {
                            MouseButton = data[1],
                            X = int.Parse(data[2]),
                            Y = int.Parse(data[3])
                        });
                        break;
                }
            }
            catch
            {
            }

            Task.Run(() => ListenForCommands());
        }
    }
}
