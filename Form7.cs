using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;
namespace WinFormProject
{
    public partial class Form7 : Form
    {
      
        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=project;";
            MySqlConnection conn2 = new MySqlConnection(connectionString);
            return conn2;
        }
        public Form7()
        {
            InitializeComponent();
        }
        private void showdataGridView1() //แสดงเมนูอาหาร
        {
            MySqlConnection conn2 = databaseConnection();
            DataSet ds2 = new DataSet();

            conn2.Open();

            MySqlCommand cmd2;

            cmd2 = conn2.CreateCommand();
            cmd2.CommandText = "SELECT * FROM data WHERE type = \"อาหารคาว\" ";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd2);
            adapter.Fill(ds2);

            conn2.Close();

            dataGridView1.DataSource = ds2.Tables[0].DefaultView;
        }
        private void showdataGridView2() //แสดงเมนูอาหาร
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

            dataGridView2.DataSource = ds.Tables[0].DefaultView;
        }
        private void showMoney() // แสดงราคาทั้งหมด แล้วอยู่ใน กิตวิว แสดง ยอดรวม 
        {
            textBox1.Text = "0";
            MySqlConnection conn = databaseConnection();
            conn.Open();
            MySqlCommand cmd;
            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT SUM(price) FROM saledata WHERE status = '" + "Notpaid" + "'";
            Object sum = cmd.ExecuteScalar();
            conn.Close();
            if (Convert.ToString(sum) != "")
            {
                textBox1.Text = Convert.ToString(sum);
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) //แสดงรายการอาหาร
        {
            dataGridView1.CurrentRow.Selected = true; //เซลล์คลิก

            int selectedRows = dataGridView1.CurrentCell.RowIndex;
            int PC = Convert.ToInt32(dataGridView1.Rows[selectedRows].Cells["ID"].Value);

            NameText.Text = dataGridView1.Rows[e.RowIndex].Cells["menu"].FormattedValue.ToString();
            PriceText.Text = dataGridView1.Rows[e.RowIndex].Cells["price"].FormattedValue.ToString();
            type.Text = dataGridView1.Rows[e.RowIndex].Cells["type"].FormattedValue.ToString();
            try
            {


                MySqlConnection conn = databaseConnection();

                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT picture FROM data WHERE ID = \"{ PC }\"", conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MemoryStream ms = new MemoryStream((byte[])ds.Tables[0].Rows[0]["picture"]);
                    pictureBox1.Image = new Bitmap(ms);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void showdataSugar() //แสดงหน้าจอของหวาน
        {
            MySqlConnection conn = databaseConnection();
            DataSet ds = new DataSet();

            conn.Open();

            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM data WHERE type = \"ของหวาน\" ";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();

            dataGridView1.DataSource = ds.Tables[0].DefaultView;
        }
        private void showdatadrink() //แสดงหน้าจอเครื่องดื่ม
        {
            MySqlConnection conn = databaseConnection();
            DataSet ds = new DataSet();

            conn.Open();

            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM data WHERE type = \"เครื่องดื่ม\" ";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();

            dataGridView1.DataSource = ds.Tables[0].DefaultView;
        }
       

       
        private void From7_Load(object sender, EventArgs e)
        {
            showdataGridView1();
            showdataGridView2();
            showMoney();
            

        }

        private void เมนเสนๆToolStripMenuItem_Click(object sender, EventArgs e)
        {

            label1.Text = "เมนูเส้น ๆ"; //แถบรายการ
            label2.Text = "เลือกเมนูอาหารเส้น";
            
            pictureBox1.Image = null;


            showdataGridView1();
        }

        private void เมนของหวานToolStripMenuItem_Click(object sender, EventArgs e)
        {

            {
                label1.Text = "เมนูหวานๆ";
                label2.Text = "เลือกเมนูของหวาน";
             
                pictureBox1.Image = null;


                showdataSugar();
            }
        }

        private void เครองดมToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "เมนูเครื่องดื่ม";
            label2.Text = "เลือกเครื่องดื่ม";
            
            pictureBox1.Image = null;


            showdatadrink();
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e) //กลับไปหน้าแอดมิน
        {
            Form1 a = new Form1();
            this.Hide();
            a.Show();
        }



     

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e) //เซลคลิก 2
        {
            dataGridView2.CurrentRow.Selected = true;
            NameText.Text = dataGridView2.Rows[e.RowIndex].Cells["menu"].FormattedValue.ToString();
            PriceText.Text = dataGridView2.Rows[e.RowIndex].Cells["price"].FormattedValue.ToString();
            type.Text = dataGridView2.Rows[e.RowIndex].Cells["type"].FormattedValue.ToString();

        }

        private void button1_Click(object sender, EventArgs e) //สั่งอาหาร
        {
            MySqlConnection conn = databaseConnection();
            string sql = $"INSERT INTO saledata (menu,price,type,status,Username) VALUES(\"{ NameText.Text} \",\"{ PriceText.Text }\",\"{ type.Text }\",\"{ "Notpaid" }\",\"{Program.Username}\")";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {

                showdataGridView2();
                showMoney();


            }
        }

        private void button2_Click(object sender, EventArgs e) //ลบรายการอาหาร
        {
            if (textBox1.Text == "0")
            {
                MessageBox.Show("กรุณสั่งอาหาร"); //ยังไม่เลือกอะไรจะขึ้นแสดง
            }
            else
            {
                int selectedRow = dataGridView2.CurrentCell.RowIndex;
                int deleteId = Convert.ToInt32(dataGridView2.Rows[selectedRow].Cells["ID"].Value);
                MySqlConnection conn = databaseConnection();
                String sql = "DELETE FROM saledata WHERE ID = '" + deleteId + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();
                if (rows > 0)
                {
                    MessageBox.Show("ลบข้อมูลสำเร็จ");
                    showdataGridView2();
                    showMoney();
                }
            }
        }
        private void button3_Click(object sender, EventArgs e) //ไปหน้าคิดเงิน
        {
            this.Hide();
            Money n = new Money();
            n.allprice = textBox1.Text;
            n.Show();
        }

        private void button4_Click_1(object sender, EventArgs e) //ออกจากโปรแกรม
        {
            Application.Exit();
          
        }
    }
}
    





