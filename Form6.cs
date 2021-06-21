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
    public partial class Form6 : Form

    {
       
        public Form6()
        {
            InitializeComponent();
          //  textBox4.Text = Program.finalPrice.ToString();
            //richTextBox1.Text = Program.orderfood;
           // richTextBox2.Text = Program.Price;
        }


        
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                double receive = double.Parse(textBox5.Text);
                double moneyy;
                double mm = double.Parse(textBox4.Text);
                moneyy = receive - mm;
                MessageBox.Show("เงินทอน" +  moneyy  + "บาท");
               
                

            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
       
        private void Form6_Load(object sender, EventArgs e)
        {
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            Form2 f = new Form2();
         
            ShowDialog();
        }
       
    }
}
