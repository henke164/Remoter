using Remoter.Client.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Remoter.Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var client = new RemoterTcpClient();
            client.Connect();
            client.ListenForScreenshots((byte[] imageData) =>
            {
                var image = BytesToImage(imageData);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = image;
            });
        }

        public static Image BytesToImage(byte[] imageBytes)
        {
            var converter = new ImageConverter();
            return (Image)converter.ConvertFrom(imageBytes);
        }

    }
}
