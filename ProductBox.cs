using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace LinQ_EF_Project
{
    public partial class ProductBox : Form
    {
        EF_Model Ent = new EF_Model();
        public ProductBox()
        {
            InitializeComponent();
        }

        //Display Products Codes
        private void ProductBox_Load(object sender, EventArgs e)
        {
            foreach (Product product in Ent.Products)
            {
                comboBox1.Items.Add(product.Product_Code);
            }
            comboBox1.Text = "Existing Products ...........";
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
        }

        //Selected Product
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button3.Enabled = true;
            button4.Enabled = true;
            int code = int.Parse(comboBox1.Text);
            Product Pr = Ent.Products.Find(code);
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            textBox1.Text = Pr.Product_Code.ToString();
            textBox2.Text = Pr.Product_Name;
            var units = from u in Ent.Product_MeasuringUnit where u.Product_Code == code select u.Measuring_Unit;
            foreach (var unit in units) { listBox1.Items.Add(unit); }
            var sQ = from s in Ent.Supply_Quantity where s.Product_Code == code select s.Supply_Num;
            if (sQ != null)
            {
                foreach (int SuppNoticeNum in sQ)
                {
                    Supply supp = Ent.Supplies.FirstOrDefault(a => a.Supp_Num == SuppNoticeNum);
                    Store str = Ent.Stores.FirstOrDefault(s => s.Store_ID == supp.Store_Id);
                    if (listBox2.Items.Contains(str.Name))
                    {
                        continue;
                    }
                    listBox2.Items.Add(str.Name);
                }
            }
        }

        //.......................Adding
        private void button1_Click(object sender, EventArgs e)
        {
            Product product = new Product();
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                Product Pr = Ent.Products.Find(int.Parse(textBox1.Text));
                if (Pr == null)
                {
                    if (listBox1.Items.Count != 0)
                    {
                        product.Product_Code = int.Parse(textBox1.Text);
                        product.Product_Name = textBox2.Text;
                        Ent.Products.Add(product);
                        Ent.SaveChanges();
                        foreach (var item in listBox1.Items)
                        {
                            Product_MeasuringUnit product_MeasuringUnit = new Product_MeasuringUnit();
                            product_MeasuringUnit.Product_Code = int.Parse(textBox1.Text);
                            product_MeasuringUnit.Measuring_Unit = item.ToString();
                            Ent.Product_MeasuringUnit.Add(product_MeasuringUnit);
                            Ent.SaveChanges();
                        }
                        MessageBox.Show("Product Added :)");
                    }
                    else { MessageBox.Show("Pleaze Enter at least one measuring unit !"); }
                }
                else { MessageBox.Show("The Product already exists !"); }
            }
            else { MessageBox.Show("Pleaze Enter Full Data !"); }

        }

        //Add Units .............................
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                if (textBox3.Text != "")
                {
                    if (listBox1.Items.Contains(textBox3.Text)) { MessageBox.Show("The unit already exist !"); }
                    else
                    {
                        listBox1.Items.Add(textBox3.Text);
                        textBox3.Text = "";
                    }
                }
                else { MessageBox.Show("Enter an Unit !!"); }
            }
            else { MessageBox.Show("Pleaze Enter Full Data !"); }
        }

        //Remove Units .......................................
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                int PCode = int.Parse(textBox1.Text);
                if (listBox1.SelectedItem != null)
                {
                    Product_MeasuringUnit PU = Ent.Product_MeasuringUnit.FirstOrDefault(a => a.Product_Code == PCode && a.Measuring_Unit == listBox1.SelectedItem.ToString());
                    if (PU != null)
                    {
                        if (listBox1.Items.Count > 1)
                        {
                            Ent.Product_MeasuringUnit.Remove(PU);
                            Ent.SaveChanges();
                            listBox1.Items.Remove(listBox1.SelectedItem);
                        }
                        else { MessageBox.Show("Products must have at least one measuring unit !"); }
                    }
                    else
                    { listBox1.Items.Remove(listBox1.SelectedItem); }
                }
                else { MessageBox.Show("Pleaze select an unit to remove !"); }
            }
            else { MessageBox.Show("Pleaze Enter Full Data !"); }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = true;
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            comboBox1.Text = "";
            textBox1.Text = textBox2.Text = textBox3.Text = "";
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            if (checkBox1.Checked == false)
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = true;
                button4.Enabled = true;
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
                comboBox1.Text = "Existing Product .........";
                textBox1.Text = textBox2.Text = textBox3.Text = "";
                listBox1.Items.Clear();
                listBox2.Items.Clear();
            }
        }

        //Editing..........................................................
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                Product product = Ent.Products.Find(int.Parse(textBox1.Text));
                if (product != null)
                {
                    product.Product_Name = textBox2.Text;
                    Ent.SaveChanges();
                    foreach (var item in listBox1.Items)
                    {
                        int PCode = int.Parse(textBox1.Text);
                        Product_MeasuringUnit PU = Ent.Product_MeasuringUnit.FirstOrDefault(a => a.Product_Code == PCode && a.Measuring_Unit == item.ToString().ToUpper());
                        if (PU == null)
                        {
                            Product_MeasuringUnit product_MeasuringUnit = new Product_MeasuringUnit();
                            product_MeasuringUnit.Product_Code = int.Parse(textBox1.Text);
                            product_MeasuringUnit.Measuring_Unit = item.ToString();
                            Ent.Product_MeasuringUnit.Add(product_MeasuringUnit);
                            Ent.SaveChanges();
                        }
                    }
                    MessageBox.Show("Product Data Edited :)");
                }
                else { MessageBox.Show("The product doesn't exist"); }
            }
            else { MessageBox.Show("Pleaze Enter Full Data"); }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
            textBox2.ReadOnly = false;
        }
    }
}
