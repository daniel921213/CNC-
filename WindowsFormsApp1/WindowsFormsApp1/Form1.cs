using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        Thread file_send;
        IDictionary<string, Socket> clientList = new Dictionary<string, Socket>();




        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.ScrollBars = ScrollBars.Both;

        }

        Socket Client;
        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(StartSever);
            thread.IsBackground = true;
            thread.Start();
        }
        void StartSever()
        {
            Socket sever = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);   //1.建立伺服器端電話
            IPAddress ip = IPAddress.Parse(textBox3.Text);
            IPEndPoint end = new IPEndPoint(ip, int.Parse(textBox4.Text));
            sever.Bind(end); //3.將電話卡插入到電話中,繫結埠
            sever.Listen(20);
            listView1.Items.Add("開啟");

            while (true)//5.等待接電話
            {
                Socket connectClient = sever.Accept();
                if (connectClient != null)
                {
                    string information = connectClient.RemoteEndPoint.ToString();

                    clientList.Add(information, connectClient);
                    string msg = information + "進入了";
                    SendMsg(msg);
                    Thread th = new Thread(ReciveMsg);
                    th.IsBackground = true;
                    th.Start(connectClient);
                    //this.dataGridView1.Rows[0].Cells[1].Value = true;




                }
            }

        }




        void ReciveMsg(object o)
        {
            Socket client = o as Socket;
            while (true)
            {
                try
                {
                    byte[] arrMsg = new byte[102 * 1024];
                    int length = client.Receive(arrMsg);
                    if (length > 0)
                    {
                        string recMsg = Encoding.UTF8.GetString(arrMsg, 0, length);
                        IPEndPoint endpoint = client.RemoteEndPoint as IPEndPoint;
                        listView1.Items.Add(DateTime.Now + "[" + endpoint.Port.ToString() + "]" + recMsg);
                        textBox3.Text = recMsg;
                        SendMsg("[" + endpoint.Port.ToString() + "]" + recMsg);

                    }
                }
                catch (Exception)
                {
                    client.Close();
                    clientList.Remove(client.RemoteEndPoint.ToString());
                }
            }
        }

            private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != null)
            {
                SendMsg(textBox2.Text);
            }
        }
        void SendMsg(string msg)
        {
            {
                foreach (var item in clientList)
                {
                    byte[] arrMsg = Encoding.UTF8.GetBytes(msg);
                    item.Value.Send(arrMsg);
                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {

        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    myStream.Close();
                    textBox5.Text = saveFileDialog1.FileName + "完成存檔";
                }
                else
                {
                    textBox5.Text = "沒有存成功";
                }

            
            }


        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }

}
