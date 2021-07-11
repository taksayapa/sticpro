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
    public partial class Form1 : Form
    {
        private MySqlConnection databaseConnection()
        {
            String connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=sticpro;";

            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            MySqlConnection conn = databaseConnection();
            conn.Open();


            MySqlCommand cmd;
            cmd = conn.CreateCommand();
            string user = textBox1.Text;
            string pass = textBox2.Text;
            cmd.CommandText = $"SELECT * FROM account WHERE username=\"{user}\" AND password=\"{pass}\"";
            

            MySqlDataReader adapter = cmd.ExecuteReader();


            if (adapter.HasRows)
            {
                Shop m = new Shop();
                m.textBox3.Text = textBox1.Text;
                m.Show();
                
            }
            else if (textBox1.Text == "admin" && textBox2.Text == "ad123")
            {
                admin m = new admin();
                m.Show();
            }
            else
            {
                MessageBox.Show("กรุณากรอกบัญชีผู้ใช้หรือรหัสผ่านอีกครั้ง");
            }
            conn.Close();
            {
                this.Hide();
                Form1 m = new Form1();
                
            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 m = new Form2();
            m.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = true;
            }
            else
            {
                textBox2.UseSystemPasswordChar = false;
            }
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }
    }
}
