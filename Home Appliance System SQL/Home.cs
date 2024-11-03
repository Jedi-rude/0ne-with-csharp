using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Home_Appliance_System_SQL
{
    public partial class Home : Form
    {
        DBConnection connection = new DBConnection();
        List<Product> CP = new List<Product>();
        Product Product = new Product();

        SqlCommand command;
        SqlDataReader reader;

        public string UserName { get; set; }
        public int Totalvalue { get; set; }

        public Home()
        {
            InitializeComponent();
        }

        //This below exit button will work for stop using the application
        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to exit application", "Exit Meassage", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        //This following block of code will logout the current account
        private void BtnLogOut_Click(object sender, EventArgs e)
        {
            Login L = new Login();
            L.Show();
            this.Hide();
        }

        //The following button will perform product adding to the shopping cart views and shopping cart database 
        int GrandTotal = 0;
        private void BtnAddCart_Click(object sender, EventArgs e)
        {
            if (TextId.Text != "" && TxtPrice.Text != "" && TxtBrand.Text != "")
            {
                //There is no selected item in quantity and duration, the information message will show
                if (TxtQty.Text == "" || CmbDuration.Text == "")
                {
                    MessageBox.Show("Please Select Duration Contract and Quantity", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    int qty = Convert.ToInt32(TxtQty.Text);
                    int duration = Convert.ToInt32(CmbDuration.Text);
                    if (qty <= 0 || duration <= 0)
                    {
                        MessageBox.Show("There is no negative values for quantity and duration", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        //There is correct step for adding, the below block of code will work
                        try
                        {
                            int total = duration * qty * Convert.ToInt32(TxtPrice.Text);
                            DataGridViewRow newrow = new DataGridViewRow();
                            newrow.CreateCells(CartView);
                            newrow.Cells[0].Value = TextId.Text;
                            newrow.Cells[1].Value = TxtBrand.Text;
                            newrow.Cells[2].Value = TxtModel.Text;
                            newrow.Cells[3].Value = TxtPrice.Text;
                            newrow.Cells[4].Value = TxtQty.Text;
                            newrow.Cells[5].Value = CmbDuration.Text;
                            newrow.Cells[6].Value = total.ToString();
                            CartView.Rows.Add(newrow);
                            GrandTotal += total;
                            Totalvalue += GrandTotal;
                            LblTotal.Text = Totalvalue + "";
                            AddtodbCart();
                            ClearFields();
                        }
                        catch //This statement will for incorrect process for adding to cart
                        {
                            MessageBox.Show("Please Enter Data Type Correctly", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
     
                }
            }
            else
            {
                // This Message will show how to 
                MessageBox.Show("Please Select Row to add shoppingcart", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }                   
        }

        //This button search click will work for searching brands and categories of items
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (CmbBrand.Text != "" && CmbCategory.Text != "")
            {
                //This below statement will work for brand and category search
                if (CmbBrand.Text != "" || CmbCategory.Text != "")
                {
                    try
                    {
                        BrandandCategorySearch();
                    }
                    catch //  if there is no valid brand names and category names, this warning msg will show
                    {
                        MessageBox.Show("Please Select Given Items to Search", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

            //This statement will work for brand search
            else if (CmbBrand.Text != "" && CmbCategory.Text == "")
            {
                try
                {
                    BrandSearch();
                }
                catch // if there is no valid brand names this warning msg will show 
                {
                    MessageBox.Show("Please Select Given Items to Search", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            //This statement will work for category search
            else if (CmbCategory.Text != "" && CmbBrand.Text == "")
            {
                try
                {
                    CategorySearch();
                }
                catch // if there is no valid category this warning msg will show
                {
                    MessageBox.Show("Please Select Given Items to Search", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            //This following statement is inform to select items
            else
            {
                MessageBox.Show("Please Select one or both fields to search", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ClearFields(); //This function will clear all fields
        }

        private void Home_Load(object sender, EventArgs e)
        {
            //This connection will query valid category from database to combobox
            connection.OpenConnection();
            command = new SqlCommand("SELECT DISTINCT P_Category FROM Product", connection.GetCon());
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                CmbCategory.Items.Add(reader.GetString(0));
            }
            connection.CloseConnection();

            //This connection will query valid brand from database to combobox
            connection.OpenConnection();
            command = new SqlCommand("SELECT DISTINCT P_Brand FROM Product", connection.GetCon());
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                CmbBrand.Items.Add(reader.GetString(0));
            }
            connection.CloseConnection();

            LblUserType.Text = UserName;
            LblDateTime.Text = DateTime.Today.ToShortDateString();
            LblTotal.Text += Totalvalue + " ";
        }

        //This button checkout click will work checkout orders
        private void BtnCheckOut_Click(object sender, EventArgs e)
        {
            CheckOut C = new CheckOut();
            C.UserName = UserName;
            if (UserName != "")
            {
                C.PriceValue = Totalvalue;
            }
            else
            {
                C.PriceValue = 0;
            }
            C.Show();
            this.Hide();
        }

        //This button clear click will work for clear fields and shopping cart
        private void BtnClearCart_Click(object sender, EventArgs e)
        {
            CartView.Rows.Clear();
            CleardbCart();
            Totalvalue = 0;
            LblTotal.Text = string.Empty;
        }

        //This button refrresh click will gets all products from database and clear avaliable fields
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            AllProducts();
            ClearFields();
        }

        private void ProductView_Click(object sender, EventArgs e)
        {
            TextId.Text = ProductView.SelectedRows[0].Cells[0].Value.ToString();
            TxtBrand.Text = ProductView.SelectedRows[0].Cells[1].Value.ToString();
            TxtModel.Text = ProductView.SelectedRows[0].Cells[3].Value.ToString();
            TxtPrice.Text = ProductView.SelectedRows[0].Cells[7].Value.ToString();
        }

        //This button sort click will work to sort the items by price and energy
        private void BtnSort_Click(object sender, EventArgs e)
        {
            //if there is no selected item to sort the below inform msg will show
            if (CmbSort.Text == "")
            {
                MessageBox.Show("Please select data to sort", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //This below statement will work for price and energy sorting
                try
                {
                    if (CmbSort.SelectedItem.Equals("Price"))
                    {
                        PriceSort();
                    }
                    else
                    {
                        EnergySort();
                    }
                }
                catch //if there is no valid items to sort this statement will catch
                {
                    MessageBox.Show("Please select given item to sort", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            ClearFields();
        }

        //This button allproduct click will work to display all items from database
        private void AllProduct_Click(object sender, EventArgs e)
        {
            AllProducts();
        }

        //This function will wrok for category search
        public void CategorySearch()
        {
            connection.OpenConnection();
            command = new SqlCommand("SELECT * FROM Product WHERE P_Category=@val1", connection.GetCon());
            command.Parameters.AddWithValue("@val1", CmbCategory.SelectedItem.ToString());
            reader = command.ExecuteReader();
            ProductView.DataSource = GetProduct(reader);
            connection.CloseConnection();
        }

        //This function will work for brandsearch
        public void BrandSearch()
        {
            connection.OpenConnection();
            command = new SqlCommand("SELECT * FROM Product WHERE P_Brand=@val1", connection.GetCon());
            command.Parameters.AddWithValue("@val1", CmbBrand.SelectedItem.ToString());
            reader = command.ExecuteReader();
            ProductView.DataSource = GetProduct(reader);
            connection.CloseConnection();
        }

        //This function will work for both brand and category search
        public void BrandandCategorySearch()
        {
            connection.OpenConnection();
            command = new SqlCommand("SELECT * FROM Product WHERE P_Brand=@val1 AND P_Category=@val2", connection.GetCon());
            command.Parameters.AddWithValue("@val1", CmbBrand.SelectedItem.ToString());
            command.Parameters.AddWithValue("@val2", CmbCategory.SelectedItem.ToString());
            reader = command.ExecuteReader();
            ProductView.DataSource = GetProduct(reader);
            connection.CloseConnection();
        }

        //This function will work for price sorting process
        public void PriceSort()
        {
            connection.OpenConnection();
            command = new SqlCommand("SELECT * FROM Product ORDER BY P_Price", connection.GetCon());
            reader = command.ExecuteReader();
            ProductView.DataSource = GetProduct(reader);
            connection.CloseConnection();
        }

        //This function will work for energy sorting process
        public void EnergySort()
        {
            connection.OpenConnection();
            command = new SqlCommand("SELECT * FROM Product ORDER BY P_PowerUse", connection.GetCon());
            reader = command.ExecuteReader();
            ProductView.DataSource = GetProduct(reader);
            connection.CloseConnection();
        }

        //This function will return list with objects
        private List<Product> GetProduct(SqlDataReader reader)
        {
            List<Product> lis = new List<Product>();

            while (reader.Read())
            {
                lis.Add(new Product { ID = reader.GetInt32(0), Brand = reader.GetString(1), Category = reader.GetString(2), Model = reader.GetString(3), Power = reader.GetString(4), Color = reader.GetString(5), Size = reader.GetString(6), Price = reader.GetInt32(7) });
            }

            return lis;
        }

        //This function work for adding items to database
        public void AddtodbCart()
        {
            string query = "INSERT INTO ProductList (Brand,Model,Price,Quantity,Duration) VALUES " + "(@Brand,@Model,@Price,@Quantity,@Duration)";
            command = new SqlCommand(query, connection.GetCon());
            command.Parameters.AddWithValue("@Brand", TxtBrand.Text);
            command.Parameters.AddWithValue("@Model", TxtModel.Text);
            command.Parameters.AddWithValue("@Price", int.Parse(TxtPrice.Text));
            command.Parameters.AddWithValue("@Quantity", int.Parse(TxtQty.Text));
            command.Parameters.AddWithValue("@Duration", int.Parse(CmbDuration.SelectedItem.ToString()));
            connection.OpenConnection();
            command.ExecuteNonQuery();
            connection.CloseConnection();
            MessageBox.Show("Product inserted to Cart", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //This function is work for cleaning database item 
        public void CleardbCart()
        {
            string query = "DELETE FROM ProductList";
            command = new SqlCommand(query, connection.GetCon());
            connection.OpenConnection();
            command.ExecuteNonQuery();
            connection.CloseConnection();
        }

        //This function will clear all avaliable fields
        public void ClearFields()
        {
            TextId.Text = string.Empty;
            TxtBrand.Text = string.Empty;
            TxtPrice.Text = string.Empty;
            TxtQty.Text = string.Empty;
            TxtModel.Text = string.Empty;
            CmbDuration.Text = string.Empty;
            CmbSort.Text = string.Empty;
            CmbBrand.Text = string.Empty;
            CmbCategory.Text = string.Empty;
        }

        //This function will gets all products from database
        public void AllProducts()
        {
            connection.OpenConnection();
            command = new SqlCommand("SELECT * FROM Product", connection.GetCon());
            reader = command.ExecuteReader();
            ProductView.DataSource = GetProduct(reader);
            connection.CloseConnection();
        }
    }
}
