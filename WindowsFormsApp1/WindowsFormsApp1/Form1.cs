using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly TcpClient client = new TcpClient();
        private NetworkStream mainStream;
        private int portNumber;

        private static Image GrabDesktop()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(bounds.Width,bounds.Height,PixelFormat.Format32bppRgb);
            Graphics graphics = Graphics.FromImage(screenshot);
            graphics.CopyFromScreen(bounds.X,bounds.Y,0,0,bounds.Size,CopyPixelOperation.SourceCopy);
            return screenshot;
        }
  
        private void SendDesktopImage()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            mainStream = client.GetStream();
            binaryFormatter.Serialize(mainStream,GrabDesktop());

            
        }
        public Form1()
        {
            InitializeComponent();

        }



        private void Connect_Click(object sender, EventArgs e)
        {
            portNumber = int.Parse(txtPort.Text);
            try
            {
                client.Connect(txtIp.Text, portNumber);
                MessageBox.Show("Successfully Connected!");
            }
            catch (Exception)
            {
                MessageBox.Show("Connection Failure!");

            }

        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            if(button2.Text.StartsWith("Share"))
            {
                timer1.Start();
                button2.Text = "Stop Sharing";
            }
            else
            {
                timer1.Stop();
                button2.Text = "Share my Screen";
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendDesktopImage();
        }

    }
}
