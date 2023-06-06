using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinQ_EF_Project
{
    public partial class ClientBox : Form
    {
        EF_Model Ent = new EF_Model();

        public ClientBox()
        {
            InitializeComponent();
        }

        //Display Vendors IDs
        private void ClientBox_Load(object sender, EventArgs e)
        {
            foreach (Client cl in Ent.Clients)
            {
                comboBox1.Items.Add(cl.Client_Id);

            }
        }

        //Selected ID
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = int.Parse(comboBox1.Text);
            Client cl = Ent.Clients.Find(id);
            textBox1.Text = cl.Client_Id.ToString();
            textBox2.Text = cl.Client_Name;
            textBox3.Text = cl.Fax.ToString();
            textBox4.Text = cl.Mail;
            textBox5.Text = cl.Website;
            textBox6.Text = cl.Telephone.ToString();
            textBox7.Text = cl.Mobile_Num.ToString();
        }

        //Add Vendor
        private void button1_Click(object sender, EventArgs e)
        {
            Client client = new Client();
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                Client cl = Ent.Clients.Find(int.Parse(textBox1.Text));
                if (cl == null)
                {
                    client.Client_Id = int.Parse(textBox1.Text);
                    client.Client_Name = textBox2.Text;
                    client.Mail = textBox4.Text;
                    client.Website = textBox5.Text;
                    try
                    {
                        client.Fax = int.Parse(textBox3.Text);
                        client.Telephone = int.Parse(textBox6.Text);
                        client.Mobile_Num = int.Parse(textBox7.Text);
                    }
                    catch (Exception)
                    {
                        client.Fax = null;
                        client.Telephone = null;
                        client.Mobile_Num = null;
                    }
                    Ent.Clients.Add(client);
                    Ent.SaveChanges();
                    MessageBox.Show("Client Added :)");
                }
                else
                {
                    MessageBox.Show("The Client already exist !");
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
            Client client = Ent.Clients.Find(int.Parse(textBox1.Text));
            client.Client_Name = textBox2.Text;
            client.Mail = textBox4.Text;
            client.Website = textBox5.Text;
            try
            {
                client.Fax = int.Parse(textBox3.Text);
                client.Telephone = int.Parse(textBox6.Text);
                client.Mobile_Num = int.Parse(textBox7.Text);
            }
            catch (Exception)
            {
                client.Fax = null;
                client.Telephone = null;
                client.Mobile_Num = null;
            }
            Ent.SaveChanges();
            MessageBox.Show("Client Data Edited");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
        }
    }
}
