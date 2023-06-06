using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace LinQ_EF_Project
{
    public partial class SupplyPermissionBox : Form
    {
        EF_Model Ent = new EF_Model();

        public SupplyPermissionBox()
        {
            InitializeComponent();
        }

        ///Display Supplies
        private void SupplyPermissionBox_Load(object sender, EventArgs e)
        {
            foreach (Supply supp in Ent.Supplies)
            {
                comboBox1.Items.Add(supp.Supp_Num);
            }
        }

        ///Selected Number
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int num = int.Parse(comboBox1.Text);
            Supply supply = Ent.Supplies.Find(num);
            textBox1.Text = supply.Supp_Num.ToString();
            dateTimePicker4.Text = supply.Supp_Date.ToString();
            Store st = Ent.Stores.Find(supply.Store_Id);
            textBox3.Text = st.Name;
            Vendor vr = Ent.Vendors.Find(supply.Vendor_Id);
            textBox4.Text = vr.Vendor_Name;
            listView1.Items.Clear();
            var products = from pr in Ent.Supply_Quantity where pr.Supply_Num == supply.Supp_Num select pr;
            foreach (var prod in products)
            {
                Product product = Ent.Products.Find(prod.Product_Code);
                ListViewItem item = new ListViewItem(product.Product_Name);
                item.SubItems.Add(prod.Supplied_Quantity.ToString());
                item.SubItems.Add(prod.Prod_Date.ToString());
                item.SubItems.Add(prod.Expiration_Date.ToString());
                listView1.Items.Add(item);
            }
        }

        /// ADD Btn CheckBox
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            textBox1.ReadOnly = false;
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            dateTimePicker4.Enabled = true;
            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            foreach (Store store in Ent.Stores)
            {
                comboBox2.Items.Add(store.Name);
            }
            foreach (Vendor vendor in Ent.Vendors)
            {
                comboBox3.Items.Add(vendor.Vendor_Name);
            }
            foreach (Product product in Ent.Products)
            {
                comboBox4.Items.Add(product.Product_Name);
            }
            if (checkBox1.Checked == false)
            {
                button1.Enabled = false;
                textBox1.ReadOnly = true;
            }
        }

        /// New Btns
        private void button3_Click(object sender, EventArgs e)
        {
            StoreBox store = new StoreBox();
            store.ShowDialog();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            VendorBox vendor = new VendorBox();
            vendor.ShowDialog();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            ProductBox product = new ProductBox();
            product.ShowDialog();
        }

        //
        // Add Btn
        //
        private void button1_Click(object sender, EventArgs e)
        {
            Supply NewSupplyPermission = new Supply();
            Supply supplyPer = Ent.Supplies.Find(int.Parse(textBox1.Text));
            if (supplyPer == null)
            {
                if (listView1.Items.Count != 0)
                {
                    NewSupplyPermission.Supp_Num = int.Parse(textBox1.Text);
                    NewSupplyPermission.Supp_Date = dateTimePicker4.Value;
                    Store st = Ent.Stores.FirstOrDefault(s => s.Name == comboBox2.Text);
                    NewSupplyPermission.Store_Id = st.Store_ID;
                    Vendor vn = Ent.Vendors.FirstOrDefault(s => s.Vendor_Name == comboBox3.Text);
                    NewSupplyPermission.Vendor_Id = vn.Vendor_Id;
                    Ent.Supplies.Add(NewSupplyPermission);
                    Ent.SaveChanges();
                    foreach (ListViewItem item in listView1.Items)
                    {
                        try
                        {
                            Supply_Quantity SuppliedProducts = new Supply_Quantity();
                            SuppliedProducts.Supply_Num = int.Parse(textBox1.Text);
                            string str = item.SubItems[0].Text;
                            Product pr = Ent.Products.FirstOrDefault(a => a.Product_Name == str);
                            SuppliedProducts.Product_Code = pr.Product_Code;
                            SuppliedProducts.Supplied_Quantity = int.Parse(item.SubItems[1].Text);
                            SuppliedProducts.Prod_Date = DateTime.Parse(item.SubItems[2].Text);
                            SuppliedProducts.Expiration_Date = DateTime.Parse(item.SubItems[3].Text);
                            Ent.Supply_Quantity.Add(SuppliedProducts);
                            Ent.SaveChanges();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("There is a duplicate product !");
                            return;
                        }
                    }
                    MessageBox.Show("Supply Permission Added");
                }
                else { MessageBox.Show("Pleaze Enter at least one product"); }
            }
            else { MessageBox.Show("There is a permission with same Number !"); }
        }

        //
        // Add Product to the list Btn
        //
        private void button6_Click(object sender, EventArgs e)
        {
            ListViewItem Newitem = new ListViewItem(comboBox4.Text);
            try
            {
                Newitem.SubItems.Add(int.Parse(textBox5.Text).ToString());
                Newitem.SubItems.Add(dateTimePicker1.Value.ToString());
                Newitem.SubItems.Add(dateTimePicker2.Value.ToString());
                listView1.Items.Add(Newitem);
                comboBox1.Text = textBox5.Text = "";
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
            }
            catch (Exception)
            { MessageBox.Show("Pleaze Enter Valid Quantity !"); }
        }

        //Remove from List
        private void button7_Click(object sender, EventArgs e)
        {
            int num = int.Parse(textBox1.Text);
            if (listView1.SelectedItems != null)
            {
                Supply_Quantity sQ = Ent.Supply_Quantity.FirstOrDefault(a => a.Supply_Num == num);
                if (sQ != null)
                {
                    if (listView1.Items.Count > 1)
                    {
                        Ent.Supply_Quantity.Remove(sQ);
                        Ent.SaveChanges();
                        foreach (ListViewItem item in listView1.SelectedItems)
                        {
                            listView1.Items.Remove(item);
                        }
                    }
                    else { MessageBox.Show("Supplies must have at least one product !"); }
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
            groupBox2.Enabled = true;
            foreach (Store store in Ent.Stores)
            {
                comboBox2.Items.Add(store.Name);
            }
            foreach (Vendor vendor in Ent.Vendors)
            {
                comboBox3.Items.Add(vendor.Vendor_Name);
            }
            foreach (Product product in Ent.Products)
            {
                comboBox4.Items.Add(product.Product_Name);
            }
        }

        //Edit
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox4.Text != "")
            {
                Supply UpdatedSupply = Ent.Supplies.Find(int.Parse(textBox1.Text));
                if (UpdatedSupply != null)
                {
                    UpdatedSupply.Supp_Date = dateTimePicker4.Value;
                    Store st = Ent.Stores.FirstOrDefault(s => s.Name == comboBox2.Text);
                    UpdatedSupply.Store_Id = st.Store_ID;
                    Vendor vn = Ent.Vendors.FirstOrDefault(s => s.Vendor_Name == comboBox3.Text);
                    UpdatedSupply.Vendor_Id = vn.Vendor_Id;
                    Ent.SaveChanges();
                    foreach (ListViewItem item in listView1.Items)
                    {
                        int num = int.Parse(textBox1.Text);
                        string PName = item.SubItems[0].Text;
                        Product pr = Ent.Products.FirstOrDefault(a => a.Product_Name == PName);
                        Supply_Quantity SQ = Ent.Supply_Quantity.FirstOrDefault(a => a.Supply_Num == num && a.Product_Code == pr.Product_Code);
                        if (SQ == null)
                        {
                            Supply_Quantity SuppliedProducts = new Supply_Quantity();
                            SuppliedProducts.Supply_Num = num;
                            string str = item.SubItems[0].Text;
                            Product product = Ent.Products.FirstOrDefault(a => a.Product_Name == str);
                            SuppliedProducts.Product_Code= product.Product_Code;
                            SuppliedProducts.Supplied_Quantity = int.Parse(item.SubItems[1].Text);
                            SuppliedProducts.Prod_Date = DateTime.Parse(item.SubItems[2].Text);
                            SuppliedProducts.Expiration_Date = DateTime.Parse(item.SubItems[3].Text);
                            Ent.Supply_Quantity.Add(SuppliedProducts);
                            Ent.SaveChanges();
                            MessageBox.Show("Supply Permission Edited");
                        }
                        else
                        {
                            MessageBox.Show("There is a duplicate product");
                            return;
                        }
                    }
                }
                else { MessageBox.Show("The supply permission doesn't exist"); }
            }
            else { MessageBox.Show("Pleaze Enter Full Data"); }
        }
    }
}
