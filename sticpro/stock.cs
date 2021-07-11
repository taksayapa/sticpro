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
    public partial class stock : Form
    {
        private MySqlConnection databaseConnection()
        {
            String connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=sticpro;";

            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }

        public stock()
        {
            InitializeComponent();
        }
        private List<forprintstock> allbook = new List<forprintstock>();

        private void กลบToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            admin m = new admin();
            m.Show();
        }

        private void ออกจากระบบToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 m = new Form1();
            m.Show();
        }

        private void พมพToolStripMenuItem_Click(object sender, EventArgs e)
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
            e.Graphics.DrawString("ไอดี", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(50, 255));
            e.Graphics.DrawString("สินค้า", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(160, 255));
            e.Graphics.DrawString("ราคา", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(470, 255));
            e.Graphics.DrawString("จำนวนคงเหลือ", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(650, 255));
            e.Graphics.DrawString("-------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 18, FontStyle.Bold), Brushes.Black, new PointF(50, 265));
            int y = 290;

            allbook.Clear();
            loaddata();
            foreach (var i in allbook)
            {
                e.Graphics.DrawString(i.IDstock, new Font("TH SarabunPSK", 14, FontStyle.Regular), Brushes.Black, new PointF(50, y));
                e.Graphics.DrawString(i.namestock, new Font("TH SarabunPSK", 14, FontStyle.Regular), Brushes.Black, new PointF(160, y));
                e.Graphics.DrawString(i.pricestock, new Font("TH SarabunPSK", 14, FontStyle.Regular), Brushes.Black, new PointF(470, y));
                e.Graphics.DrawString(i.amountstock, new Font("TH SarabunPSK", 14, FontStyle.Regular), Brushes.Black, new PointF(670, y));
                y = y + 20;
            }
            e.Graphics.DrawString("สินค้า", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(520, y + 20));
            e.Graphics.DrawString("ราคา", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(520, y + 20 + 20));
            e.Graphics.DrawString("จำนวนสินค้าคงเหลือ", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(520, y + 20 + 20 + 20));
            e.Graphics.DrawString(textBox2.Text, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(650, y + 20));
            e.Graphics.DrawString(textBox3.Text, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(650, y + 20 + 20));
            e.Graphics.DrawString(textBox4.Text, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(650, y + 20));
            e.Graphics.DrawString("", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(750, y + 20));
            e.Graphics.DrawString("บาท", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(750, y + 20 + 20));
            e.Graphics.DrawString("ชิ้น", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(750, y + 20 + 20 + 20));
        }
        private void loaddata()
        {
            MySqlConnection conn = new MySqlConnection("host=127.0.0.1;username=root;password=;database=sticpro;");

            conn.Open();
            if (textBox4.Text == "") //กรณีที่ไม่ได้ค้นหา collection
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM shop", conn);

                MySqlDataReader adapter = cmd.ExecuteReader();

                while (adapter.Read()) //loop รับค่าจากฟิวของ db 
                {
                    Program.IDstock = adapter.GetString("id");
                    Program.namestock = adapter.GetString("collection");
                    Program.pricestock = adapter.GetString("price");
                    Program.amountstock = adapter.GetString("amount");
                    forprintstock item = new forprintstock()
                    {
                        IDstock = Program.IDstock,
                        namestock = Program.namestock,
                        pricestock = Program.pricestock,
                        amountstock = Program.amountstock

                    };
                    allbook.Add(item);
                }
            }
            else
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM shop WHERE collection LIKE '%" + textBox1.Text + "%'", conn);
                MySqlDataReader adapter = cmd.ExecuteReader();
                while (adapter.Read())
                {
                    Program.IDstock= adapter.GetString("id");
                    Program.namestock = adapter.GetString("collection");
                    Program.pricestock = adapter.GetString("price");
                    Program.amountstock = adapter.GetString("amount");
                    forprintstock item = new forprintstock()
                    {
                        IDstock = Program.IDstock,
                        namestock= Program.namestock,
                        pricestock = Program.pricestock, 
                        amountstock = Program.amountstock

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
            cmd.CommandText = "SELECT * FROM shop";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();
            dataGridView1.DataSource = ds.Tables[0].DefaultView;
           
        }

        private void stock_Load(object sender, EventArgs e)
        {
            showEquipment();
            comboBox1.Text = "ประเภทสินค้า";
        }

        private void button2_Click(object sender, EventArgs e) //กดแล้วรีเซ็ตข้อมูลให้เป็นช่องว่าง
        {
            showEquipment();
            comboBox1.ResetText();
            textBox5.ResetText();
            textBox4.ResetText();
            textBox3.ResetText();
            textBox1.ResetText();
            textBox2.ResetText();
            comboBox1.Text = "ประเภทสินค้า";
        }

        private void stock_FormClosing(object sender, FormClosingEventArgs e) //ออกจากฟอมให้ปิดโปรแกรม
        {
            Application.Exit();
        }
        int price, amount;

        private void combotype() //เลือกcombobox แล้วโชว์ตาม ประเภท type
        {
            MySqlConnection conn = databaseConnection();

            DataSet ds = new DataSet();

            conn.Open();
            MySqlCommand cmd;
            string type = comboBox1.Text;
            cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM shop WHERE collection =\"{type}\"";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();
            dataGridView1.DataSource = ds.Tables[0].DefaultView;


        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            combotype();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) //กดแล้วให้ข้อมูลตรงดาต้ากริชวิวไปแสดงที่textbox
        {
            dataGridView1.CurrentRow.Selected = true;
            textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells["id"].FormattedValue.ToString();
            textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["amount"].FormattedValue.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["price"].FormattedValue.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["description"].FormattedValue.ToString();
           

        }

        private void button1_Click(object sender, EventArgs e) //ค้นหา
        {
            
            MySqlConnection conn = databaseConnection();

            DataSet ds = new DataSet();

            conn.Open();

            MySqlCommand cmd;
            cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM shop WHERE collection LIKE \"%{textBox1.Text}%\"";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();
            
            dataGridView1.DataSource = ds.Tables[0].DefaultView;
            price = 0; //ตัวแปรจำนวนสินค้า
            amount = 0; //ตัวแปรยอดรวมจำนวนเงิน
            
            conn.Open();
            MySqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                price = price + int.Parse(read.GetString(2));
                amount = amount + int.Parse(read.GetString(4));
            }
            textBox3.Text = $"{ price}";
            textBox4.Text = $"{amount}";
            conn.Close();
        }
    }
}
