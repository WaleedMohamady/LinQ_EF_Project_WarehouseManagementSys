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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StoreBox store = new StoreBox();
            store.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ProductBox product= new ProductBox();
            product.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            VendorBox vendor = new VendorBox();
            vendor.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClientBox client = new ClientBox();
            client.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SupplyPermissionBox supplyPermissionBox = new SupplyPermissionBox();
            supplyPermissionBox.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OrderPermissionBox orderPermissionBox = new OrderPermissionBox();
            orderPermissionBox.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MoveItemsBox moveItemsBox = new MoveItemsBox();
            moveItemsBox.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ReportsBox reportsBox = new ReportsBox();
            reportsBox.ShowDialog();
        }
    }
}
