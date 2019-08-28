using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remoter.Server.Services
{
    public class ImageSender
    {
        private ScreenshotProvider _screenshotProvider;
        private TcpServer _server = new TcpServer();
        private int _fps = 30;

        public ImageSender(float scaleFactor, int fps)
        {
            _fps = fps;
            _screenshotProvider = new ScreenshotProvider(Screen.PrimaryScreen.Bounds, scaleFactor);
        }

        public void Start()
        {
            _server.Start();
            Task.Run(() => Update());
        }

        private void Update()
        {
            var image = _screenshotProvider.CaptureScreenshot();
            var bytes = ImageToByte(image);
            _server.SendImage(bytes, image.Width, image.Height);
            Thread.Sleep(1000 / _fps);
            Task.Run(() => Update());
        }

        private byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

    }
}
