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
using System.IO;


namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;//在多執行緒程式設計中,如果需要使用大到主執行緒需要進行檢查取消
            dataGridView1.DataSource = sampleData();

        }
        IDictionary<string, Socket> clientList = new Dictionary<string, Socket>();


        private void Form1_Load(object sender, EventArgs e)

        {
            textBox3.ScrollBars = ScrollBars.Both;
            textBox2.Text = "3000";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(StartSever);
            thread.IsBackground = true;
            thread.Start();
        }
        void StartSever()
        {
            Socket sever = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);   //1.建立伺服器端電話
            IPAddress ip = IPAddress.Parse(textBox1.Text);
            IPEndPoint end = new IPEndPoint(ip, int.Parse(textBox2.Text));
            sever.Bind(end); //3.將電話卡插入到電話中,繫結埠
            sever.Listen(20);
            listMsg.Items.Add("開啟");

            while (true)//5.等待接電話
            {
                Socket connectClient = sever.Accept();
                if (connectClient != null)
                {
                    string information = connectClient.RemoteEndPoint.ToString();

                    clientList.Add(information, connectClient);
                    string msg = information + "進入了";
                    SendMsg(msg);
                    Thread th = new Thread(RecivMsg);
                    th.IsBackground = true;
                    th.Start(connectClient);

                }

            }

        }
        void RecivMsg(object o)
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
                        listMsg.Items.Add(DateTime.Now + "[" + endpoint.Port.ToString() + "]" + recMsg);
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

        void SendMsg(string str)
        {
            foreach (var item in clientList)
            {
                byte[] arrMsg = Encoding.UTF8.GetBytes(str);
                item.Value.Send(arrMsg);
            }
        }





        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                SendMsg(textBox4.Text);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "選擇檔案";
            dialog.Filter = "所有檔案(*.*)|*.*";
            var fileContent = string.Empty;
            var filePath = string.Empty;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = dialog.FileName;
                textBox3.Text = filePath;

                //Read the contents of the file into a stream
                var fileStream = dialog.OpenFile();

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    fileContent = reader.ReadToEnd();
                }
                textBox4.Text = fileContent;


            }
        }


        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private DataTable sampleData()
        {
            using (DataTable table = new DataTable())
            {
                // Add columns.
                table.Columns.Add("連線", typeof(string));
                table.Columns.Add("IP", typeof(string));
                table.Columns.Add("檔案 ", typeof(string));
                table.Columns.Add("傳送狀況", typeof(string));

                // Add rows.
                bool  correct=checkBox1.Checked= true;
                table.Rows.Add(correct, "192.168.22.12", 0, "完畢");
                table.Rows.Add("Kevin", "Male", 1, DateTime.Now);
                table.Rows.Add("Dean", "Male", 0, DateTime.Today);
                table.Rows.Add("Jenny", "Female", 1, DateTime.Today);
                return table;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}


   
