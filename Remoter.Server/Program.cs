using Remoter.Server.Models;
using Remoter.Server.Services;
using Remoter.Shared.Models;
using System;
using System.Configuration;

namespace Remoter.Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            var scaleFactor = GetScaleFactor();
            var fps = GetFPS();
            var recorder = new Recorder(scaleFactor, fps);
            recorder.Start(3000);

            var commandListener = new CommandListener();
            commandListener.Start(3001);
            commandListener.OnMouseDown = OnMouseDown;
            commandListener.OnMouseUp = OnMouseUp;
            commandListener.ListenForCommands();

            Console.WriteLine("Server started! Listening for connections...");
            Console.ReadLine();
        }

        private static void OnMouseDown(MouseEventDetails details)
        {
            Console.WriteLine("Mouse " + details.MouseButton + " down");
        }

        private static void OnMouseUp(MouseEventDetails details)
        {
            Console.WriteLine("Mouse " + details.MouseButton + " up");
        }

        private static float GetScaleFactor()
        {
            var scaleSettings = ConfigurationManager.AppSettings["ScaleFactor"];
            var scaleFactor = 1f;
            float.TryParse(scaleSettings.Replace('.', ','), out scaleFactor);
            return scaleFactor;
        }

        private static int GetFPS()
        {
            var fpsSettings = ConfigurationManager.AppSettings["FPS"];
            var fps = 30;
            int.TryParse(fpsSettings, out fps);
            return fps;
        }
    }
}
