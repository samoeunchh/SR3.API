using SR3.DaraLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SR3.PresentationDesktop
{
    public partial class Form1 : Form
    {
        private readonly HttpClient client = new HttpClient();
        public Form1()
        {
            client.BaseAddress = new Uri("http://localhost:48459/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            HttpResponseMessage response = await client.GetAsync("api/customers");
            listView1.Items.Clear();
            if (response.IsSuccessStatusCode)
            {
                var customers =await response.Content.ReadAsAsync<List<Customer>>();
                if (customers == null)
                    MessageBox.Show("No record");
                else
                {
                    foreach(var item in customers)
                    {
                        var li = listView1.Items.Add(item.CustomerId.ToString());
                        li.SubItems.Add(item.CustomerName);
                        li.SubItems.Add(item.Gender);
                        li.SubItems.Add(item.Mobile);
                        li.SubItems.Add(item.Address);
                    }
                }
            }
            else
            {
                var msg = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                MessageBox.Show(msg);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Customer Name field is required");
                return;
            }
            if (string.IsNullOrEmpty(cboGender.Text))
            {
                MessageBox.Show("Customer gender field is required");
                return;
            }
            var customer = new Customer
            {
                CustomerName=txtName.Text,
                Gender=cboGender.Text,
                Address=txtAddress.Text,
                Mobile=txtMobile.Text
            };
            HttpResponseMessage response = 
                await client.PostAsJsonAsync<Customer>("api/customers", customer);
            if (response.IsSuccessStatusCode)
            {
                txtAddress.Text = "";
                txtMobile.Text = "";
                txtName.Text = "";
                cboGender.Text = "";
                MessageBox.Show("Record was saved");
                Form1_Load(sender, e);
            }
            else
            {
                var msg = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                MessageBox.Show(msg);
            }
        }
    }
}
