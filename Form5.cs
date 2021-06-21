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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }
        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=project;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        private void Register() //สมัคร
        {
            string txtUser = User.Text; //กำหนัดตัวแปร
            string txtPass = Password.Text;
            string txtPhone = Phone.Text;
            string txtaddress = address.Text;
     

            MySqlConnection conn = databaseConnection();
            MySqlCommand command = new MySqlCommand("INSERT INTO `username`(`user`, `password`,`Phone`,`address`) VALUES (@user,@password,@Phone,@address)", conn);
            command.Parameters.Add("@user", MySqlDbType.VarChar).Value = User.Text;          
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = Password.Text;
            command.Parameters.Add("@Phone", MySqlDbType.VarChar).Value = Phone.Text;
            command.Parameters.Add("@address", MySqlDbType.VarChar).Value = address.Text;




            conn.Open();

            if (Password.Text.Equals(Confirmpassword.Text))
            {
                if (User.Text == "" || Password.Text == "" || Confirmpassword.Text == "" || Phone.Text == "" || address.Text =="")
                {
                    MessageBox.Show("กรุณากรอกให้ครบ!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else if (txtUser.Length < 6 || txtPass.Length < 6)
                {
                    MessageBox.Show("กรุณากรอก User,Pass 6-20 ตัว", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (txtPhone.Length < 10)
                {
                    MessageBox.Show("กรุณากรอกเบอร์โทรศัพท์ห้ครบ 10 ตัว", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (txtPhone.Length > 10)
                {
                    MessageBox.Show("กรุณากรอกเบอร์โทรศัพท์ไม่เกิน10 ตัว", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else if (checkUsername())
                {
                    MessageBox.Show("มีบัญชีผู้ใช้นี้อยู่แล้ว โปรดใช้'Username'อื่น!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("สมัครสมาชิกสำเร็จ");
                        
                    }

                }
            }
            else
            {
                MessageBox.Show("รหัสผ่านไม่ตรงกัน!", "รหัสผ่านไม่ตรงกัน!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public Boolean checkUsername() //เช็คผู้ใช้
        {
            MySqlConnection conn = databaseConnection();
            string username = User.Text;
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `username` WHERE `user` = @user", conn);

            command.Parameters.Add("@user", MySqlDbType.VarChar).Value = username;
            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e) //ปุ่มยืนยันการสมัคร
        {
            Register();

            



        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) //โชว์ซ่อนรหัสผ่าน
        {
            if (checkBox1.Checked)
            {
                string P = Password.Text;
                Password.PasswordChar = '\0';
                Confirmpassword.PasswordChar = '\0';
            }
            else
            {
                Password.PasswordChar = '•';
                Confirmpassword.PasswordChar = '•';
            }
        }

        private void button2_Click(object sender, EventArgs e) //ไปหน้าแอดมิน
        {
            Form1 a = new Form1();
            this.Hide();
            a.Show();
        }




        private void Phone_KeyPress_1(object sender, KeyPressEventArgs e) //เลขโทรศัพท์
        {
            int cInt = Convert.ToInt32(e.KeyChar);

            if ((int)e.KeyChar >= 48 && (int)e.KeyChar <= 57 || cInt == 8)
            {
                
                e.Handled = false;

            }

            else

            {
                {
                    MessageBox.Show("ใส่ได้เฉพาะตัวเลข");
                }
                e.Handled = true;

            }
        }

        private void User_KeyPress(object sender, KeyPressEventArgs e) //ชื่อผู้ชาย
        {
            if (System.Text.Encoding.UTF8.GetByteCount(new char[] { e.KeyChar }) > 1)

            {
                {
                    MessageBox.Show("ใส่ได้เฉพาะภาษาอังกฤษเละตัวเลข");
                }
                e.Handled = true;
            }
            if ((e.KeyChar == ' '))
            {
                
                e.Handled = true;
            }
        }

        

        private void Password_KeyPress(object sender, KeyPressEventArgs e) //รหัสผ่าน
        {
            if (System.Text.Encoding.UTF8.GetByteCount(new char[] { e.KeyChar }) > 1)
            {
                {
                    MessageBox.Show("ใส่ได้เฉพาะภาษาอังกฤษและตัวเลข");
                }
                e.Handled = true;
            }
            if ((e.KeyChar == ' '))
            {
              
                e.Handled = true;
            }
        }

        private void Confirmpassword_KeyPress(object sender, KeyPressEventArgs e) //ยินยันรหัสผ่าน
        {
            if (System.Text.Encoding.UTF8.GetByteCount(new char[] { e.KeyChar }) > 1)
            {
                {
                    MessageBox.Show("ใส่ได้เฉพาะภาษาอังกฤษและตัวเลข");
                }
                e.Handled = true;
            }
            if ((e.KeyChar == ' '))
            {
                
                e.Handled = true;
            }
        }
 
        }
    }


            
        
   



