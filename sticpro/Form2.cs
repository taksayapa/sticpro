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


namespace sticpro
{
    public partial class Form2 : Form
    {
        private MySqlConnection databaseConnection()
        {
            String connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=sticpro;";

            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }

        public Form2()
        {
            InitializeComponent();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            String firstname = textBox1.Text;
            if (firstname.Equals("Fill out the information in Thai or English"))
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            String firstname = textBox1.Text;
            if (firstname.Equals("Fill out the information in Thai or English") || firstname.Equals(""))
            {
                textBox1.Text = "Fill out the information in Thai or English";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            String lastname = textBox2.Text;
            if (lastname.Equals("Fill out the information in Thai or English"))
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }

        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            String lastname = textBox2.Text;
            if (lastname.Equals("Fill out the information in Thai or English") || lastname.Equals(""))
            {
                textBox2.Text = "Fill out the information in Thai or English";
                textBox2.ForeColor = Color.Gray;
            }
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            String password = textBox4.Text;
            if (password.Equals("Fill out the information in English or Number"))
            {
                textBox4.Text = "";
                textBox4.ForeColor = Color.Black;
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {

            String password = textBox4.Text;
            if (password.Equals("Fill out the information in English or Number") || password.Equals(""))
            {
                textBox4.Text = "Fill out the information in English or Number";
                textBox4.ForeColor = Color.Gray;
            }
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            String username = textBox5.Text;
            if (username.Equals("Fill out the information in English or Number"))
            {
                textBox5.Text = "";
                textBox5.ForeColor = Color.Black;
            }
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            String username = textBox5.Text;
            if (username.Equals("Fill out the information in English or Number") || username.Equals(""))
            {
                textBox5.Text = "Fill out the information in English or Number";
                textBox5.ForeColor = Color.Gray;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
               
                MySqlConnection conn = databaseConnection();

                string sql = "INSERT INTO account(firstname,lastname,username,password) VALUES('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox5.Text + "','" + textBox4.Text + "')";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();
                if (rows > 0)
                {
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox5.Text = "";
                    textBox4.Text = "";
                    MessageBox.Show("สมัครสมาชิกสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Form1 a = new Form1();
                    this.Hide();
                    a.Show();
                }
            
        }
        

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (System.Text.Encoding.UTF8.GetByteCount(new char[] { e.KeyChar })>1)
            {
                e.Handled = true;
                MessageBox.Show("กรุณาตรวจสอบอักขระ", "ผลการตรวจสอบ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (System.Text.Encoding.UTF8.GetByteCount(new char[] { e.KeyChar }) >1)
            {
                e.Handled = true;
                MessageBox.Show("กรุณาตรวจสอบอักขระ", "ผลการตรวจสอบ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar) != true && Char.IsNumber(e.KeyChar) == true && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("กรุณาตรวจสอบอักขระ", "ผลการตรวจสอบ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar) != true && Char.IsNumber(e.KeyChar) == true && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("กรุณาตรวจสอบอักขระ", "ผลการตรวจสอบ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
