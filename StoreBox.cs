using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace LinQ_EF_Project
{
    public partial class StoreBox : Form
    {
        EF_Model Ent = new EF_Model();
        public StoreBox()
        {
            InitializeComponent();
        }

        //Display Stores IDs
        private void StoreBox_Load(object sender, EventArgs e)
        {
            foreach (Store st in Ent.Stores)
            {
                comboBox1.Items.Add(st.Store_ID);
            }
        }

        //Selected Store
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = int.Parse(comboBox1.Text);
            Store st = Ent.Stores.Find(id);
            textBox1.Text = st.Store_ID.ToString();
            textBox2.Text = st.Name;
            textBox3.Text = st.Address;
            textBox4.Text = st.Store_Manager;
            listBox1.Items.Clear();
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
        }

        //Adding Store
        private void button1_Click(object sender, EventArgs e)
        {
            Store str = new Store();
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {
                Store st = Ent.Stores.Find(int.Parse(textBox1.Text));
                if (st == null)
                {
                    str.Store_ID = int.Parse(textBox1.Text);
                    str.Name = textBox2.Text;
                    str.Address = textBox3.Text;
                    str.Store_Manager = textBox4.Text;
                    Ent.Stores.Add(str);
                    Ent.SaveChanges();
                    MessageBox.Show("Store Added :)");
                    comboBox1.Items.Add(str.Store_ID);
                }
                else { MessageBox.Show("The Store already exists !"); }
            }
            else { MessageBox.Show("Pleaze Enter Full Data !"); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {
                Store str = Ent.Stores.Find(int.Parse(textBox1.Text));
                if (str != null)
                {
                    str.Name = textBox2.Text;
                    str.Address = textBox3.Text;
                    str.Store_Manager = textBox4.Text;
                    Ent.SaveChanges();
                    MessageBox.Show("Store Updated :)");
                }
                else { MessageBox.Show("The Store doesn't exist !"); }
            }
            else { MessageBox.Show("Pleaze Enter Full Data !"); }
        }
    }
}
