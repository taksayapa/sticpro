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
    public partial class Form3 : Form
    {
        private MySqlConnection databaseConnection()
        {
            String connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=sticpro;";

            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        public Form3()
        {
            InitializeComponent();
        }
        private List<forprinthis> allbook = new List<forprinthis>();



        private void ออกจากระบบToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 m = new Form1();
            m.Show();
        }

        private void กลับToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            admin m = new admin();
            m.Show();
        }

        private void พิมพ์ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();


        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Sticker [ pic(ture) + (a)rt ]", new Font("TH SarabunPSK", 32, FontStyle.Bold), Brushes.Black, new PointF(280, 120));
            e.Graphics.DrawString("-------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 18, FontStyle.Bold), Brushes.Black, new PointF(50, 160));
            e.Graphics.DrawString("รายการจำหน่ายสินค้า", new Font("TH SarabunPSK", 18, FontStyle.Bold), Brushes.Black, new PointF(50, 195));
            e.Graphics.DrawString("พิมพ์เมื่อ : \t " + System.DateTime.Now.ToString("dd / MM / yyyy   HH : mm : ss น."), new Font("TH SarabunPSK", 18, FontStyle.Bold), Brushes.Black, new PointF(50, 215));
            e.Graphics.DrawString("-------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 18, FontStyle.Bold), Brushes.Black, new PointF(50, 240));
            e.Graphics.DrawString("ชื่อลูกค้า", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(50, 255));
            e.Graphics.DrawString("สินค้า", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(160, 255));
            e.Graphics.DrawString("ราคา", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(470, 255));
            e.Graphics.DrawString("จำนวน", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(650, 255));
            e.Graphics.DrawString("-------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 18, FontStyle.Bold), Brushes.Black, new PointF(50, 265));
            int y = 290;


            allbook.Clear();
            loaddata();
            loaddata1();
            foreach (var i in allbook)
            {
                e.Graphics.DrawString(i.IDhis, new Font("TH SarabunPSK", 14, FontStyle.Regular), Brushes.Black, new PointF(50, y));
                e.Graphics.DrawString(i.namehis, new Font("TH SarabunPSK", 14, FontStyle.Regular), Brushes.Black, new PointF(160, y));
                e.Graphics.DrawString(i.amounthis, new Font("TH SarabunPSK", 14, FontStyle.Regular), Brushes.Black, new PointF(470, y));
                e.Graphics.DrawString(i.sumhis, new Font("TH SarabunPSK", 14, FontStyle.Regular), Brushes.Black, new PointF(670, y));
                y = y + 20;
            }
            e.Graphics.DrawString("จำนวนสินค้า", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(520, y + 20));
            e.Graphics.DrawString("ยอดรวม", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(520, y + 20 + 20));
            e.Graphics.DrawString(textBoxtotal.Text, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(650, y + 20));
            e.Graphics.DrawString(textBoxsum.Text, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(650, y + 20 + 20));
            e.Graphics.DrawString("ชิ้น", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(750, y + 20));
            e.Graphics.DrawString("บาท", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(750, y + 20 + 20));

        }
        private void loaddata()
        {
            MySqlConnection conn = new MySqlConnection("host=127.0.0.1;username=root;password=;database=sticpro;");

            conn.Open();
            if (textBox4.Text == "") //กรณีที่ไม่ได้ค้นหา collection
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM hisshop", conn);
               
                MySqlDataReader adapter = cmd.ExecuteReader();

                while (adapter.Read()) //loop รับค่าจาก db 
                {
                    Program.IDhis = adapter.GetString("username");
                    Program.namehis = adapter.GetString("collection");
                    Program.amounthis = adapter.GetString("price");
                    Program.sumhis = adapter.GetString("sum");
                    forprinthis item = new forprinthis()
                    {
                        IDhis = Program.IDhis,
                        namehis = Program.namehis,
                        amounthis = Program.amounthis,
                        sumhis = Program.sumhis

                    };
                    allbook.Add(item);
                }
            }
            else
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM hisshop WHERE collection LIKE '%" + textBox4.Text + "%'", conn);
                MySqlDataReader adapter = cmd.ExecuteReader();
                while (adapter.Read())
                {
                    Program.IDhis = adapter.GetString("username");
                    Program.namehis = adapter.GetString("collection");
                    Program.amounthis = adapter.GetString("price");
                    Program.sumhis = adapter.GetString("sum");
                    forprinthis item = new forprinthis()
                    {
                        IDhis = Program.IDhis,
                        namehis = Program.namehis,
                        amounthis = Program.amounthis,
                        sumhis = Program.sumhis

                    };
                    allbook.Add(item);
                }
            }
        }
        private void loaddata1()
        {
            MySqlConnection conn = new MySqlConnection("host=127.0.0.1;username=root;password=;database=sticpro;");

            conn.Open();
            if (textBox1.Text == "") //กรณีที่ไม่ได้ค้นหา collection
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM hisshop", conn);

                MySqlDataReader adapter = cmd.ExecuteReader();

                while (adapter.Read()) //loop รับค่าจาก db 
                {
                    Program.IDhis = adapter.GetString("username");
                    Program.namehis = adapter.GetString("collection");
                    Program.amounthis = adapter.GetString("price");
                    Program.sumhis = adapter.GetString("sum");
                    forprinthis item = new forprinthis()
                    {
                        IDhis = Program.IDhis,
                        namehis = Program.namehis,
                        amounthis = Program.amounthis,
                        sumhis = Program.sumhis

                    };
                    allbook.Add(item);
                }
            }
            else
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM hisshop WHERE collection LIKE '%" + textBox1.Text + "%'", conn);
                MySqlDataReader adapter = cmd.ExecuteReader();
                while (adapter.Read())
                {
                    Program.IDhis = adapter.GetString("username");
                    Program.namehis = adapter.GetString("collection");
                    Program.amounthis = adapter.GetString("price");
                    Program.sumhis = adapter.GetString("sum");
                    forprinthis item = new forprinthis()
                    {
                        IDhis = Program.IDhis,
                        namehis = Program.namehis,
                        amounthis = Program.amounthis,
                        sumhis = Program.sumhis

                    };
                    allbook.Add(item);
                }
            }
        }

        private void showEquipment()
        {
            MySqlConnection conn = databaseConnection();

            DataSet ds = new DataSet();

            conn.Open();
            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM hisshop";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();
            dataGridView1.DataSource = ds.Tables[0].DefaultView;
            sum = 0; //ตัวแปรจำนวนสินค้า
            total = 0; //ตัวแปรยอดรวมจำนวนเงิน
            conn.Open();
            MySqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                sum = sum + int.Parse(read.GetString(4));
                total = total + int.Parse(read.GetString(3));
            }
            textBoxtotal.Text = $"{sum}";
            textBoxsum.Text = $"{total}";
            conn.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            showEquipment();

        }

        int sum, total;

        private void btnreset_Click(object sender, EventArgs e)
        {
            showEquipment();
            textBox4.ResetText();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e) //ออกจากโปรแกรม
        {
            Application.Exit();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            String username = textBox1.Text;
            if (username.Equals("username"))
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            String username = textBox1.Text;
            if (username.Equals("username") || username.Equals(""))
            {
                textBox1.Text = "username";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            String collection = textBox4.Text;
            if (collection.Equals("collection"))
            {
                textBox4.Text = "";
                textBox4.ForeColor = Color.Black;
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            String collection = textBox4.Text;
            if (collection.Equals("collection") || collection.Equals(""))
            {
                textBox4.Text = "collection";
                textBox4.ForeColor = Color.Gray;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = databaseConnection();

            DataSet ds = new DataSet();

            conn.Open();

            MySqlCommand cmd;
            cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM hisshop WHERE username LIKE \"%{textBox1.Text}%\"";
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);


            conn.Close();


            dataGridView1.DataSource = ds.Tables[0].DefaultView;
            sum = 0; //ตัวแปรจำนวนสินค้า
            total = 0; //ตัวแปรยอดรวมจำนวนเงิน
            conn.Open();
            MySqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                sum = sum + int.Parse(read.GetString(4));
                total = total + int.Parse(read.GetString(3));
            }
            textBoxtotal.Text = $"{sum}";
            textBoxsum.Text = $"{total}";
            conn.Close();
        }

        
        private void button4_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = databaseConnection();
           
            DataSet ds = new DataSet();
           
            conn.Open();

            MySqlCommand cmd;
            cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM hisshop WHERE collection LIKE \"%{textBox4.Text}%\"";
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);
            
           
            conn.Close();


            dataGridView1.DataSource = ds.Tables[0].DefaultView;
            sum = 0; //ตัวแปรจำนวนสินค้า
            total = 0; //ตัวแปรยอดรวมจำนวนเงิน
            conn.Open();
            MySqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                sum = sum + int.Parse(read.GetString(4));
                total = total + int.Parse(read.GetString(3));
            }
            textBoxtotal.Text = $"{sum}";
            textBoxsum.Text = $"{total}";
            conn.Close();

           

        }
    }
}
