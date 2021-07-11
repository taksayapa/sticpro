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
using System.IO;
namespace sticpro
{
    public partial class admin : Form
    {
        private MySqlConnection databaseConnection()
        {
            String connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=sticpro;";

            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        public admin()
        {
            InitializeComponent();
        }
        private void showEquipment() //โชว์ข้อมูล
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

        private void ประวตการขายToolStripMenuItem_Click(object sender, EventArgs e)  //ไปหน้าประวัติการขาย
        {
            this.Hide();
            Form3 m = new Form3();
            m.Show();


        }

        private void ขอมลToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            MySqlConnection conn = databaseConnection();
            DataSet ds = new DataSet();

            conn.Open();

            MySqlCommand cmd;
            cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM shop ";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();
            dataGridView1.DataSource = ds.Tables[0].DefaultView;

            
        }

        

      

        private void ออกจากระบบToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 m = new Form1();
            m.Show();
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            String collection = textBox4.Text;
            if (collection.Equals("Name Collection"))
            {
                textBox4.Text = "";
                textBox4.ForeColor = Color.Black;
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            String collection = textBox4.Text;
            if (collection.Equals("Name Collection") || collection.Equals(""))
            {
                textBox4.Text = "Name Collection";
                textBox4.ForeColor = Color.Gray;
            }
        }

        private void button1_Click(object sender, EventArgs e) //เพิ่มข้อมูล
        {
            string connection = "datasource=127.0.0.1;port=3306;username=root;password=;database=sticpro;";
            MySqlConnection conn = new MySqlConnection(connection);
            byte[] picture = null;
            string filepath = textBox6.Text;
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            picture = br.ReadBytes((int)fs.Length);
            string sql = "INSERT INTO shop (collection,price,amount,description,picture) VALUES('"+textBox1.Text+"','"+textBox2.Text+"','"+textBox5.Text+"','"+textBox3.Text+"',@picture)";
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
                MySqlCommand cmd1 = new MySqlCommand(sql, conn);
                cmd1.Parameters.Add(new MySqlParameter("@picture", picture));
                int x = cmd1.ExecuteNonQuery();
                conn.Close();
                
                showEquipment();
            }
           
        }

        private void button2_Click(object sender, EventArgs e) //ลบข้อมูล
        {

            int selectedRow = dataGridView1.CurrentCell.RowIndex;
            int deleteId = Convert.ToInt32(dataGridView1.Rows[selectedRow].Cells["id"].Value);

            MySqlConnection conn = databaseConnection();
            string sql = "DELETE FROM shop WHERE id = '" + deleteId + "'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            conn.Close();

            if (rows > 0)
            {
                MessageBox.Show("ลบข้อมูลสำเร็จ");
                showEquipment();
            }
        }

        

        private void button3_Click(object sender, EventArgs e) //แก้ไขข้อมูล
        {
            int selectedRow = dataGridView1.CurrentCell.RowIndex;
            int editId = Convert.ToInt32(dataGridView1.Rows[selectedRow].Cells["id"].Value);
            string connection = "datasource=127.0.0.1;port=3306;username=root;password=;database=sticpro;";
            MySqlConnection conn = new MySqlConnection(connection);
            byte[] picture = null;
            string filepath = textBox6.Text;
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            picture = br.ReadBytes((int)fs.Length);
            string sql = "UPDATE shop SET collection ='" + textBox1.Text + "' , amount = '" + textBox5.Text + "' , price = '" + textBox2.Text + "', id = '" + textBox7.Text + "'  , description = '" + textBox3.Text + "' ,picture =@picture WHERE id = '" + editId + "' ";
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
                MySqlCommand cmd1 = new MySqlCommand(sql, conn);
                cmd1.Parameters.Add(new MySqlParameter("@picture", picture));
                int x = cmd1.ExecuteNonQuery();
                conn.Close();

                showEquipment();
            }


            
        }

        private void admin_Load(object sender, EventArgs e)
        {
            showEquipment();
            comboBox1.Text = "ประเภทสินค้า";
        }

        private void button4_Click(object sender, EventArgs e) //ค้นหา
        {
            MySqlConnection conn = databaseConnection();

            DataSet ds = new DataSet();

            conn.Open();
            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM shop WHERE `description` like '%" +textBox4.Text+"%'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();

            dataGridView1.DataSource = ds.Tables[0].DefaultView;
            

        }

       
        private void button5_Click(object sender, EventArgs e) //อัปโหลดรูปภาพ
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg;*.jpeg;*.gif;*.bmp;*png;)|*.jpg;*.jpeg;*.gif;*.bmp;*.png;";
            if (open.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image = new Bitmap(open.FileName);
                textBox6.Text = open.FileName;
            }
        }

        private void button6_Click(object sender, EventArgs e) //รีเซตข้อมูล
        {
            showEquipment();
            comboBox1.ResetText();
            textBox4.ResetText();
            textBox1.ResetText();
            textBox2.ResetText();
            textBox3.ResetText();
            textBox5.ResetText();
            textBox6.ResetText();
            textBox7.ResetText();
            comboBox1.Text = "ประเภทสินค้า";

        }

        private void pictureBox1_Click(object sender, EventArgs e) 
        {

        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)//กดแล้วให้ข้อมูลตรงดาต้ากริชวิวไปแสดงที่textbox
        {
            dataGridView1.CurrentRow.Selected = true;
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["collection"].FormattedValue.ToString();
            textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells["amount"].FormattedValue.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["price"].FormattedValue.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["description"].FormattedValue.ToString();
            textBox7.Text = dataGridView1.Rows[e.RowIndex].Cells["id"].FormattedValue.ToString();


            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=sticpro;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"SELECT picture FROM shop WHERE id =\"{ textBox7.Text}\"", conn);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                MemoryStream ms = new MemoryStream((byte[])ds.Tables[0].Rows[0]["picture"]);
                pictureBox2.Image = new Bitmap(ms);
            }
        }

        private void combotype()
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

        private void ขอมลสนคาToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            stock m = new stock();
            m.Show();
        }
    }

    
    
}
