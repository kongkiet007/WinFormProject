using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace WinFormProject
{
    public partial class Form2 : Form
    {
        
        
    

        
        public Form2()
        {
            InitializeComponent();
        }
        
        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=project;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }

        private void showdataGridView1() //แสดงเมนูอาหาร
        {
            MySqlConnection conn = databaseConnection();
            DataSet ds = new DataSet();

            conn.Open();

            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM data WHERE type = \"อาหารคาว\" ";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();

            dataGridView1.DataSource = ds.Tables[0].DefaultView;
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
      
        private void Form2_Load(object sender, EventArgs e)
        {



        }

        private void button1_Click(object sender, EventArgs e) //ออกจากโปรแกรม
        {
            Application.Exit();
        }




        private void เมนเสนๆToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "เมนูเส้นๆ"; //แถบรายการ
            label2.Text = "เมนูอาหารเส้น";  
            //dataSen.DataSource = null;
            pictureBox1.Image = null;
            

            showdataGridView1();
        }

        private void เมนของหวานToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "เมนูหวานๆ";
            label2.Text = "เมนูของหวาน";
            //dataSen.DataSource = null;
            pictureBox1.Image = null;
            
            
            showdataSugar();
        }

        private void button5_Click(object sender, EventArgs e) //ลบรายการ
        {
            int selectedRow = dataGridView1.CurrentCell.RowIndex;
            int deleteId = Convert.ToInt32(dataGridView1.Rows[selectedRow].Cells["ID"].Value);


            MySqlConnection conn = databaseConnection();

            String sql = "DELETE FROM data WHERE ID = '" + deleteId + "'";

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            conn.Open();

            int rows = cmd.ExecuteNonQuery();

            conn.Close();
            if (rows > 0)
            {
                showdataGridView1();
                MessageBox.Show("ลบข้อมูลสำเร็จ","", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
        }

        private void button6_Click(object sender, EventArgs e) //เพิ่มรายการ
        {
            try
            {
               


                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                byte[] img = ms.ToArray();

                MySqlConnection conn2 = databaseConnection();

                string sql2 = "INSERT INTO data(menu, price,type,picture) VALUES('" + NameText.Text + "','" + PriceText.Text + "','" + category.Text + "',@img)";

                MySqlCommand cmd2 = new MySqlCommand(sql2, conn2);

                conn2.Open();
                cmd2.Parameters.Add("@img", MySqlDbType.Blob); //เพิ่มรูปภาพเข้าดาต้าเบส
                cmd2.Parameters["@img"].Value = img;

                int rows2 = cmd2.ExecuteNonQuery();

                conn2.Close();

                if (rows2 > 0)
                {
                    MessageBox.Show("เพิ่มข้อมูลสำเร็จ", "เพิ่มข้อมูลสำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    showdataGridView1();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void button7_Click(object sender, EventArgs e) //แก้ไข้รายการ
        {
            int selectedRow = dataGridView1.CurrentCell.RowIndex;
            int editId = Convert.ToInt32(dataGridView1.Rows[selectedRow].Cells["ID"].Value);
            MySqlConnection conn = databaseConnection();
            byte[] image = null;
            string filepath = textBox1.Text;
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            image = br.ReadBytes((int)fs.Length);
            String sql = "UPDATE data SET menu = '" + NameText.Text + "',price = '" + PriceText.Text + "' WHERE ID = '" + editId + "'";
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add(new MySqlParameter("@Imgg", image));
            int rows = cmd.ExecuteNonQuery();
            conn.Close();
            if (rows > 0)
            {

                MessageBox.Show("ข้อมูลแก้ไขสำเร็จ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                showdataGridView1();
            }
          
        }

        

        private void เครองดมToolStripMenuItem_Click(object sender, EventArgs e) //แถบเมูเครื่องดื่ม
        {
            label1.Text = "เมนูเครื่องดื่ม";
            label2.Text = "เมนูเครื่องดื่ม";
            //dataSen.DataSource = null;
            pictureBox1.Image = null;
           
            
            showdatadrink();
        }
        
        private void button8_Click(object sender, EventArgs e) //เพิ่มรูปภาพ
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Choose Image(*.jpg; *.png; .gif)|.jpg; *.png; *.gif";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(opf.FileName);
                textBox1.Text = opf.FileName; //โชว์ที่มารูปภาพ
            }
            
        }


       
       
        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
       

        
    }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e) //ข้อมูลตารางแรกและใส่รูปภาพ
        {
            dataGridView1.CurrentRow.Selected = true; //เซลล์คลิก

            int selectedRows = dataGridView1.CurrentCell.RowIndex;
            int PC = Convert.ToInt32(dataGridView1.Rows[selectedRows].Cells["ID"].Value);

            NameText.Text = dataGridView1.Rows[e.RowIndex].Cells["menu"].FormattedValue.ToString();
            PriceText.Text = dataGridView1.Rows[e.RowIndex].Cells["price"].FormattedValue.ToString();
            category.Text = dataGridView1.Rows[e.RowIndex].Cells["type"].FormattedValue.ToString();
            try
            {


                MySqlConnection conn = databaseConnection();

                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT picture FROM data WHERE ID = \"{ PC }\"", conn); //ใส่รูปภาพ
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MemoryStream ms = new MemoryStream((byte[])ds.Tables[0].Rows[0]["picture"]);
                    pictureBox2.Image = new Bitmap(ms);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) //ไปหน้าสั่งอาหาร
        {
            Form7 a = new Form7();
            this.Hide();
            a.Show();
        }

        private void button2_Click(object sender, EventArgs e) //ออกจากระบบ
        {
            Form1 a = new Form1();
            this.Hide();
            a.Show();
        }
    }
}    

 







