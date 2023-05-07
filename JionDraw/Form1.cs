using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace JionDraw
{
    public partial class Form1 : Form
    {
        TcpListener tcpListener;
        TcpClient tcpClient;
        public Form1()
        {
            InitializeComponent();
            tcpListener = new TcpListener(IPAddress.Any, 8025); ;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(AcceptCallback), null);
        }
        private void AcceptCallback(IAsyncResult ar)
        {
            tcpClient = tcpListener.EndAcceptTcpClient(ar);

            // Start reading in a separate thread
            byte[] bytes = new byte[1024];
            tcpClient.GetStream().BeginRead(bytes, 0, bytes.Length, new AsyncCallback(ReadCallback), bytes);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            int bytesRead = tcpClient.GetStream().EndRead(ar);
            byte[] bytes = (byte[])ar.AsyncState;
            string message = Encoding.ASCII.GetString(bytes, 0, bytesRead);

            // Parse the message
            string[] parts = message.Split(',');
            string shapeName = parts[0];
            int x1 = int.Parse(parts[1]);
            int y1 = int.Parse(parts[2]);
            int x2 = int.Parse(parts[3]);
            int y2 = int.Parse(parts[4]);

            // Draw the shape
            Graphics g = pictureBox1.CreateGraphics();
            Pen pen = new Pen(Color.Red, 3);
            if (shapeName == "line")
            {
                g.DrawLine(pen, x1, y1, x2, y2);
            }
            else if (shapeName == "rectangle")
            {
                g.DrawRectangle(pen, x1, y1, x2 - x1, y2 - y1);
            }
            // Add more shape types as needed

            // Start reading again
            tcpClient.GetStream().BeginRead(bytes, 0, bytes.Length, new AsyncCallback(ReadCallback), bytes);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}