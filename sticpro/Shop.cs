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
    public partial class Shop : Form
    {
        class ForPrint //เก็บค่าจาก ดาต้าเบส เพื่อไปปริ้น
        {
            public string id { get; set; }
            public string collection { get; set; }
            public string price { get; set; }
            public string amount { get; set; }

        }
        List<Classstock> allstock = new List<Classstock>(); // ลิสสต๊อกทั้งหมด
        List<Classstockmain> movedata = new List<Classstockmain>(); //list move history
        string idFormcart;
        string nameFormcart;
        string sumFormcart;
        string amountFormcart;
        string amountfromDB;
        string amountnew;
        Boolean dtstock;
        private List<ForPrint> allbook = new List<ForPrint>(); /*listใบเสร็จ*/


        private MySqlConnection databaseConnection()
        {
            String connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=sticpro;";

            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        //public int P1 { get; set; }
        //public string stk { get; set; }

        int check_1, check_2, sum_old;
        public int sum = 0;
        public Shop()
        {
            InitializeComponent();
        }
        //public void combokind()
        //{
        //    MySqlConnection conn = databaseConnection();

        //    DataSet ds = new DataSet();

        //    conn.Open();
        //    MySqlCommand cmd;

        //    cmd = conn.CreateCommand();
        //    cmd.CommandText = $"SELECT * FROM shop WHERE kind =\"{comboBox1.SelectedItem.ToString()}\"";

        //    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
        //    adapter.Fill(ds);

        //    conn.Close();
        //    dataShop.DataSource = ds.Tables[0].DefaultView;
        //}

        private void Showdataproduct() //เรียกข้อมูลสินค้ามาแสดงที่ dt gv
        {
            MySqlConnection conn = databaseConnection(); //เพื่อคอนเน เพื่อเปิดดาต้าเบส แต่ใช้ตัวแปรเป็นconn

            DataSet ds = new DataSet();

            conn.Open();

            MySqlCommand cmd; //เพื่อเชื่อมกับดาต้าเบส conn

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM shop";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd); //ประกาศcmd มาใช้
            adapter.Fill(ds);

            conn.Close();

            dataShop.DataSource = ds.Tables[0].DefaultView; //แสดงในดาต้ากริชวิว
        }
        private void Showdatacart() //ตะกร้าสินค้า
        {
            MySqlConnection conn = databaseConnection();

            DataSet ds = new DataSet();

            conn.Open();

            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM `order`";


            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();

            dataGridView1.DataSource = ds.Tables[0].DefaultView;
        }
        //private void showEquipment()
        //{
        //    MySqlConnection conn = databaseConnection();

        //    DataSet ds = new DataSet();

        //    conn.Open();
        //    MySqlCommand cmd;

        //    cmd = conn.CreateCommand();
        //    cmd.CommandText = "SELECT * FROM shop";

        //    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
        //    adapter.Fill(ds);

        //    conn.Close();
        //    dataShop.DataSource = ds.Tables[0].DefaultView;
        //}

        private void Shop_Load(object sender, EventArgs e) //ให้มันโชว์ตาราง คอมโบที่ต้องการ
        {
            show();
            show_order();
            comboBox1.Text = "ประเภทสินค้า";

        }




        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            combotype();
        }
        private void show() //โชว์ตาราง
        {
            MySqlConnection conn = databaseConnection();

            DataSet ds = new DataSet();

            conn.Open();
            MySqlCommand cmd;
            string type = comboBox1.Text;
            cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM shop";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();
            dataShop.DataSource = ds.Tables[0].DefaultView;
            dataShop.ReadOnly = true;


        }
        private void combotype() //ให้คอมโบเสริชจากที่เราต้องการ
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
            dataShop.DataSource = ds.Tables[0].DefaultView;


        }

        private void ออกจากระบบToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                this.Hide();
                Form1 m = new Form1();
                m.Show();
            }
        }

        
        int check, amountold; //ประกาศตัวแปร เพื่อเรียกใช้ในคำสั่งข้างใน
        private void button1_Click(object sender, EventArgs e)
        {
            if (dtstock == true)
            {
                MySqlConnection conn = databaseConnection();
                conn.Open();
                MySqlCommand cmd;
                cmd = conn.CreateCommand();
                cmd.CommandText = $"SELECT *FROM shop WHERE id='{txtid.Text}'";
                MySqlDataReader row = cmd.ExecuteReader();
                if (row.HasRows)
                {
                    while (row.Read())
                    {
                        check = int.Parse(row.GetString(4)); //จำนวนในสต๊อก
                        sum_old = int.Parse(row.GetString(2)); //sum_old เก็บ sum ไว้คำนวณราคารวม
                    }
                }
                conn.Close();
                if (check >= int.Parse(comboamount.Text))
                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = $"SELECT *FROM `order` WHERE id='{txtid.Text}'"; //เช็คว่ามีสินค้าเดิมในตะกร้ามั้ย  เพื่อบวกจำนวนสินค้า
                    row = cmd.ExecuteReader();
                    if (row.HasRows)
                    {
                        while (row.Read())
                        {
                            amountold = int.Parse(row.GetString(3)); //จำนวนในตะกร้า

                        }
                        amountnew = (amountold + int.Parse(comboamount.Text)).ToString(); //จำนวนสินค้าในตะกร้า+จำนวนสินค้าที่เราเพิ่มเข้าไป
                        MySqlConnection conn1 = databaseConnection();
                        conn1.Open();
                        string sql = "UPDATE `order` SET amount = '" + amountnew + "',sum = '" + int.Parse(amountnew) * sum_old + "' WHERE ID = '" + txtid.Text + "'";
                        MySqlCommand command1 = new MySqlCommand(sql, conn1);
                        command1.ExecuteReader();
                        conn1.Close();

                        conn1.Open();
                        sql = "UPDATE shop SET amount = '" + (check - int.Parse(comboamount.Text)) + "' WHERE id = '" + txtid.Text + "'";
                        command1 = new MySqlCommand(sql, conn1);
                        command1.ExecuteReader();
                        conn1.Close();
                        MessageBox.Show("เพิ่มข้อมูลสำเร็จ", "ระบบ");
                        Showdataproduct();
                        Showdatacart();
                        showprice();
                    }
                    else
                    {
                        MySqlConnection conn1 = databaseConnection();
                        string sql = "INSERT INTO `order` (id, collection, price, amount,sum,description) VALUES('" + txtid.Text + "' ,'" + txtcoll.Text + "' , '" + txtprice.Text + "','" + comboamount.Text + "','" + (int.Parse(comboamount.Text) * int.Parse(txtprice.Text)).ToString() + "','"+txtdes.Text+"')";
                        conn1.Open();
                        cmd = new MySqlCommand(sql, conn1);
                        cmd.ExecuteReader();
                        conn1.Close();
                        //conn1.Open();
                        //sql = "SELECT * FROM `order` WHERE id = '" + txtid.Text + "'";
                        //MySqlCommand command = new MySqlCommand(sql, conn1);
                        //MySqlDataReader readdata = command.ExecuteReader();
                        //while (readdata.Read())
                        //{
                        //    amountfromDB = readdata.GetString("amount").ToString();
                        //}
                        //conn1.Close();

                        amountnew = (check - int.Parse(comboamount.Text)).ToString();

                        conn1.Open();
                        MessageBox.Show("เพิ่มสินค้าสำเร็จ", "แจ้งเตือน");

                        sql = "UPDATE shop SET amount = '" + amountnew + "' WHERE id = '" + txtid.Text + "'";
                        MySqlCommand command1 = new MySqlCommand(sql, conn1);
                        command1.ExecuteReader();
                        conn1.Close();

                        Showdataproduct();
                        Showdatacart();
                        showprice();
                    }
                    conn.Close();
                }
                else
                {
                    MessageBox.Show("ขออภัยค่ะ จำนวนสินค้าในคลังไม่เพียงพอ", "แจ้งเตือน");
                }
            }
        

            //MySqlConnection conn = databaseConnection();
            //string sql = "INSERT INTO order (collection,price,amount) VALUES('" + txtcoll.Text + "' , '" + txtprice.Text + "','"+txtamount.Text+"')";
            //MySqlCommand cmd = new MySqlCommand(sql, conn);

            //conn.Open();
            //int rows = cmd.ExecuteNonQuery();

            //conn.Close();

            //if (rows > 0)
            //{
            //    MessageBox.Show("เพิ่มข้อมูลสำเร็จ");
            //    showEquipment();
            //}
            //if (label3.Text == "Sticker")
            //{
            //    Shop obj = comboBox1.SelectedItem as Shop;
            //    if (obj != null)
            //    {
            //        richTextBox1.Text += obj.stk + Environment.NewLine;
            //        richTextBox2.Text += Convert.ToString(obj.P1) + Environment.NewLine;

            //        sum = sum + obj.P1;
            //    }
            //}
            //textBox1.Text = Convert.ToString(sum);
        }

        private void showprice() //ยอดรวม
        {
            sum = 0;
            MySqlConnection conn = databaseConnection();
            string sql = "SELECT * FROM `order`";
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                sum = sum + int.Parse(read.GetString("sum").ToString());
            }
            textBox1.Text = sum.ToString();
            conn.Close();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox4.Text=(int.Parse(textBox2.Text) - int.Parse(textBox1.Text)).ToString();
            MySqlConnection conn = databaseConnection();
            string sql = "INSERT INTO hisshop (username,collection,price,sum,getmoney,moneyc) VALUES('" + textBox3.Text + "' , '" + txtcoll.Text + "','" + textBox1.Text + "','" + comboamount.Text + "','" + textBox2.Text + "','" + textBox4.Text + "')";
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            conn.Open();
            int rows = cmd.ExecuteNonQuery();

            conn.Close();

            if (rows > 0)
            {

            }

            conn.Close();

            double receive = double.Parse(textBox2.Text); //รับค่ามาเพื่อ 
            if (receive >= sum)
            {
                textBox4.Text = Convert.ToString(receive - sum);
                MessageBox.Show("ชำระเงินเรียบร้อย", "การชำระเงิน", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else if (receive < sum)
            {
                MessageBox.Show("จำนวนเงินของคุณไม่เพียงพอ", "ระบบ");
            }

            printPreviewDialog1.Document = printDocument1; //ปริิ้นใบเสร็จอัตโนมัติ
            printPreviewDialog1.ShowDialog();

            MySqlConnection conn1 = databaseConnection();
            conn1.Open();

            string sqlDelete = "DELETE FROM `order`"; //ถ้าเรากดยืนยันแล้วก็จะไปลบข้อมูลในดาต้า ทำให้เป็นว่างเปล่า
            MySqlCommand commandDelete = new MySqlCommand(sqlDelete, conn1);
            commandDelete.ExecuteReader();

            conn1.Close();
            Showdatacart();
            textBox1.Text = "0"; //ยอดรวม
            textBox4.Text = "0"; //เงินทอน
            textBox2.Text = "0"; // เงินที่ได้รับ
            Showdataproduct();
            MessageBox.Show("ขอบคุณที่ใช้บริการค่ะ ", "แจ้งเตือน");

        }

       
        
        private void button2_Click(object sender, EventArgs e) //แก้ไข
        {
            if (dtstock == false)
            {
                MySqlConnection conn = databaseConnection();
                conn.Open();
                MySqlCommand cmd;
                cmd = conn.CreateCommand();
                cmd.CommandText = $"SELECT *FROM shop WHERE id='{txtid.Text}'";
                MySqlDataReader row = cmd.ExecuteReader();
                if (row.HasRows)
                {
                    while (row.Read())
                    {
                        check_1 = int.Parse(row.GetString(4)); //จำนวนในสต๊อก
                        sum_old = int.Parse(row.GetString(2));
                    }
                }
                conn.Close();

                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = $"SELECT *FROM `order` WHERE id='{txtid.Text}'";
                row = cmd.ExecuteReader();
                if (row.HasRows)
                {
                    while (row.Read())
                    {
                        check_2 = int.Parse(row.GetString(3)); //จำนวนในตะกร้า
                    }
                }
                conn.Close();

                if (check_1 >= int.Parse(comboamount.Text)) //จำนวนในสต๊อกมากกว่าตำนวนที่เลือก
                {
                    if (int.Parse(comboamount.Text) > check_2)  //จำนวนที่เราเลือกเพิ่มมากว่าจำนวนที่อยู่ในตะกร้า //แก้ไขเพิ่มจำนวนมากกว่าเดิม
                    {
                        conn.Open();
                        string sql = "UPDATE `order` SET amount = '" + comboamount.Text + "', sum = '" + sum_old * int.Parse(comboamount.Text) + "' WHERE ID = '" + txtid.Text + "'";
                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.ExecuteReader();
                        conn.Close();

                        string amountstock = (check_1 - (int.Parse(comboamount.Text) - check_2)).ToString(); //จำนวนสินค้าในสต๊อกที่เหลืออยู่หลังจากที่เราเพิ่มสินค้าเข้าไปในตะกร้า
                        conn.Open();
                        sql = "UPDATE shop SET amount = '" + amountstock + "' WHERE ID = '" + txtid.Text + "'";
                        command = new MySqlCommand(sql, conn);
                        command.ExecuteReader();
                        conn.Close();
                    }
                    else
                    {
                        conn.Open(); //แก้ไขจำนวนในตะกร้าโดยลดลงกว่าเดิม
                        string sql = "UPDATE `order` SET amount = '" + comboamount.Text + "', sum = '" + sum_old * int.Parse(comboamount.Text) + "' WHERE id = '" + txtid.Text + "'";
                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.ExecuteReader();
                        conn.Close();

                        string amountstock = ((check_2 - int.Parse(comboamount.Text)) + check_1).ToString(); //เอาจำนวนที่เราเอาออกจากตะกร้ามาบวกเข้ากับจำนวนเดิมในคลัง
                        conn.Open();
                        sql = "UPDATE shop SET amount = '" + amountstock + "' WHERE id = '" + txtid.Text + "'";
                        command = new MySqlCommand(sql, conn);
                        command.ExecuteReader();
                        conn.Close();
                    }
                    MessageBox.Show("แก้ข้อมูลสำเร็จ", "แจ้งเตือน");
                    Showdataproduct();
                    Showdatacart();
                    showprice();
                }
                else
                {
                    MessageBox.Show("โปรดตรวจสอบจำนวนสินค้า", "แจ้งเตือน");
                }
            }

            //if (label3.Text == "Sticker")
            //{
            //    Shop obj = comboBox1.SelectedItem as Shop;
            //    if (obj != null)
            //    {

            //        richTextBox1.Clear();
            //        richTextBox2.Clear();
            //        textBox2.Text = String.Empty;
            //        textBox4.Text = String.Empty;
            //        sum = 0;
            //    }
            //}
            //textBox1.Text = Convert.ToString(sum);
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            e.Graphics.DrawString("Sticker [ pic(ture) + (a)rt ]", new Font("TH SarabunPSK", 32, FontStyle.Bold), Brushes.Black, new PointF(280, 120));
            e.Graphics.DrawString("------------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 18, FontStyle.Bold), Brushes.Black, new PointF(40, 160));
            e.Graphics.DrawString("รายการจำหน่ายสินค้า", new Font("TH SarabunPSK", 18, FontStyle.Bold), Brushes.Black, new PointF(50, 195));
            e.Graphics.DrawString("พิมพ์เมื่อ : \t " + System.DateTime.Now.ToString("dd / MM / yyyy   HH : mm : ss น."), new Font("TH SarabunPSK", 18, FontStyle.Bold), Brushes.Black, new PointF(50, 215));
            e.Graphics.DrawString("-----------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 18, FontStyle.Bold), Brushes.Black, new PointF(50, 240));
            e.Graphics.DrawString("รหัสสินค้า", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(50, 255));
            e.Graphics.DrawString("รายการ", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(160, 255));
            e.Graphics.DrawString("ราคา", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(470, 255));
            e.Graphics.DrawString("จำนวน", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(670, 255));
            e.Graphics.DrawString("-----------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 18, FontStyle.Bold), Brushes.Black, new PointF(50, 265));
            int y = 290;

            allbook.Clear();
            loaddata();
            foreach (var i in allbook)
            {
                e.Graphics.DrawString(i.id, new Font("TH SarabunPSK", 14, FontStyle.Regular), Brushes.Black, new PointF(50, y));
                e.Graphics.DrawString(i.collection, new Font("TH SarabunPSK", 14, FontStyle.Regular), Brushes.Black, new PointF(160, y));
                e.Graphics.DrawString(i.price, new Font("TH SarabunPSK", 14, FontStyle.Regular), Brushes.Black, new PointF(470, y));
                e.Graphics.DrawString(i.amount, new Font("TH SarabunPSK", 14, FontStyle.Regular), Brushes.Black, new PointF(670, y));
                y = y + 20;
            }
            e.Graphics.DrawString("ราคารวม", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(570, y));
            e.Graphics.DrawString("เงินที่ได้รับ", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(570, y + 20));
            e.Graphics.DrawString("เงินทอน", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(570, y + 20 + 20));
            e.Graphics.DrawString(textBox1.Text + " บาท", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(650, y));
            e.Graphics.DrawString(textBox2.Text + " บาท", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(650, y + 20));
            e.Graphics.DrawString(textBox4.Text + " บาท", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(650, y + 20 + 20));
        }
        private void loaddata() //ดึงข้อมูลเพื่อไปแสดงในใบปริ้น
        {
            MySqlConnection conn = new MySqlConnection("host=127.0.0.1;username=root;password=;database=sticpro;");

            conn.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM `order`", conn);
            MySqlDataReader adapter = cmd.ExecuteReader();

            while (adapter.Read())
            {
                Program.id = adapter.GetString("id");
                Program.collection = adapter.GetString("collection");
                Program.price = adapter.GetString("price");
                Program.amount = adapter.GetString("amount");
                ForPrint item = new ForPrint()
                {
                    id = Program.id,
                    collection = Program.collection,
                    price = Program.price,
                    amount = Program.amount

                };
                allbook.Add(item);
            }

            
        }




        //private void dataShop_CellClick(object sender, DataGridViewCellEventArgs e)
        //{

        //        dataShop.CurrentRow.Selected = true;
        //        txtcoll.Text = dataShop.Rows[e.RowIndex].Cells["collection"].FormattedValue.ToString();
        //        textBox5.Text = dataShop.Rows[e.RowIndex].Cells["amount"].FormattedValue.ToString();
        //        txtprice.Text = dataShop.Rows[e.RowIndex].Cells["price"].FormattedValue.ToString();
        //        txtdes.Text = dataShop.Rows[e.RowIndex].Cells["description"].FormattedValue.ToString();
        //        txtid.Text = dataShop.Rows[e.RowIndex].Cells["id"].FormattedValue.ToString();

        //        //byte[] pic;
        //        //pic = (byte[])dataShop.CurrentRow.Cells[6].Value;
        //        //MemoryStream ms = new MemoryStream(pic);
        //        //pictureBox2.Image = Image.FromStream(ms);
        //}

        //private void button6_Click(object sender, EventArgs e)
        //{
        //        MySqlConnection conn = databaseConnection();

        //        DataSet ds = new DataSet();

        //        conn.Open();
        //        MySqlCommand cmd;

        //        cmd = conn.CreateCommand();
        //        cmd.CommandText = "SELECT * FROM shop";

        //        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
        //        adapter.Fill(ds);

        //        conn.Close();
        //        dataShop.DataSource = ds.Tables[0].DefaultView;
        //        comboBox1.Text = "ประเภทสินค้า";
        //}

        //private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{

        //}

        int dltamount;
        private void button5_Click(object sender, EventArgs e) //ลบสินค้า
        {
            if (dtstock == false)
            {
                int selectedRow = dataGridView1.CurrentCell.RowIndex;
                int deletepro = Convert.ToInt32(dataGridView1.Rows[selectedRow].Cells["id"].Value);
                dltamount = Convert.ToInt32(dataGridView1.Rows[selectedRow].Cells["amount"].Value);

                MySqlConnection conn = databaseConnection();
                string sql = "DELETE FROM `order` WHERE id = '" + deletepro + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                conn.Open();

                int rows = cmd.ExecuteNonQuery();

                conn.Close();
                if (rows > 0)
                {
                    conn.Open();
                    string sqlcom = "SELECT * FROM shop WHERE id = '" + deletepro + "'";
                    MySqlCommand command = new MySqlCommand(sqlcom, conn);
                    MySqlDataReader readdata = command.ExecuteReader();
                    while (readdata.Read())
                    {
                        amountfromDB = readdata.GetString("amount").ToString();
                    }
                    dltamount = dltamount + int.Parse(amountfromDB); //จำนวนที่ลบจากตะกร้า+สต๊อกหลัก
                    conn.Close();
                    conn.Open();
                    sql = "UPDATE shop SET amount = '" + dltamount + "' WHERE id = '" + deletepro + "'";
                    command = new MySqlCommand(sql, conn);
                    command.ExecuteReader();
                    conn.Close();
                    MessageBox.Show("ลบข้อมูลสำเร็จ", "แจ้งเตือน");
                    Showdataproduct();
                    Showdatacart();
                    showprice();
                }
            }
        }

        private void button6_Click_1(object sender, EventArgs e)// รีเฟรชข้อมูล ให้เป็นค่าเริ่มต้น
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
            dataShop.DataSource = ds.Tables[0].DefaultView;
            comboBox1.Text = "ประเภทสินค้า";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) //โชว์ข้อมูล แล้วกดให้ไปโชว์ที่textbox
        {
            dtstock = false;
            dataGridView1.CurrentRow.Selected = true;
            txtcoll.Text = dataGridView1.Rows[e.RowIndex].Cells["collection"].FormattedValue.ToString();
            txtprice.Text = dataGridView1.Rows[e.RowIndex].Cells["price"].FormattedValue.ToString();
            txtdes.Text = dataGridView1.Rows[e.RowIndex].Cells["description"].FormattedValue.ToString();
            txtid.Text = dataGridView1.Rows[e.RowIndex].Cells["id"].FormattedValue.ToString();
            
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=sticpro;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"SELECT picture FROM shop WHERE id =\"{ txtid.Text}\"", conn);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                MemoryStream ms = new MemoryStream((byte[])ds.Tables[0].Rows[0]["picture"]);
                pictureBox2.Image = new Bitmap(ms);
            }
            conn = databaseConnection();
            conn.Open();
             
            cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT *FROM shop WHERE id='{txtid.Text}'";
            MySqlDataReader row = cmd.ExecuteReader();
            if (row.HasRows)
            {
                while (row.Read())
                {
                    textBox5.Text = row.GetString(4); //จำนวนในสต๊อก
                   
                }
            }
            conn.Close();

        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "0")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "0";
                textBox2.ForeColor = Color.Silver;
            }
        }

        private void Shop_FormClosing(object sender, FormClosingEventArgs e)
        {
            MySqlConnection conn = databaseConnection();
            conn.Open();
            MySqlCommand cmd;
            cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM `order` ";
            MySqlDataReader row = cmd.ExecuteReader();

            while (row.Read())
            {
                MySqlConnection conn2 = databaseConnection();
                conn2.Open();
                MySqlCommand cmd2;
                cmd2 = conn2.CreateCommand();
                cmd2.CommandText = $"SELECT * FROM shop WHERE id = \"{row.GetString(5)}\"";
                MySqlDataReader row2 = cmd2.ExecuteReader();
                while (row2.Read())
                {
                    amountnew = $"{int.Parse(row.GetString(3)) + int.Parse(row2.GetString(4))}"; //จำนวนสินค้าในตะกร้า+จำนวนสินค้าในสต๊อก
                }
                conn2.Close();

                conn2.Open();
                string sql3 = "UPDATE shop SET amount = '" + amountnew + "' WHERE id = '" + row.GetString(5) + "'";
                MySqlCommand command = new MySqlCommand(sql3, conn2);
                command.ExecuteReader();
                conn2.Close();
            }
            conn.Close();

            MySqlConnection conn1 = databaseConnection();
            string sql = "DELETE FROM `order`";
            MySqlCommand cmd1 = new MySqlCommand(sql, conn1);

            conn1.Open();
            int rows = cmd1.ExecuteNonQuery();
            conn1.Close();
            Application.Exit();
        }

        private void dataShop_CellClick_1(object sender, DataGridViewCellEventArgs e) //โชว์ข้อมูลที่textbox
        {
            dtstock = true;
            dataShop.CurrentRow.Selected = true;
            txtcoll.Text = dataShop.Rows[e.RowIndex].Cells["collection"].FormattedValue.ToString();
            textBox5.Text = dataShop.Rows[e.RowIndex].Cells["amount"].FormattedValue.ToString();
            txtprice.Text = dataShop.Rows[e.RowIndex].Cells["price"].FormattedValue.ToString();
            txtdes.Text = dataShop.Rows[e.RowIndex].Cells["description"].FormattedValue.ToString();
            txtid.Text = dataShop.Rows[e.RowIndex].Cells["id"].FormattedValue.ToString();
            

            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=sticpro;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"SELECT picture FROM shop WHERE id =\"{ txtid.Text}\"", conn);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                MemoryStream ms = new MemoryStream((byte[])ds.Tables[0].Rows[0]["picture"]);
                pictureBox2.Image = new Bitmap(ms);
            }
        }

        private void show_order() //โชว์ข้อมูลตรงตะกร้า
        {
                MySqlConnection conn = databaseConnection();

                DataSet ds = new DataSet();

                conn.Open();
                MySqlCommand cmd;

                cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM `order`";

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(ds);

                conn.Close();
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
                dataGridView1.ReadOnly = true;
        }

        //private void comboamount_TextChanged(object sender, EventArgs e)
        //{

        //}

        //private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //        dtstock = false;
        //        dataGridView1.CurrentRow.Selected = true;
        //        txtcoll.Text = dataGridView1.Rows[e.RowIndex].Cells["collection"].FormattedValue.ToString();
        //        textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells["amount"].FormattedValue.ToString();
        //        txtprice.Text = dataGridView1.Rows[e.RowIndex].Cells["price"].FormattedValue.ToString();
        //        txtdes.Text = dataGridView1.Rows[e.RowIndex].Cells["description"].FormattedValue.ToString();
        //        txtid.Text = dataGridView1.Rows[e.RowIndex].Cells["id"].FormattedValue.ToString();
        //        string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=sticpro;";
        //        MySqlConnection conn = new MySqlConnection(connectionString);
        //        conn.Open();
        //        MySqlCommand cmd = new MySqlCommand($"SELECT picture FROM shop WHERE collection =\"{ txtcoll.Text}\"", conn);
        //        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            MemoryStream ms = new MemoryStream((byte[])ds.Tables[0].Rows[0]["picture"]);
        //            pictureBox2.Image = new Bitmap(ms);
        //        }


        //}
        }
}


