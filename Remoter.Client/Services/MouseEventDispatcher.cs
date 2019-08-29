using Remoter.Shared.Models;
using System.Windows.Forms;

namespace Remoter.Client.Services
{
    public class MouseEventDispatcher
    {
        private CommandClient _commandClient;
        private bool _mouseDown = false;

        public MouseEventDispatcher(CommandClient commandClient)
        {
            _commandClient = commandClient;
        }

        public void TriggerMouseDown(MouseEventArgs e)
        {
            _mouseDown = true;
            var message = $"{e.Button.ToString()}|{e.X}|{e.Y}";
            _commandClient.SendCommand(CommandMessageType.MouseDown, message);
        }

        public void TriggerMouseUp(MouseEventArgs e)
        {
            _mouseDown = false;
            var message = $"{e.Button.ToString()}|{e.X}|{e.Y}";
            _commandClient.SendCommand(CommandMessageType.MouseUp, message);
        }

        public void TriggerMouseMove(MouseEventArgs e)
        {
            if (_mouseDown)
            {
                var message = $"{e.Button.ToString()}|{e.X}|{e.Y}";
                _commandClient.SendCommand(CommandMessageType.MouseDrag, message);
            }
        }

        public void TriggerMouseLeave()
        {
            _mouseDown = false;
            _commandClient.SendCommand(CommandMessageType.MouseUp, "All|0|0");
        }
    }
}
