using Remoter.Client.Services;
using Remoter.Shared.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Remoter.Client
{
    public partial class Form1 : Form
    {
        private ImageClient _imageClient = new ImageClient();
        private CommandClient _commandClient = new CommandClient();

        private MouseEventDispatcher _mouseEventDispatcher;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            _mouseEventDispatcher = new MouseEventDispatcher(_commandClient);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _commandClient.Connect();
            _imageClient.Connect();
            _imageClient.ListenForScreenshots((byte[] imageData) =>
            {
                var image = (Image)new ImageConverter().ConvertFrom(imageData);
                ThreadSafeUpdatePictureBox(image);
            });
        }

        private void ThreadSafeUpdatePictureBox(Image image)
        {
            pictureBox1.BeginInvoke(new MethodInvoker(() =>
            {
                pictureBox1.Image = image;
            }));
        }
        
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
            => _mouseEventDispatcher.TriggerMouseDown(e);

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
            => _mouseEventDispatcher.TriggerMouseUp(e);

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
            => _mouseEventDispatcher.TriggerMouseMove(e);

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
            => _mouseEventDispatcher.TriggerMouseLeave();
    }
}
