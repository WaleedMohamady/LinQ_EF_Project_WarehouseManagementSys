using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace LinQ_EF_Project
{
    public partial class ReportsBox : Form
    {
        EF_Model Ent = new EF_Model();
        public ReportsBox()
        {
            InitializeComponent();
        }

        private void ReportsBox_Load(object sender, EventArgs e)
        {
            foreach (Store st in Ent.Stores)
            {
                comboBox1.Items.Add(st.Store_ID);
            }
            foreach (Product product in Ent.Products)
            {
                comboBox2.Items.Add(product.Product_Code);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            int id = int.Parse(comboBox1.Text);
            Store st = Ent.Stores.Find(id);
            label3.Text = st.Store_ID.ToString();
            label5.Text = st.Name;
            label7.Text = st.Address;
            label9.Text = st.Store_Manager;
            listBox1.Items.Clear();
            if (textBox1.Text == "")
            {
                Supply supp = Ent.Supplies.FirstOrDefault(a => a.Store_Id == id);
                if (supp != null)
                {
                    var pCode_Seq = from sq in Ent.Supply_Quantity where sq.Supply_Num == supp.Supp_Num select sq.Product_Code;
                    foreach (int pCode in pCode_Seq)
                    {
                        Product product = Ent.Products.FirstOrDefault(a => a.Product_Code == pCode);
                        listBox1.Items.Add(product.Product_Name);
                    }
                }
                Order ord = Ent.Orders.FirstOrDefault(a => a.Store_Id == id);
                if (ord != null)
                {
                    var pCode_Seq = from sq in Ent.Order_Quantity where sq.Order_Num == ord.Order_Num select sq.Product_Code;
                    foreach (int pCode in pCode_Seq)
                    {
                        Product product = Ent.Products.FirstOrDefault(a => a.Product_Code == pCode);
                        listBox2.Items.Add(product.Product_Name);
                    }
                }
            }
            else
            {
                DateTime date = DateTime.Parse(textBox1.Text);
                Supply supp = Ent.Supplies.FirstOrDefault(a => a.Store_Id == id && a.Supp_Date == date);
                if (supp != null)
                {
                    var pCode_Seq = from sq in Ent.Supply_Quantity where sq.Supply_Num == supp.Supp_Num select sq.Product_Code;
                    foreach (int pCode in pCode_Seq)
                    {
                        Product product = Ent.Products.FirstOrDefault(a => a.Product_Code == pCode);
                        listBox1.Items.Add(product.Product_Name);
                    }
                }

                Order ord = Ent.Orders.FirstOrDefault(a => a.Store_Id == id && a.Order_Date == date);
                if (ord != null)
                {
                    var pCode_Seq = from sq in Ent.Order_Quantity where sq.Order_Num == ord.Order_Num select sq.Product_Code;
                    foreach (int pCode in pCode_Seq)
                    {
                        Product product = Ent.Products.FirstOrDefault(a => a.Product_Code == pCode);
                        listBox2.Items.Add(product.Product_Name);
                    }
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int code = int.Parse(comboBox2.Text);
            Product Pr = Ent.Products.Find(code);
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            label13.Text = Pr.Product_Code.ToString();
            label18.Text = Pr.Product_Name;
            var units = from u in Ent.Product_MeasuringUnit where u.Product_Code == code select u.Measuring_Unit;
            foreach (var unit in units) { listBox5.Items.Add(unit); }
            if (textBox1.Text == "")
            {
                Supply_Quantity SQ = Ent.Supply_Quantity.FirstOrDefault(a => a.Product_Code == code);
                if (SQ != null)
                {
                    var Stores_Seq = from sq in Ent.Supplies where sq.Supp_Num == SQ.Supply_Num select sq.Store_Id;
                    foreach (int store in Stores_Seq)
                    {
                        Store str = Ent.Stores.FirstOrDefault(a => a.Store_ID == store);
                        listBox4.Items.Add(str.Name);
                    }
                }
                Order_Quantity ord = Ent.Order_Quantity.FirstOrDefault(a => a.Product_Code == code);
                if (ord != null)
                {
                    var Stores_Seq = from sq in Ent.Orders where sq.Order_Num == ord.Order_Num select sq.Store_Id;
                    foreach (int store in Stores_Seq)
                    {
                        Store str = Ent.Stores.FirstOrDefault(a => a.Store_ID == store);
                        listBox3.Items.Add(str.Name);
                    }
                }
            }
            else
            {
                DateTime date = DateTime.Parse(textBox1.Text);
                Supply_Quantity SQ = Ent.Supply_Quantity.FirstOrDefault(a => a.Product_Code == code);
                if (SQ != null)
                {
                    var Stores_Seq = from sq in Ent.Supplies where sq.Supp_Num == SQ.Supply_Num && sq.Supp_Date == date select sq.Store_Id;
                    foreach (int store in Stores_Seq)
                    {
                        Store str = Ent.Stores.FirstOrDefault(a => a.Store_ID == store);
                        listBox4.Items.Add(str.Name);
                    }
                }

                Order_Quantity ord = Ent.Order_Quantity.FirstOrDefault(a => a.Product_Code == code);
                if (ord != null)
                {
                    var Stores_Seq = from sq in Ent.Orders where sq.Order_Num == ord.Order_Num && sq.Order_Date == date select sq.Store_Id;
                    foreach (int store in Stores_Seq)
                    {
                        Store str = Ent.Stores.FirstOrDefault(a => a.Store_ID == store);
                        listBox3.Items.Add(str.Name);
                    }
                }
            }
        }


    }
}
