using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace WinFormProject
{
    public partial class Money : Form
    {
        List<Bill> allbill = new List<Bill>(); //ประกาศตัวแปรบิล
        public string allprice;
        private MySqlConnection databaseConnection() 
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=project;";
            MySqlConnection conn2 = new MySqlConnection(connectionString);
            return conn2;
        }
        public Money()
        {
            InitializeComponent();
        }
        private void showdataGridView2() //แสดงเมนูอาหารที่สั่ง
        {
            MySqlConnection conn = databaseConnection();
            DataSet ds = new DataSet();

            conn.Open();

            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID,menu,price,type FROM saledata WHERE status = '" + "Notpaid" + "'  ";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();

            dataGridView1.DataSource = ds.Tables[0].DefaultView;
        }
        
        private void button3_Click(object sender, EventArgs e) //คิดเงิน
        {
            int A = int.Parse(textBox1.Text);
            int B = int.Parse(textBox2.Text);
            string time = DateTime.Now.ToString("dd/MM/yyyy");
            if (textBox2.Text == "0" || textBox2.Text == "") //เราไม่จ่ายเงินสักบาท
            {
                MessageBox.Show("กรุณาจ่ายเงินด้วย", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (B < A) //ถ้าจ่ายเงินไม่ครบ
                {
                    MessageBox.Show("กรุณาจ่ายเงินให้ครบด้วย", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    int C = B - A;
                    textBox3.Text = C.ToString();


                    MySqlConnection conn = databaseConnection(); //อัพเดทขึ้นว่าชำระหรือยัง
                    String sql = "UPDATE saledata SET `datetime`='"+time+"',status = '" + "Yes paid" + "' WHERE status = '" + "Notpaid" + "' ";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (rows > 0)
                    {
                        MessageBox.Show("ขอบคุณที่ใช้บริการ");
                        printPreviewDialog1.Document = printDocument1; //ปริ้นใบเสร็จ
                        printPreviewDialog1.ShowDialog();
                        showdataGridView2();
                        
                    }

                }
            }
        }
        

        private void Money_Load(object sender, EventArgs e) //คิดเงินใบเสร็จ
        {
            
            showdataGridView2();
            textBox1.Text = allprice;
            
            allbill.Clear();
            MySqlConnection conn = databaseConnection();
            MySqlCommand cmd = new MySqlCommand($"SELECT * FROM saledata WHERE Username = \"{Program.Username}\"AND Status = '" + "NotPaid" + "'", conn);
            conn.Open();
            MySqlDataReader adapter = cmd.ExecuteReader();
            while (adapter.Read())
            {
                Program.menu = adapter.GetString("menu").ToString();
                Program.price = adapter.GetString("price").ToString();
                Program.type = adapter.GetString("type").ToString();
                Bill item = new Bill()
                {
                    menu = Program.menu,
                    price = Program.price,
                    type = Program.type,
                };
                allbill.Add(item);
            }
           

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) //ใบเสร็จ
        {
            Image logo = Image.FromFile(@"E:\New folder2\logo.png"); //โลโก้
            e.Graphics.DrawImage(logo, new PointF(400,700));
            e.Graphics.DrawString("ใบเสร็จ", new Font("supermarket", 28, FontStyle.Bold), Brushes.Black, new Point(400, 50));
            e.Graphics.DrawString("ร้านทำเส้น", new Font("supermarket", 24, FontStyle.Bold), Brushes.Black, new Point(365, 90));
            e.Graphics.DrawString(" วัน   " + System.DateTime.Now.ToString("dd/MM/yyyy "), new Font("supermarket", 16, FontStyle.Regular), Brushes.Black, new PointF(580, 150));
            e.Graphics.DrawString("เวลา  " + System.DateTime.Now.ToString("HH : mm : ss น."), new Font("supermarket", 16, FontStyle.Regular), Brushes.Black, new PointF(570, 180));
            e.Graphics.DrawString("ข้อมูลร้าน : ร้านทำเส้น By cafe Benz", new Font("supermarket", 18, FontStyle.Regular), Brushes.Black, new Point(80, 150));
            e.Graphics.DrawString("         บ้านเลขที่ 66 หมู่ 6 ", new Font("supermarket", 18, FontStyle.Regular), Brushes.Black, new Point(80, 195));
            e.Graphics.DrawString("         อำเภอหนองหาน จังหวัดอุดรธานี 41130", new Font("supermarket", 18, FontStyle.Regular), Brushes.Black, new Point(80, 230));
            e.Graphics.DrawString("-----------------------------------------------------------------------------------", new Font("supermarket", 16, FontStyle.Regular), Brushes.Black, new Point(80, 285));
            e.Graphics.DrawString("    ลำดับ       ชื่อรายการอาหาร             ราคา (บาท)         ประเภทอาหาร", new Font("supermarket", 18, FontStyle.Regular), Brushes.Black, new Point(80, 315));
            e.Graphics.DrawString("-----------------------------------------------------------------------------------", new Font("supermarket", 16, FontStyle.Regular), Brushes.Black, new Point(80, 345));
            int y = 345;
            int number = 1;
            foreach (var i in allbill)
            {
                y = y + 35;
                e.Graphics.DrawString("   " + number.ToString(), new Font("supermarket", 16, FontStyle.Regular), Brushes.Black, new PointF(100, y));
                e.Graphics.DrawString("   " + i.menu, new Font("supermarket", 16, FontStyle.Regular), Brushes.Black, new PointF(190, y));// ชื่อรายการอาหาร
                e.Graphics.DrawString("   " + i.price, new Font("supermarket", 16, FontStyle.Regular), Brushes.Black, new PointF(450, y));//ราคา
                e.Graphics.DrawString("   " + i.type, new Font("supermarket", 16, FontStyle.Regular), Brushes.Black, new PointF(600, y));//ประเภท
                number = number + 1;
            }
            e.Graphics.DrawString("-----------------------------------------------------------------------------------", new Font("supermarket", 16, FontStyle.Regular), Brushes.Black, new Point(80, y + 30));
            e.Graphics.DrawString("รวมทั้งสิ้น   " + textBox1.Text + "    บาท", new Font("supermarket", 16, FontStyle.Regular), Brushes.Black, new Point(490, (y + 30) + 45));
            e.Graphics.DrawString("ชื่อผู้ให้บริการ     " + Program.Username, new Font("supermarket", 16, FontStyle.Bold), Brushes.Black, new Point(80, (y + 30) + 45));
            e.Graphics.DrawString("รับเงิน        " + textBox2.Text + "  บาท", new Font("supermarket", 16, FontStyle.Regular), Brushes.Black, new Point(490, ((y + 30) + 45) + 45));
            e.Graphics.DrawString("เงินทอน      " + textBox3.Text + "   บาท", new Font("supermarket", 16, FontStyle.Regular), Brushes.Black, new Point(490, (((y + 30) + 45) + 45) + 45));
        }

        private void button1_Click(object sender, EventArgs e) //ไปดูหน้าประวัติการขาย
        {
            Form3 a = new Form3();
            this.Hide();
            a.Show();
        }

        private void button2_Click(object sender, EventArgs e) //ออกจากโปรแกรม
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e) //กลับไปสั่งอาหาร
        {
            Form7 a = new Form7();
            this.Hide();
            a.Show();
        }
    }

}