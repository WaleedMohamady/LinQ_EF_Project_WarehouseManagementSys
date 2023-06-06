using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace LinQ_EF_Project
{
    public partial class OrderPermissionBox : Form
    {
        EF_Model Ent = new EF_Model();
        public OrderPermissionBox()
        {
            InitializeComponent();
        }

        private void OrderPermissionBox_Load(object sender, EventArgs e)
        {
            foreach (Order ord in Ent.Orders)
            {
                comboBox1.Items.Add(ord.Order_Num);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int num = int.Parse(comboBox1.Text);
            Order order = Ent.Orders.Find(num);
            textBox1.Text = order.Order_Num.ToString();
            dateTimePicker1.Value = order.Order_Date;
            Store st = Ent.Stores.Find(order.Store_Id);
            textBox2.Text = st.Name;
            Client cl = Ent.Clients.Find(order.Client_Id);
            textBox3.Text = cl.Client_Name;
            listView1.Items.Clear();
            var products = from pr in Ent.Order_Quantity where pr.Order_Num == order.Order_Num select pr;
            foreach (var prod in products)
            {
                Product product = Ent.Products.Find(prod.Product_Code);
                ListViewItem item = new ListViewItem(product.Product_Name);
                item.SubItems.Add(prod.Ordered_Quantity.ToString());
                listView1.Items.Add(item);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            textBox1.ReadOnly = false;
            groupBox1.Enabled = true;
            dateTimePicker1.Enabled = true;
            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            var Stores = from st in Ent.Supplies select st.Store_Id;
            foreach (var store in Stores)
            {
                Store str = Ent.Stores.Find(store);
                if (comboBox2.Items.Contains(str.Name))
                {
                    continue;
                }
                comboBox2.Items.Add(str.Name);
            }

            foreach (Client client in Ent.Clients)
            {
                comboBox3.Items.Add(client.Client_Name);
            }

            if (checkBox1.Checked == false)
            {
                button1.Enabled = false;
                textBox1.ReadOnly = true;
            }
        }

        //New Btns
        private void button4_Click(object sender, EventArgs e)
        {
            ClientBox client = new ClientBox();
            client.ShowDialog();
        }

        //Add Btn
        private void button1_Click(object sender, EventArgs e)
        {
            Order NewOrderPermission = new Order();
            Order orderPer = Ent.Orders.Find(int.Parse(textBox1.Text));
            if (orderPer == null)
            {
                if (listView1.Items.Count != 0)
                {
                    NewOrderPermission.Order_Num = int.Parse(textBox1.Text);
                    NewOrderPermission.Order_Date = dateTimePicker1.Value;
                    Store st = Ent.Stores.FirstOrDefault(s => s.Name == comboBox2.Text);
                    NewOrderPermission.Store_Id = st.Store_ID;
                    Client cl = Ent.Clients.FirstOrDefault(s => s.Client_Name == comboBox3.Text);
                    NewOrderPermission.Client_Id = cl.Client_Id;
                    Ent.Orders.Add(NewOrderPermission);
                    Ent.SaveChanges();
                    foreach (ListViewItem item in listView1.Items)
                    {
                        try
                        {
                            Order_Quantity OrderedProducts = new Order_Quantity();
                            OrderedProducts.Order_Num = int.Parse(textBox1.Text);
                            string str = item.SubItems[0].Text;
                            Product pr = Ent.Products.FirstOrDefault(a => a.Product_Name == str);
                            OrderedProducts.Product_Code = pr.Product_Code;
                            OrderedProducts.Ordered_Quantity = int.Parse(item.SubItems[1].Text);
                            Ent.Order_Quantity.Add(OrderedProducts);
                            Ent.SaveChanges();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("There is a duplicate product !");
                            return;
                        }
                    }
                    MessageBox.Show("Order Permission Added");
                }
                else { MessageBox.Show("Pleaze Enter at least one product"); }
            }
            else { MessageBox.Show("There is a permission with same Number !"); }
        }

        //Add to list
        private void button3_Click(object sender, EventArgs e)
        {
            ListViewItem Newitem = new ListViewItem(comboBox4.Text);
            try
            {
                int quantity = int.Parse(textBox4.Text);
                int sumQuantities = 0;
                Product pr = Ent.Products.FirstOrDefault(a => a.Product_Name == comboBox4.Text);
                Store store = Ent.Stores.FirstOrDefault(a => a.Name == comboBox2.Text);
                var SuppliedNums = from N in Ent.Supplies where N.Store_Id == store.Store_ID select N.Supp_Num;
                foreach (var item in SuppliedNums)
                {
                    var SuppliedQuantities = Ent.Supply_Quantity.Where(a=> a.Supply_Num == item && a.Product_Code == pr.Product_Code).Select(a=> a.Supplied_Quantity);
                    foreach (var Q in SuppliedQuantities)
                    {
                        sumQuantities+= Q;
                    }
                }
                if (quantity <= sumQuantities)
                {
                    Newitem.SubItems.Add(int.Parse(textBox4.Text).ToString());
                    listView1.Items.Add(Newitem);
                    textBox4.Text = "";
                }
                else { MessageBox.Show("The Ordered Quantity is greater than The Stock !"); }
            }
            catch (Exception)
            { MessageBox.Show("Pleaze Enter Valid Quantity !"); }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            Store store = Ent.Stores.FirstOrDefault(a => a.Name == comboBox2.Text);
            Supply supp = Ent.Supplies.FirstOrDefault(a => a.Store_Id == store.Store_ID);
            var pCode_Seq = from sq in Ent.Supply_Quantity where sq.Supply_Num == supp.Supp_Num select sq.Product_Code;
            foreach (int pCode in pCode_Seq)
            {
                Product product = Ent.Products.FirstOrDefault(a => a.Product_Code == pCode);
                comboBox4.Items.Add(product.Product_Name);
            }
        }

        //Remove
        private void button5_Click(object sender, EventArgs e)
        {
            int num = int.Parse(textBox1.Text);
            if (listView1.SelectedItems != null)
            {
                Order_Quantity oQ = Ent.Order_Quantity.FirstOrDefault(a => a.Order_Num == num);
                if (oQ != null)
                {
                    if (listView1.Items.Count > 1)
                    {
                        Ent.Order_Quantity.Remove(oQ);
                        Ent.SaveChanges();
                        foreach (ListViewItem item in listView1.SelectedItems)
                        {
                            listView1.Items.Remove(item);
                        }
                    }
                    else { MessageBox.Show("Orders must have at least one product !"); }
                }
                else
                {
                    foreach (ListViewItem item in listView1.SelectedItems)
                    {
                        listView1.Items.Remove(item);
                    }
                }
            }
            else { MessageBox.Show("Select a product to remove"); }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            groupBox1.Enabled = true;
            var Stores = from st in Ent.Supplies select st.Store_Id;
            foreach (var store in Stores)
            {
                Store str = Ent.Stores.Find(store);
                if (comboBox2.Items.Contains(str.Name))
                {
                    continue;
                }
                comboBox2.Items.Add(str.Name);
            }

            foreach (Client client in Ent.Clients)
            {
                comboBox3.Items.Add(client.Client_Name);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox3.Text != "")
            {
                Order UpdatedOrder = Ent.Orders.Find(int.Parse(textBox1.Text));
                if (UpdatedOrder != null)
                {
                    UpdatedOrder.Order_Date = dateTimePicker1.Value;
                    Store st = Ent.Stores.FirstOrDefault(s => s.Name == comboBox2.Text);
                    UpdatedOrder.Store_Id = st.Store_ID;
                    Client cl = Ent.Clients.FirstOrDefault(s => s.Client_Name == comboBox3.Text);
                    UpdatedOrder.Client_Id = cl.Client_Id;
                    Ent.SaveChanges();
                    foreach (ListViewItem item in listView1.Items)
                    {
                        int num = int.Parse(textBox1.Text);
                        string PName = item.SubItems[0].Text;
                        Product pr = Ent.Products.FirstOrDefault(a => a.Product_Name == PName);
                        Order_Quantity OQ = Ent.Order_Quantity.FirstOrDefault(a => a.Order_Num == num && a.Product_Code == pr.Product_Code);
                        if (OQ == null)
                        {
                            Order_Quantity OrderedProducts = new Order_Quantity();
                            OrderedProducts.Order_Num = num;
                            string str = item.SubItems[0].Text;
                            Product product = Ent.Products.FirstOrDefault(a => a.Product_Name == str);
                            OrderedProducts.Product_Code = product.Product_Code;
                            OrderedProducts.Ordered_Quantity = int.Parse(item.SubItems[1].Text);
                            Ent.Order_Quantity.Add(OrderedProducts);
                            Ent.SaveChanges();
                            MessageBox.Show("Order Permission Edited");
                        }
                        else
                        {
                            MessageBox.Show("There is a duplicate product");
                            return;
                        }
                    }
                }
                else { MessageBox.Show("The Order permission doesn't exist"); }
            }
            else { MessageBox.Show("Pleaze Enter Full Data"); }
        }
    }
}
