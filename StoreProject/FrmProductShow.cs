using System;

using System.Collections.Generic;

using System.ComponentModel;

using System.Data;

using System.Data.SqlClient;

using System.Drawing;

using System.Drawing.Text;

using System.IO;

using System.Linq;

using System.Text;

using System.Threading.Tasks;

using System.Windows.Forms;

namespace StoreProject

{

    public partial class FrmProductShow : Form

    {

        public FrmProductShow()

        {

            InitializeComponent();

        }

        private Image convertByteArrayToImage(byte[] byteArrayIn)

        {

            if (byteArrayIn == null || byteArrayIn.Length == 0)

            {

                return null;

            }

            try

            {

                using (MemoryStream ms = new MemoryStream(byteArrayIn))

                {

                    return Image.FromStream(ms);

                }

            }

            catch (ArgumentException ex)

            {

                // อาจเกิดขึ้นถ้า byte array ไม่ใช่ข้อมูลรูปภาพที่ถูกต้อง

                Console.WriteLine("Error converting byte array to image: " + ex.Message);

                return null;

            }

        }


        //สร้างเมธอดดึงข้อมูลทั้งหมดจาก product_tb มาแสดงที่ list view

        private void getAllproductToLV()

        {

            //Connect String เพื่อติดต่อไปยังฐานข้อมูล

            string connectionString = @"Server=DESKTOP-9U4FO0V\SQLEXPRESS;Database=store_db;Trusted_Connection=True;";

            //สร้าง Connection ไปยังฐานข้อมูล

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))

            {

                try

                {

                    sqlConnection.Open(); //เปิดการเชื่อมต่อไปยังฐานข้อมูล

                    //การทำงานกับตารางในฐานข้อมูล (SELECT, INSERT, UPDATE, DELETE)

                    //สร้างคำสั่ง SQL ในที่นี้คือ ดึงข้อมูลทั้งหมดจากตาราง product_tb

                    string strSQL = "SELECT proId, proName, proPrice, proQuan, proUnit, proStatus, proImage FROM product_tb";


                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(strSQL, sqlConnection))

                    {

                        //ดึง

                        DataTable dataTable = new DataTable();

                        dataAdapter.Fill(dataTable);

                        //ตั้งค่า ListView

                        lvAllProduct.Items.Clear();

                        lvAllProduct.Columns.Clear();

                        lvAllProduct.FullRowSelect = true;

                        lvAllProduct.View = View.Details;

                        //ตั้งค่าการแสดงรูปใน ListView

                        if (lvAllProduct.SmallImageList == null)

                        {

                            lvAllProduct.SmallImageList = new ImageList();

                            lvAllProduct.SmallImageList.ImageSize = new Size(50, 50);

                            lvAllProduct.SmallImageList.ColorDepth = ColorDepth.Depth32Bit;

                        }

                        lvAllProduct.SmallImageList.Images.Clear();

                        //กำหนดรายละเอียดของ Column ใน ListView

                        lvAllProduct.Columns.Add("รูปภาพ", 100, HorizontalAlignment.Left);

                        lvAllProduct.Columns.Add("รหัสสินค้า", 100, HorizontalAlignment.Left);

                        lvAllProduct.Columns.Add("ชื่อสินค้า", 250, HorizontalAlignment.Left);

                        lvAllProduct.Columns.Add("ราคา", 80, HorizontalAlignment.Left);

                        lvAllProduct.Columns.Add("จำนวน", 80, HorizontalAlignment.Left);

                        lvAllProduct.Columns.Add("หน่วย", 80, HorizontalAlignment.Left);

                        lvAllProduct.Columns.Add("สถานะ", 120, HorizontalAlignment.Left);

                        //Loop วนเข้าไปใน DataTable

                        foreach (DataRow dataRow in dataTable.Rows)

                        {

                            ListViewItem item = new ListViewItem(); //สร้าง item เพื่อเก็บแต่ละข้อมูลในแต่ละรายการ

                            //เอารูปใส่ใน item

                            Image proImage = null;

                            if (dataRow["proImage"] != DBNull.Value)

                            {

                                byte[] imgByte = (byte[])dataRow["proImage"];

                                //แปลงข้อมูลรูปจากฐานข้อมูลซึ่งเป็น Binary ให้เป็นรูป

                                proImage = convertByteArrayToImage(imgByte);

                            }

                            string imageKey = null;

                            if (proImage != null)

                            {

                                imageKey = $"pro_{dataRow["proId"]}";

                                lvAllProduct.SmallImageList.Images.Add(imageKey, proImage);

                                item.ImageKey = imageKey;

                            }

                            else

                            {

                                item.ImageIndex = -1;

                            }

                            //เอาแต่ละรายการใส่ใน item

                            item.SubItems.Add(dataRow["proId"].ToString());

                            item.SubItems.Add(dataRow["proName"].ToString());

                            item.SubItems.Add(dataRow["proPrice"].ToString());

                            item.SubItems.Add(dataRow["proQuan"].ToString());

                            item.SubItems.Add(dataRow["proUnit"].ToString());

                            item.SubItems.Add(dataRow["proStatus"].ToString());

                            //เอาข้อมูลใน item

                            lvAllProduct.Items.Add(item);



                        }

                    }

                }

                catch (Exception ex)

                {

                    MessageBox.Show("พบข้อผิดพลาด กรุณาลองใหม่หรือติอต่อ IT :" + ex.Message);

                }

            }

        }



        //Form_Load จะทำงานทุกครั้งที่ Form ถูกเปิดขึ้นมา

        private void FrmProductShow_Load(object sender, EventArgs e)

        {

            //ให้ไปดึงข้อมูลจาก product_tb มาแสดงที่ ListView

            getAllproductToLV();

        }

    }

}

