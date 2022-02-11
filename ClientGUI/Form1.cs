using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace ClientGUI
{
    public partial class Form1 : Form
    {
        Socket sock;
        Socket acc;
      
        public Form1()
        {
            InitializeComponent();
            sock = socket();

        }
        Socket socket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                sock.Connect(new IPEndPoint(IPAddress.Parse(textBox1.Text), 3));
                new Thread(() =>
                {
                    read();
                }).Start();
            }
            catch {
                MessageBox.Show("Error");
            }

        }

        void read() {

            while (true) {
                try
                {
                    byte[] buffer = new byte[255];
                    int rec = sock.Receive(buffer, 0, buffer.Length, 0);

                    if (rec <= 0)
                    {
                        throw new SocketException();
                    }

                    Array.Resize(ref buffer, rec);

                    Invoke((MethodInvoker)delegate
                    {
                        listBox1.Items.Add("From Server: "+Encoding.Default.GetString(buffer));
                    });

                }
                catch
                {
                    MessageBox.Show("Disconnected");
                    Application.Exit();
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.Default.GetBytes(textBox1.Text);
            sock.Send(data, 0, data.Length, 0);
        }
    }
}
