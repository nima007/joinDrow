using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JionDraw
{
    public partial class Form2 : Form
    {
        private Graphics g;
        private Pen pen;
        private Point startPoint;
        private Point endPoint;
        private List<string> shapes;
        public Form2()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
            pen = new Pen(Color.Black, 2);
            shapes = new List<string>();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            startPoint = e.Location;
        }
        private void drawingPanel_MouseUp(object sender, MouseEventArgs e)
        {
            endPoint = e.Location;
            if (startPoint != endPoint)
            {
                // Draw the shape on the panel
                if (shapeComboBox.SelectedItem.ToString() == "Line")
                {
                    g.DrawLine(pen, startPoint, endPoint);
                }
                else if (shapeComboBox.SelectedItem.ToString() == "Rectangle")
                {
                    g.DrawRectangle(pen, Math.Min(startPoint.X, endPoint.X), Math.Min(startPoint.Y, endPoint.Y), Math.Abs(startPoint.X - endPoint.X), Math.Abs(startPoint.Y - endPoint.Y));
                }
                // Add more shape types as needed

                // Add the shape to the list
                shapes.Add(shapeComboBox.SelectedItem.ToString() + "," + startPoint.X + "," + startPoint.Y + "," + endPoint.X + "," + endPoint.Y);
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            try
            {
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect("127.0.0.1", 8025);
                NetworkStream networkStream = tcpClient.GetStream();
                foreach (string shape in shapes)
                {
                    byte[] data = Encoding.ASCII.GetBytes(shape);
                    networkStream.Write(data, 0, data.Length);
                }
                networkStream.Close();
                tcpClient.Close();

                // Clear the list and the drawing panel
                shapes.Clear();
                g.Clear(Color.White);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
