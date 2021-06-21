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
namespace WinFormProject
{
    public partial class Form3 : Form
    {
        
        int total;
        public Form3()
        {
            InitializeComponent();
        }
        private MySqlConnection databaseConnection() //ดึงข้อมูลดาต้าเบส
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=project;";
            MySqlConnection conn2 = new MySqlConnection(connectionString);
            return conn2;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            dateTimePicker2.Value = System.DateTime.Now;
            dateTimePicker1.Value = System.DateTime.Now;
            label2.Text = Program.Username;
            if (Program.Username == "admin")
            {
                ShowdataproductAdmin();
            }
            else
            {
                showhistorysald();
            }
            summ = 0;
            sumsale();


        }
        private void ShowdataproductAdmin()
        {
            MySqlConnection conn = databaseConnection();

            DataSet ds = new DataSet();

            conn.Open();

            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT menu,price,datetime,Username FROM saledata WHERE status = '" + "Yes paid" + "' ";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();

            dataGridView1.DataSource = ds.Tables[0].DefaultView;
        }

        private void showhistorysald() //โชว์ประวัติการขาย
        {
            MySqlConnection conn = databaseConnection();
            DataSet ds = new DataSet();
            conn.Open();
            MySqlCommand cmd;
            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT menu,price,datetime,Username FROM saledata WHERE status = '" + "Yes paid" + "' and Username = '" + label2.Text + "' ";
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);
            conn.Close();
            dataGridView1.DataSource = ds.Tables[0].DefaultView;
        }
        int summ;
        private void sumsale() //ยอดรวมที่ขายได้
        {
            MySqlConnection conn = databaseConnection();
            DataSet ds = new DataSet();
            conn.Open();
            MySqlCommand cmd;
            cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT price FROM saledata WHERE Username = \"{label2.Text}\" ";
            MySqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                int kk = read.GetInt32("price"); //รวมราคาที่ขายได้
                summ = summ + kk;
            }
            conn.Close();
            textBox4.Text = Convert.ToString(summ);


        }

        private void button1_Click(object sender, EventArgs e) //ปุ่มกลับไปสั่งอาหาร
        {
            Form7 a = new Form7();
            this.Hide();
            a.Show();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) //ดาต้ากิตวิว
        {
            dataGridView1.CurrentRow.Selected = true;
        }

        private void button2_Click(object sender, EventArgs e) //ออกจากโปรแกรม
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }



        private void button3_Click(object sender, EventArgs e) //ค้นหารายการขายตามวัน
        {
            string su = Program.Username;
            if (textBox1.Text != "")
            {

                MySqlConnection conn = databaseConnection();

                DataSet ds = new DataSet();

                conn.Open();
                MySqlCommand cmd;

                cmd = conn.CreateCommand();
                cmd.CommandText = $"SELECT menu,price,datetime,Username FROM saledata WHERE  Username=@data OR menu=@data  AND datetime between @date1 and @date2  "; //ค้นหาชื่อจากUser,=อาหาร

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand.Parameters.AddWithValue("@date1", dateTimePicker2.Value.ToString("dd/MM/yyyy")); //เอาค่าจาก dateTimePicker ไปเก็บที่ parameters @date1
                da.SelectCommand.Parameters.AddWithValue("@date2", dateTimePicker1.Value.ToString("dd/MM/yyyy"));
                da.SelectCommand.Parameters.AddWithValue("@data", textBox1.Text);
                //da.SelectCommand.Parameters.AddWithValue("@data2", su);

                da.Fill(ds);
                conn.Close();
                dataGridView1.DataSource = ds.Tables[0].DefaultView;

                total = 0; //ตัวแปรยอดรวมจำนวนเงิน
                conn.Open();
                MySqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {

                    total = total + int.Parse(read.GetString(1));
                }

                textBox4.Text = $"{total}"; //โชว์เงินในเทคบล็อก4
                conn.Close();
            }
            else
            {

                MySqlConnection conn = databaseConnection();

                DataSet ds = new DataSet();

                conn.Open();
                MySqlCommand cmd;

                cmd = conn.CreateCommand();
                //cmd.CommandText = $"SELECT menu,price,datetime,Username FROM saledata WHERE datetime between @date1 and @date2 AND Username = @data3    ";
                cmd.CommandText = $"SELECT menu,price,datetime,Username FROM saledata WHERE datetime between @date1 and @date2 ";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.SelectCommand.Parameters.AddWithValue("@date1", dateTimePicker2.Value.ToString("dd/MM/yyyy"));
                da.SelectCommand.Parameters.AddWithValue("@date2", dateTimePicker1.Value.ToString("dd/MM/yyyy"));
                //da.SelectCommand.Parameters.AddWithValue("@data3", su);

                da.Fill(ds);
                conn.Close();
                dataGridView1.DataSource = ds.Tables[0].DefaultView;

                total = 0; //ตัวแปรยอดรวมจำนวนเงิน
                conn.Open();
                MySqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {

                    total = total + int.Parse(read.GetString(1));
                }

                textBox4.Text = $"{total}";
                conn.Close();
            }
        }
    }
}
     
     


 


