using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace LinQ_EF_Project
{
    public partial class MoveItemsBox : Form
    {
        EF_Model Ent = new EF_Model();
        public MoveItemsBox()
        {
            InitializeComponent();
        }

        private void MoveItemsBox_Load(object sender, EventArgs e)
        {
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

            foreach (Store st in Ent.Stores)
            {
                comboBox1.Items.Add(st.Name);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            Store store = Ent.Stores.FirstOrDefault(a => a.Name == comboBox1.Text);
            var vendors = from v in Ent.Supplies where v.Store_Id == store.Store_ID select v.Vendor_Id;
            foreach (var vendor in vendors)
            {
                Vendor ven = Ent.Vendors.FirstOrDefault(a => a.Vendor_Id == vendor);
                if (comboBox3.Items.Contains(ven.Vendor_Name))
                {
                    continue;
                }
                comboBox3.Items.Add(ven.Vendor_Name);
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            DateTime proDate = DateTime.Parse(textBox2.Text);
            DateTime ExDate = DateTime.Parse(textBox3.Text);
            Store store = Ent.Stores.FirstOrDefault(a => a.Name == comboBox1.Text);
            Vendor ven = Ent.Vendors.FirstOrDefault(a => a.Vendor_Name == comboBox3.Text);
            var SuppliedNums = from N in Ent.Supplies where N.Store_Id == store.Store_ID && N.Vendor_Id == ven.Vendor_Id select N.Supp_Num;
            foreach (var item in SuppliedNums)
            {
                var SuppliedProducts = Ent.Supply_Quantity.Where(a => a.Supply_Num == item && a.Prod_Date == proDate && a.Expiration_Date == ExDate).Select(a => a.Product_Code);
                foreach (var P in SuppliedProducts)
                {
                    Product product = Ent.Products.FirstOrDefault(a => a.Product_Code == P);
                    if (comboBox2.Items.Contains(product.Product_Name))
                    {
                        continue;
                    }
                    comboBox2.Items.Add(product.Product_Name);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime proDate = DateTime.Parse(textBox2.Text);
                DateTime ExDate = DateTime.Parse(textBox3.Text);
                int quantity = int.Parse(textBox1.Text);
                int sumQuantities = 0;
                Product pr = Ent.Products.FirstOrDefault(a => a.Product_Name == comboBox2.Text);
                Store store = Ent.Stores.FirstOrDefault(a => a.Name == comboBox1.Text);
                Vendor ven = Ent.Vendors.FirstOrDefault(a => a.Vendor_Name == comboBox3.Text);
                var SuppliedNums = from N in Ent.Supplies where N.Store_Id == store.Store_ID && N.Vendor_Id == ven.Vendor_Id select N.Supp_Num;
                foreach (var item in SuppliedNums)
                {
                    var SuppliedQuantities = Ent.Supply_Quantity.Where(a => a.Supply_Num == item && a.Product_Code == pr.Product_Code).Select(a => a.Supplied_Quantity);
                    foreach (var Q in SuppliedQuantities)
                    {
                        sumQuantities += Q;
                    }
                }
                if (quantity < sumQuantities)
                {
                    Product prod = Ent.Products.FirstOrDefault(a => a.Product_Name == comboBox2.Text);
                    Store str = Ent.Stores.FirstOrDefault(a => a.Name == comboBox1.Text);
                    Vendor vn = Ent.Vendors.FirstOrDefault(a => a.Vendor_Name == comboBox3.Text);
                    Supply supply = Ent.Supplies.FirstOrDefault(a => a.Store_Id == str.Store_ID && a.Vendor_Id == vn.Vendor_Id);
                    Supply_Quantity SQ = Ent.Supply_Quantity.FirstOrDefault(a => a.Supply_Num == supply.Supp_Num);
                    SQ.Supplied_Quantity -= quantity;
                    Supply NewSupplyPermission = new Supply();
                    Supply supplyPer = Ent.Supplies.Find(int.Parse(textBox1.Text));
                    if (supplyPer != null)
                    {
                        MessageBox.Show("There is a permission with same Number !");
                        return;
                    }
                    NewSupplyPermission.Supp_Num = supply.Supp_Num + 1000;
                    NewSupplyPermission.Supp_Date = supply.Supp_Date;
                    Store st = Ent.Stores.FirstOrDefault(s => s.Name == comboBox4.Text);
                    NewSupplyPermission.Store_Id = st.Store_ID;
                    Vendor vnd = Ent.Vendors.FirstOrDefault(s => s.Vendor_Name == comboBox3.Text);
                    NewSupplyPermission.Vendor_Id = vn.Vendor_Id;
                    Ent.Supplies.Add(NewSupplyPermission);
                    Ent.SaveChanges();
                    try
                    {
                        Supply_Quantity SuppliedProducts = new Supply_Quantity();
                        SuppliedProducts.Supply_Num = supply.Supp_Num + 1000;
                        Product pro = Ent.Products.FirstOrDefault(a => a.Product_Name == comboBox2.Text);
                        SuppliedProducts.Product_Code = pr.Product_Code;
                        SuppliedProducts.Supplied_Quantity = quantity;
                        SuppliedProducts.Prod_Date = proDate;
                        SuppliedProducts.Expiration_Date = ExDate;
                        Ent.Supply_Quantity.Add(SuppliedProducts);
                        Ent.SaveChanges();
                        MessageBox.Show("Supply Permission Added");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("There is a duplicate product !");
                        return;
                    }
                }
                else if (quantity == sumQuantities)
                {
                    Product prod = Ent.Products.FirstOrDefault(a => a.Product_Name == comboBox2.Text);
                    Store str = Ent.Stores.FirstOrDefault(a => a.Name == comboBox1.Text);
                    Vendor vn = Ent.Vendors.FirstOrDefault(a => a.Vendor_Name == comboBox3.Text);
                    Supply supply = Ent.Supplies.FirstOrDefault(a => a.Store_Id == str.Store_ID && a.Vendor_Id == vn.Vendor_Id);
                    Supply_Quantity SQ = Ent.Supply_Quantity.FirstOrDefault(a => a.Supply_Num == supply.Supp_Num);
                    Ent.Supply_Quantity.Remove(SQ);
                }

                else { MessageBox.Show("The Ordered Quantity is greater than The Stock !"); }
            }
            catch (Exception)
            { MessageBox.Show("Pleaze Enter Valid Quantity !"); }
        }
    }
}
