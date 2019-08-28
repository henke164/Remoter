using System.Drawing;

namespace Remoter.Server.Services
{
    public class ScreenshotProvider
    {
        private Rectangle _bounds;

        public ScreenshotProvider(Rectangle bounds, float scalingFactor)
        {
            _bounds = new Rectangle(
                bounds.X,
                bounds.Y,
                (int)(bounds.Width * scalingFactor),
                (int)(bounds.Height * scalingFactor));
        }

        public Bitmap CaptureScreenshot()
        {
            using (var bitmap = new Bitmap(_bounds.Width, _bounds.Height))
            {
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, _bounds.Size);
                }
                return (Bitmap)bitmap.Clone();
            }
        }
    }
}
