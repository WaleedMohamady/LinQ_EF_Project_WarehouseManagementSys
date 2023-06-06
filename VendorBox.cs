using System;
using System.Windows.Forms;

namespace LinQ_EF_Project
{
    public partial class VendorBox : Form
    {
        EF_Model Ent = new EF_Model();
        public VendorBox()
        {
            InitializeComponent();
        }

        //Display Vendors IDs
        private void VendorBox_Load(object sender, EventArgs e)
        {
            foreach (Vendor vn in Ent.Vendors)
            {
                comboBox1.Items.Add(vn.Vendor_Id);

            }
        }

        //Selected ID
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = int.Parse(comboBox1.Text);
            Vendor vn = Ent.Vendors.Find(id);
            textBox1.Text = vn.Vendor_Id.ToString();
            textBox2.Text = vn.Vendor_Name;
            textBox3.Text = vn.Fax.ToString();
            textBox4.Text = vn.Mail;
            textBox5.Text = vn.Website;
            textBox6.Text = vn.Telephone.ToString();
            textBox7.Text = vn.Mobile_Num.ToString();
        }

        //Add Vendor
        private void button1_Click(object sender, EventArgs e)
        {
            Vendor vendor = new Vendor();
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                Vendor vn = Ent.Vendors.Find(int.Parse(textBox1.Text));
                if (vn == null)
                {
                    vendor.Vendor_Id = int.Parse(textBox1.Text);
                    vendor.Vendor_Name = textBox2.Text;
                    vendor.Mail = textBox4.Text;
                    vendor.Website = textBox5.Text;
                    try
                    {
                        vendor.Fax = int.Parse(textBox3.Text);
                        vendor.Telephone = int.Parse(textBox6.Text);
                        vendor.Mobile_Num = int.Parse(textBox7.Text);
                    }
                    catch (Exception)
                    {
                        vendor.Fax = null;
                        vendor.Telephone = null;
                        vendor.Mobile_Num = null;
                    }
                    Ent.Vendors.Add(vendor);
                    Ent.SaveChanges();
                    MessageBox.Show("Vendor Added :)");
                }
                else
                {
                    MessageBox.Show("The vendor already exist !");
                    textBox1.Text = "";
                }
            }
            else { MessageBox.Show("Pleaze Enter Full Data !"); }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            textBox1.ReadOnly = false;
            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
            if (checkBox1.Checked == false)
            {
                button1.Enabled = false;
                comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Vendor vendor = Ent.Vendors.Find(int.Parse(textBox1.Text));
            vendor.Vendor_Name = textBox2.Text;
            vendor.Mail = textBox4.Text;
            vendor.Website = textBox5.Text;
            try
            {
                vendor.Fax = int.Parse(textBox3.Text);
                vendor.Telephone = int.Parse(textBox6.Text);
                vendor.Mobile_Num = int.Parse(textBox7.Text);
            }
            catch (Exception)
            {
                vendor.Fax = null;
                vendor.Telephone = null;
                vendor.Mobile_Num = null;
            }
            Ent.SaveChanges();
            MessageBox.Show("Vendor Data Edited");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
        }
    }
}
