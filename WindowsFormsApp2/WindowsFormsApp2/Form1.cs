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
using System.Diagnostics; //診斷


namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        DateTime beforeDT = DateTime.Now;
        string mt = "..";
        string mt1 = "...";
        string mt2 = "....";
        string mt3 = ".....";


        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;//在多執行緒程式設計中,如果需要使用大到主執行緒需要進行檢查取消
            dataGridView1.DataSource = sampleData();
            

        }
        //IDictionary<string, Socket> clientList = new Dictionary<string, Socket>();
        Socket Client;


        private void Form1_Load(object sender, EventArgs e)

        {
            try
            {
                this.FormClosing += new FormClosingEventHandler(Form1_Load);
                // 讀取設定到帳號 TextBox.Text
                this.textBox1.Text = WindowsFormsApp2.Properties.Settings.Default.IP;
                // 讀取設定到密碼 TextBox.Text
                this.textBox2.Text = WindowsFormsApp2.Properties.Settings.Default.PORT;


                string folder = @"C:\Setting";
                Directory.CreateDirectory(folder);
                textBox7.Text = "CNC";
                timer1.Interval = 1000;
                timer1.Enabled = true;
                timer1.Start();
                this.Text = beforeDT.ToString();
                textBox3.ScrollBars = ScrollBars.Both;
                textBox4.ScrollBars = ScrollBars.Both;
                textBox5.ScrollBars = ScrollBars.Both;
                int i = 0;
                char[] ncip = { ',' };
                //string str = System.IO.Directory.GetCurrentDirectory();
                string str = @"C:\Setting\";
                StreamReader sr = new StreamReader(str + "\\text1.txt");
                string line = sr.ReadLine();
                string[] a = line.Split(ncip);
                dataGridView1.Rows[i].Cells[1].Value = a[0];
                dataGridView1.Rows[i].Cells[2].Value = a[1];
                dataGridView1.Rows[i].Cells[3].Value = a[2];

                while (line != null)
                {
                    //Read the next line
                    line = sr.ReadLine();
                    if (line != null)
                    {
                        string[] a2 = line.Split(ncip);
                        i++;
                        dataGridView1.Rows[i].Cells[1].Value = a2[0];
                        dataGridView1.Rows[i].Cells[2].Value = a2[1];
                        dataGridView1.Rows[i].Cells[3].Value = a2[2];

                    }
                }
                //close the file
                sr.Close();
                StreamReader sr1 = new StreamReader(str + "\\text4.txt");
                string t4 = sr1.ReadLine();
                textBox6.Text = t4;
                sr1.Close();
                //StreamReader sr1 = new StreamReader( dd+ "\\text5.txt");
            }
            catch
            {

            }

        }

    

        void ReciveMsg(object o)
        {
            try
            {


                Socket client = o as Socket;
                //5.等待接電話
                while (true)
                {
                    byte[] arrlist = new byte[1024 * 1024];
                    int length = client.Receive(arrlist);
                    string msg = Encoding.UTF8.GetString(arrlist, 0, length);
                    listMsg.Items.Add(msg);
                    textBox3.Text = Encoding.UTF8.GetString(arrlist, 0, length);
                }
            }
            catch
            {
                MessageBox.Show("Sever端關閉","提醒", MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                for(int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = false;
                    dataGridView1.Rows[i].Cells[5].Value = "未連線";
                    

                }
                textBox3.Clear();
                listMsg.Clear();


            }

        }
        void SendMsg(string msg)
        {
            try
            {
                byte[] arrMsg = Encoding.UTF8.GetBytes(msg);
                Client.Send(arrMsg);
            }
            catch
            {
                MessageBox.Show("連線已斷開");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (textBox4.Text != "")
            {
                listMsg.Clear();
                SendMsg(textBox4.Text);
                textBox4.Clear();              
                //dataGridView1.rowindex
                string a=dataGridView1.CurrentCell.RowIndex.ToString();
                int b=Int32.Parse(a);
                dataGridView1.Rows[b].Cells[6].Value = "傳送完成";
            }
            else
            {
                MessageBox.Show("沒有字元", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private DataTable sampleData()
        {
            using (DataTable table = new DataTable())
            {
                // Add columns.
                table.Columns.Add("連線", typeof(string));
                table.Columns.Add("機台名稱", typeof(string));
                table.Columns.Add("IP", typeof(string));
                table.Columns.Add("檔案", typeof(string));
                table.Columns.Add("...", typeof(string));
                table.Columns.Add("狀態", typeof(string));
                table.Columns.Add("傳送狀況", typeof(string));
                table.Columns.Add("連接", typeof(string));
                table.Columns.Add("連線時間", typeof(string));

                // Add rows.
                string IP = "192.168.22";
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                table.Rows.Add(false, "", "", "", "...", "", "", "連線", "");
                return table;
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 7)
            {
                try
                {

                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        dataGridView1.Rows[i].Cells[5].Value = "未連線";
                    }


                    //建立伺服器端電話                 
                    Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    //建立手機卡
                    IPAddress ip = IPAddress.Parse(textBox1.Text + dataGridView1.Rows[e.RowIndex].Cells[2].Value);
                    IPEndPoint endinput = new IPEndPoint(ip, int.Parse(textBox2.Text));
                    startProgress();
                    startProgress1();
                    Client.Connect(endinput);
                    Thread th = new Thread(ReciveMsg);
                    th.IsBackground = true;
                    th.Start(Client);
                    dataGridView1.Rows[e.RowIndex].Cells[5].Value = "已連線";
                    // Thread thread = new Thread(StartSever);
                    //thread.IsBackground = true;
                    //thread.Start();
                    if (dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString() == "已連線")
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[0].Value = true;
                        DateTime nowtime = DateTime.Now;
                        dataGridView1.Rows[e.RowIndex].Cells[8].Value = nowtime;

                    }


                }
                catch
                {
                    MessageBox.Show("IP或port錯誤", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }

            if (e.ColumnIndex == 4)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = "選擇檔案";
                dialog.Filter = "所有檔案(*.*)|*.*";
                var fileContent = string.Empty;
                var filePath = string.Empty;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = dialog.FileName;
                    dataGridView1.Rows[e.RowIndex].Cells[3].Value = filePath;

                    //Read the contents of the file into a stream
                    var fileStream = dialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                    textBox5.Text = fileContent;
                }

            }
            if (dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString() == "已連線")
            {
                if (e.ColumnIndex == 3)
                {
                    var fileContent = string.Empty;
                    var filePath = string.Empty;
                    var fileStream = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                    textBox4.Text = fileContent;


                }
            }
            if (e.ColumnIndex == 3)
            {

                var fileContent = string.Empty;
                var filePath = string.Empty;
                var fileStream = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    fileContent = reader.ReadToEnd();
                }
                textBox5.Text = fileContent;

            }

        
            
            if (e.ColumnIndex == 1)
            {

                string tt = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                label5.Text = tt;

                //radioButton4.Text = radioButton4.Text+ tt;
                //  textBox4.Text = 
            }
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string dd = textBox6.Text;
            string str = @"C:\Setting\";
            StreamWriter sw = new StreamWriter(str + "\\text1.txt", false, Encoding.ASCII);
            //Write a line of text
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                sw.WriteLine(dataGridView1.Rows[i].Cells[1].Value + "," + dataGridView1.Rows[i].Cells[2].Value + "," + dataGridView1.Rows[i].Cells[3].Value);
            }
            sw.Close();

            StreamWriter sw1 = new StreamWriter(str + "\\text4.txt", false, Encoding.ASCII);
            sw1.WriteLine(textBox6.Text);
            sw1.Close();


            string folder = "";
            if (!Directory.Exists(folder))
            {
                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    string folder1 = dd + @"\" + dataGridView1.Rows[i].Cells[1].Value;

                    Directory.CreateDirectory(folder1);
                    folder1 = folder;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            {
                DateTime afterDT = DateTime.Now;
                TimeSpan ts = afterDT.Subtract(beforeDT);

                this.Text = beforeDT + " " + " " + "程式執行時間:" + ts.ToString().Substring(0, 8);//擷取字元
            }

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox6.Text = folderBrowserDialog1.SelectedPath;
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {

            if (radioButton1.Checked)
            {
                try
                {
                    string txt = textBox7.Text;
                    string a = textBox3.Text.Substring(0, textBox3.Text.Length - 1).Trim();
                    string b = a.Substring(1, 7).Trim();


                    if (File.Exists(textBox6.Text + @"\" + b + "." + txt))
                    {
                        if (MessageBox.Show("要直接覆蓋嗎?", "詢問", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {

                            string txt2 = textBox7.Text;
                            string a2 = textBox3.Text.Substring(0, textBox3.Text.Length - 1).Trim();
                            string b2 = a2.Substring(1, 7).Trim();
                            StreamWriter sw = new StreamWriter(textBox6.Text + @"\" + b2 + "." + txt2, false, Encoding.ASCII);
                            sw.WriteLine(textBox3.Text);
                            sw.Close();
                            textBox3.Clear();
                            MessageBox.Show(textBox6.Text + @"\" + b2 + "." + txt2 + "存檔成功", "文字框", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                        }
                    }

                    else
                    {
                        string txt1 = textBox7.Text;
                        string a1 = textBox3.Text.Substring(0, textBox3.Text.Length - 1).Trim();
                        string b1 = a1.Substring(1, 7).Trim();
                        StreamWriter sw = new StreamWriter(textBox6.Text + @"\" + b1 + "." + txt1, false, Encoding.ASCII);
                        sw.WriteLine(textBox3.Text);
                        sw.Close();
                        textBox3.Clear();
                        MessageBox.Show(textBox6.Text + @"\" + b1 + "." + txt1 + "存檔成功", "文字框", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                    }


                }
                catch
                {
                    MessageBox.Show("無%字元判斷或目錄不存在", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

            }
            else if (radioButton2.Checked)
            {
                try
                {
                    string txt = textBox7.Text;
                    string a = textBox3.Text.Substring(0, textBox3.Text.Length - 1).Trim();
                    string b = a.Substring(1, 7).Trim();
                    StreamWriter sw = new StreamWriter(textBox6.Text + @"\" + b + "." + txt, false, Encoding.ASCII);
                    sw.WriteLine(textBox3.Text);
                    sw.Close();
                    textBox3.Clear();
                    MessageBox.Show(textBox6.Text + @"\" + b + "." + txt + "存檔成功", "文字框", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                }
                catch
                {
                    MessageBox.Show("無%字元判斷或目錄不存在", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
               

            }
            else if (radioButton3.Checked)
            {
                try
                {
                    string localfile = "";
                    int counter = 1;
                    string txt = textBox7.Text;
                    string a = textBox3.Text.Substring(0, textBox3.Text.Length - 1).Trim();
                    string b = a.Substring(1, 7).Trim();
                    string newpath = textBox6.Text + @"\" + b + "." + txt;
                    while (File.Exists(newpath))
                    {
                        newpath = textBox6.Text + @"\" + b + "-" + counter + "." + txt;
                        counter++;
                    }
                    localfile = newpath;
                    StreamWriter sw = new StreamWriter(localfile, false, Encoding.ASCII);
                    sw.WriteLine(textBox3.Text);
                    sw.Close();
                    textBox3.Clear();
                    MessageBox.Show(newpath + "存檔成功", "文字框", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                }
                catch
                {
                    MessageBox.Show("無%字元判斷或目錄不存在", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);


                }

            }
            else if (radioButton4.Checked)
            {
                try
                {
                    string txt = textBox7.Text;
                    string a = textBox3.Text.Substring(0, textBox3.Text.Length - 1).Trim();
                    string b = a.Substring(1, 7).Trim();
                    string str = textBox6.Text;
                    // string txt = textBox7.Text;
                    //string a = textBox3.Text.Substring(1, 7).Trim();

                    if (File.Exists(str + @"\" + label5.Text + @"\" + b + "." + txt))
                    {
                        DialogResult dr;
                        dr = MessageBox.Show("要直接覆蓋嗎?\n取消鍵為建立排序檔", "詢問", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                        if (dr == DialogResult.Yes)
                        {

                            string txt1 = textBox7.Text;
                            string a1 = textBox3.Text.Substring(0, textBox3.Text.Length - 1).Trim();
                            string b1 = a1.Substring(1, 7).Trim();
                            string str1 = textBox6.Text;
                            StreamWriter sw = new StreamWriter(str1 + @"\" + label5.Text + @"\" + b1 + "." + txt1, false, Encoding.ASCII);
                            sw.WriteLine(textBox3.Text);
                            sw.Close();
                            textBox3.Clear();
                            MessageBox.Show(str1 + @"\" + label5.Text + @"\" + b1 + "." + txt1 + "存檔成功", "文字框", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                        }
                        else if (dr == DialogResult.Cancel)
                        {
                            dr = MessageBox.Show("確定要新增排序檔嗎?", "詢問", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == DialogResult.Yes)
                            {
                                string localfile = "";
                                int counter = 1;
                                string txt3 = textBox7.Text;
                                string a3 = textBox3.Text.Substring(0, textBox3.Text.Length - 1).Trim();
                                string b3 = a3.Substring(1, 7).Trim();
                                string newpath = textBox6.Text + @"\" + label5.Text + @"\" + b3 + "." + txt3;
                                while (File.Exists(newpath))
                                {
                                    newpath = textBox6.Text + @"\" + label5.Text + @"\" + b3 + "-" + counter + "." + txt;
                                    counter++;
                                }
                                localfile = newpath;
                                StreamWriter sw = new StreamWriter(localfile, false, Encoding.ASCII);
                                sw.WriteLine(textBox3.Text);
                                sw.Close();
                                textBox3.Clear();
                                MessageBox.Show(localfile + "存檔成功", "文字框", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);


                            }

                        }

                    }
                    else
                    {
                        string txt2 = textBox7.Text;
                        string a2 = textBox3.Text.Substring(0, textBox3.Text.Length - 1).Trim();
                        string b2 = a2.Substring(1, 7).Trim();
                        string str2 = textBox6.Text;
                        StreamWriter sw = new StreamWriter(str2 + @"\" + label5.Text + @"\" + b2 + "." + txt2, false, Encoding.ASCII);
                        sw.WriteLine(textBox3.Text);
                        sw.Close();
                        textBox3.Clear();
                        MessageBox.Show(str2 + @"\" + label5.Text + @"\" + b2 + "." + txt2 + "存檔成功", "文字框", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);


                    }

                }
                catch
                {
                    MessageBox.Show("無%字元判斷或目錄不存在", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                

            }

            else
            {
                MessageBox.Show("目錄不正確","錯誤",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            WindowsFormsApp2.Properties.Settings.Default.IP = this.textBox1.Text;
            WindowsFormsApp2.Properties.Settings.Default.PORT=this.textBox2.Text;
            WindowsFormsApp2.Properties.Settings.Default.Save();
        }

        private void startProgress()
        {
            progressBar1.Visible = true;
            // 設置進度條最小值.
            progressBar1.Minimum = 1;
            // 設置進度條最大值.
            progressBar1.Maximum = 15;
            // 設置進度條初始值
            progressBar1.Value = 1;
            // 設置每次增加的步長
            progressBar1.Step = 1;
            Graphics g = this.progressBar1.CreateGraphics();
            

            // 循環執行
            for (int x = 1; x <= 15; x++)
            {

                // 每次循環讓程序休眠300毫秒
                
                // 執行PerformStep()函數
                progressBar1.PerformStep();
                string str = Math.Round((100 * x / 15.0), 2).ToString("#0.00 ") + "%"+"  "+ "尋找IP";           
                Font font = new Font("Times New Roman", (float)8, FontStyle.Regular);
                PointF pt = new PointF(this.progressBar1.Width / 2 -17, this.progressBar1.Height / 2 - 7);
                g.DrawString(str, font, Brushes.Blue, pt);             
                System.Threading.Thread.Sleep(600);
            }
            progressBar1.Visible = false;
        }
        private void startProgress1()
        {
            progressBar2.Visible = true;
            // 設置進度條最小值.
            progressBar2.Minimum = 1;
            // 設置進度條最大值.
            progressBar2.Maximum = 15;
            // 設置進度條初始值
            progressBar2.Value = 1;
            // 設置每次增加的步長
            progressBar2.Step = 1;
            
            Graphics g = this.progressBar2.CreateGraphics();
            // 循環執行
            for (int x = 1; x <= 15; x++)
            {
                // 每次循環讓程序休眠300毫秒
               
                // 執行PerformStep()函數
                progressBar2.PerformStep();
                string str = Math.Round((100 * x / 15.0), 2).ToString("#0.00") + "%" + " "+"連線中";
                Font font = new Font("Times New Roman", (float)9, FontStyle.Regular);
                PointF pt = new PointF(this.progressBar2.Width / 2 - 10, this.progressBar2.Height / 2 - 7);
                g.DrawString(str, font, Brushes.Red, pt);
                
                System.Threading.Thread.Sleep(600);
            }
            progressBar2.Visible = false;
        }


       
    }
    
}









