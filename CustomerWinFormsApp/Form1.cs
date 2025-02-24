using CustomerWinFormsApp.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CustomerWinFormsApp
{
    public partial class Form1 : Form
    {
        private readonly IConfiguration _configuration;
        private readonly string apiUrl;
        public Form1(IConfiguration configuration)
        {
            InitializeComponent();
            _configuration = configuration;
            apiUrl = _configuration["ApiSettings:BaseUrl"] ?? throw new InvalidOperationException("API URL is not configured.");
        }
        private async void Form1_Load_1(object sender, EventArgs e)
        {
            var customers = await GetCustomersAsync();
            dataGridView1.DataSource = customers;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var row = dataGridView1.SelectedRows[0];
                txtId.Text = row.Cells[0].Value?.ToString() ?? "";
                txtName.Text = row.Cells[1].Value?.ToString() ?? "";
                txtEmail.Text = row.Cells[2].Value?.ToString() ?? "";
                txtPhone.Text = row.Cells[3].Value?.ToString() ?? "";
                txtAddress.Text = row.Cells[4].Value?.ToString() ?? "";
            }
        }
        // Method to fetch customers from the API
        private async Task<List<Customer>> GetCustomersAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var customers = JsonConvert.DeserializeObject<List<Customer>>(data);
                    return customers;
                }
                else
                {
                    MessageBox.Show("Failed to load data from API.");
                    return new List<Customer>();
                }
            }
        }

        //Method to add a new Customer 
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Please provide  Name.");
                    return;
                }
                if (string.IsNullOrEmpty(txtEmail.Text))
                {
                    MessageBox.Show("Please provide Email.");
                    return;
                }
                else
                {
                    string email = txtEmail.Text.Trim();
                    if (!IsValidEmail(email))
                    {
                        MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtEmail.Focus();
                        return;
                    }
                }
                if (string.IsNullOrEmpty(txtPhone.Text))
                {
                    MessageBox.Show("Please provide Phone number");
                    return;
                }
                else
                {
                    string phoneNumber = txtPhone.Text.Trim();
                    if (!IsValidPhoneNumber(phoneNumber))
                    {
                        MessageBox.Show("Please enter a valid 10-digit phone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtPhone.Focus();
                        return;
                    }
                }
                if (string.IsNullOrEmpty(txtAddress.Text))
                {
                    MessageBox.Show("Please provide  Address");
                    return;
                }
                int newId = GetMaxIdFromDataGridView() + 1;
                var newCustomer = new Customer
                {
                    Id = newId,
                    Name = txtName.Text,
                    Email = txtEmail.Text,
                    PhoneNumber =txtPhone.Text,
                    Address = txtAddress.Text
                };
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var content = new StringContent(JsonConvert.SerializeObject(newCustomer), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("", content);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Customer added successfully.");
                        txtId.Clear();
                        txtName.Clear();
                        txtEmail.Clear();
                        txtPhone.Clear();
                        txtAddress.Clear();
                        var customers = await GetCustomersAsync();
                        dataGridView1.DataSource = customers;
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Failed to add customer: {errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding customer: {ex.Message}");
            }
        }

        //Method to update an existing Customer 
        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    MessageBox.Show("Please enter a valid Customer ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Please provide  Name.");
                    return;
                }
                if (string.IsNullOrEmpty(txtEmail.Text))
                {
                    MessageBox.Show("Please provide Email.");
                    return;
                }
                else
                {
                    string email = txtEmail.Text.Trim();
                    if (!IsValidEmail(email))
                    {
                        MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtEmail.Focus();
                        return;
                    }
                }
                if (string.IsNullOrEmpty(txtPhone.Text))
                {
                    MessageBox.Show("Please provide Phone number");
                    return;
                }
                else
                {
                    string phoneNumber = txtPhone.Text.Trim();
                    if (!IsValidPhoneNumber(phoneNumber))
                    {
                        MessageBox.Show("Please enter a valid 10-digit phone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtPhone.Focus();
                        return;
                    }
                }
                if (string.IsNullOrEmpty(txtAddress.Text))
                {
                    MessageBox.Show("Please provide  Address");
                    return;
                }
                var customerDto = new
                {
                    Id = txtId.Text,
                    Name = txtName.Text,
                    Email = txtEmail.Text,
                    PhoneNumber = txtPhone.Text,
                    Address = txtAddress.Text
                };
                string jsonCustomer = JsonConvert.SerializeObject(customerDto);
                var content = new StringContent(jsonCustomer, Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PutAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var customers = await GetCustomersAsync();
                        dataGridView1.DataSource = customers;
                        MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        MessageBox.Show("Customer not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Failed to update customer: {errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding customer: {ex.Message}", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Method to delete an existing Customer 
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    MessageBox.Show("Please enter a valid Customer Id.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int customerId = int.Parse(txtId.Text);
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.DeleteAsync($"{apiUrl}/{customerId}");
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Customer deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtId.Clear();
                        txtName.Clear();
                        txtEmail.Clear();
                        txtPhone.Clear();
                        txtAddress.Clear();
                        var customers = await GetCustomersAsync();
                        dataGridView1.DataSource = customers;
                    }
                    else
                    {
                        MessageBox.Show($"Error deleting customer: {response.ReasonPhrase}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int GetMaxIdFromDataGridView()
        {
            // Initialize a variable to hold the maximum Id
            int maxId = 0;
            // Loop through the rows of the DataGridView to find the maximum Id
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Id"].Value != null)
                {
                    int id = Convert.ToInt32(row.Cells["Id"].Value);
                    maxId = Math.Max(maxId, id);  // Keep track of the maximum Id
                }
            }
            return maxId;
        }
        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            string phonePattern = @"^\d{10}$"; // Ensures exactly 10 digits
            return Regex.IsMatch(phoneNumber, phonePattern);
        }
    }
}