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
    public partial class Form1 : Form
    {
        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=project;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) //ล็อกอินเข้า
        {
            MySqlConnection conn = databaseConnection();
            conn.Open();

            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM username WHERE User = \"{User.Text}\" AND Password = \"{Password.Text}\"";

            MySqlDataReader row = cmd.ExecuteReader();
            if (row.HasRows)
            {
                MessageBox.Show("เข้าสู่ระบบสำเร็จ"); //ใส่รหัสถูก
                Program.Username = User.Text;
                MySqlConnection con2 = databaseConnection();
                con2.Open();
                MySqlCommand cmd2 = con2.CreateCommand();
                cmd2.CommandText = $"SELECT status FROM Username WHERE user = \"{User.Text}\"";
                MySqlDataReader dr = cmd2.ExecuteReader();
                if (dr.Read())
                {
                    string Status = dr.GetValue(0).ToString();
                    if (Status == "admin")
                    {
                        Form2 a = new Form2();
                        this.Hide();
                        a.Show();
                    }
                    else
                    {
                        Form7 a = new Form7();
                        this.Hide();
                        a.Show();
                    }
                }
                else
                {
                    MessageBox.Show("ชื่อผู้ใช้ หรือ รหัสผ่านไม่ถูกต้อง"); //ใส่รหัสไม่ถูก
                }
                conn.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) //ไปสมัครสมาชิก
        {
            Form5 a = new Form5();
            this.Hide();
            a.Show();
        }

        private void User_KeyPress(object sender, KeyPressEventArgs e) //คีย์ได้แต่ภาษาอังกฤษ
        {
            if (System.Text.Encoding.UTF8.GetByteCount(new char[] { e.KeyChar }) > 1)
            {
                e.Handled = true;
            }
            if ((e.KeyChar == ' '))
            {
                e.Handled = true;
            }
        }

       

        private void Password_KeyPress(object sender, KeyPressEventArgs e) //คีย์ได้แต่ภาษาอังกฤษ
        {
            if (System.Text.Encoding.UTF8.GetByteCount(new char[] { e.KeyChar }) > 1)
            {
                e.Handled = true;
            }
            if ((e.KeyChar == ' '))
            {
                e.Handled = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) //โชว์หรือซ่อนรหัสผ่าน
        {
            if (checkBox1.Checked)
            {
                string P = Password.Text;
                Password.PasswordChar = '\0';
                
            }
            else
            {
                Password.PasswordChar = '•';
                
            }
        }

        private void button2_Click(object sender, EventArgs e) //ออกจากโปรแกรม
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

