using Remoter.Server.Services;
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
            var imageSender = new ImageSender(scaleFactor, fps);
            imageSender.Start();

            Console.WriteLine("Server started! Listening for connections...");
            Console.ReadLine();
        }

        private static float GetScaleFactor()
        {
            var scaleSettings = ConfigurationManager.AppSettings["ScaleFactor"];
            var scaleFactor = 1f;
            float.TryParse(scaleSettings, out scaleFactor);
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
