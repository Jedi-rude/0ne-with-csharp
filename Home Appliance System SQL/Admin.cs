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
    public partial class Admin : Form
    {
        DBConnection connection = new DBConnection();

        List<Product> CP = new List<Product>();
        Product Product = new Product();

        SqlDataReader reader;
        SqlCommand command;

        public string UserType { get; set; }

        public Admin()
        {
            InitializeComponent();
        }

        private void Admin_Load(object sender, EventArgs e)
        {
          
            lbldate.Text = DateTime.Today.ToShortDateString();
            LblUserType.Text = UserType.ToString();
        }

        //This button exit click will work to stop the application
        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to exit application", "Exit Meassage", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        //This logoutimg work for going back to login page
        private void LogOutImg_Click(object sender, EventArgs e)
        {
            Login L = new Login();
            L.Show();
            this.Hide();
        }

        //This button search will work for searching products from database calling functions
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (CmbBrand.Text != "" && CmbAppliance.Text != "")
            {
                if (CmbBrand.Text != "" || CmbAppliance.Text != "")
                {
                    try
                    {
                        BrandandCategorySearch();
                    }
                    catch //if there is no correct process for searching product this statement will catch
                    {
                        MessageBox.Show("Please Select Given Items to Search", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }                   
                }
            }

            else if (CmbBrand.Text != "" && CmbCategory.Text == "")
            {
                try
                {
                    BrandSearch();
                }
                catch //if there is no correct process for searching product this statement will catch
                {
                    MessageBox.Show("Please Select Given Items to Search", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }                
            }

            else if (CmbCategory.Text != "" && CmbBrand.Text == "")
            {
                try
                {
                    CategorySearch();
                }
                catch //if there is no correct process for searching product this statement will catch
                {
                    MessageBox.Show("Please Select Given Items to Search", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }                
            }

            else
            {
                MessageBox.Show("Please Select one or both fields to search", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ClearFields();
        }

        //This button add click will work for adding new items to database
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (TxtPBrand.Text != "" || TxtPColor.Text != "" || CmbCategory.Text != "" || TxtPowerUsage.Text != "" || TxtPPrice.Text != "" || TxtPsize.Text != "")
            {
                try
                {
                    AddToProduct();
                    AllProducts(); //after successfully added all of the products will show
                    ClearFields();                
                }
                catch //if there is no correct process for adding product this statement will catch
                {
                    MessageBox.Show("Please Select given item and correctly choose", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Enter Data Correctly in Each Fields", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void AdminView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            TxtPid.Text = AdminView.CurrentRow.Cells[0].Value.ToString();
            TxtPBrand.Text = AdminView.CurrentRow.Cells[1].Value.ToString();
            CmbCategory.Text = AdminView.CurrentRow.Cells[2].Value.ToString();
            TxtPowerUsage.Text = AdminView.CurrentRow.Cells[3].Value.ToString();
            TxtPColor.Text = AdminView.CurrentRow.Cells[4].Value.ToString();
            TxtPsize.Text = AdminView.CurrentRow.Cells[5].Value.ToString();
            TxtPPrice.Text = AdminView.CurrentRow.Cells[6].Value.ToString();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        //This button delete click will work for deleting product from database
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (TxtPid.Text != "")
            {
                try
                {
                    DeleteProduct();
                    AllProducts(); //after successfully deleted all of the products will show
                    ClearFields();
                }
                catch //if there is no correct process for deleting product this statement will catch
                {
                    MessageBox.Show("Please Select ID to Delete", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please Select Row to Delete", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //This button update click will work for updating process of product
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (TxtPBrand.Text != "" || TxtPColor.Text != "" || CmbCategory.SelectedItem.ToString() != "" || TxtPowerUsage.Text != "" || TxtPPrice.Text != "" || TxtPsize.Text != "")
            {
                try
                {
                    UpdateProduct();
                    AllProducts(); //after successfully updated all of the products will show
                    ClearFields();
                }
                catch //if there is no correct process for updating product this statement will catch
                {
                    MessageBox.Show("Please select and correct update", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please Enter Data in Each Fields", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //This button refresh click will show all products and clearfields
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            AllProducts();
            ClearFields();
        }

        //This picturebox click will go back to login page
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Login L = new Login();
            L.Show();
            this.Hide();
        }

        private List<Product> GetProduct(SqlDataReader reader)
        {
            List<Product> lis = new List<Product>();

            while (reader.Read())
            {
                lis.Add(new Product { ID = reader.GetInt32(0), Brand = reader.GetString(1), Category = reader.GetString(2), Power = reader.GetString(3), Color = reader.GetString(4), Size = reader.GetString(5), Price = reader.GetInt32(6) });
            }
            return lis;
        }

        //This function will query all products from database
        public void AllProducts()
        {
            connection.OpenConnection();
            command = new SqlCommand("SELECT * FROM Product", connection.GetCon());
            reader = command.ExecuteReader();
            AdminView.DataSource = GetProduct(reader);
            connection.CloseConnection();
        }

        //This function will perform brand and category search process
        public void BrandandCategorySearch()
        {
            connection.OpenConnection();
            command = new SqlCommand("select * from Product where P_Brand=@val1 and P_Category=@val2", connection.GetCon());
            command.Parameters.AddWithValue("@val1", CmbBrand.SelectedItem.ToString());
            command.Parameters.AddWithValue("@val2", CmbAppliance.SelectedItem.ToString());
            reader = command.ExecuteReader();
            AdminView.DataSource = GetProduct(reader);
            connection.CloseConnection();
        }

        //This function will perform category search process
        public void CategorySearch()
        {
            connection.OpenConnection();
            command = new SqlCommand("SELECT * FROM Product WHERE P_Category=@val1", connection.GetCon());
            command.Parameters.AddWithValue("@val1", CmbCategory.SelectedItem.ToString());
            reader = command.ExecuteReader();
            AdminView.DataSource = GetProduct(reader);
            connection.CloseConnection();
        }

        //This function will perform brand search process
        public void BrandSearch()
        {
            connection.OpenConnection();
            command = new SqlCommand("SELECT * FROM Product WHERE P_Brand=@val1", connection.GetCon());
            command.Parameters.AddWithValue("@val1", CmbBrand.SelectedItem.ToString());
            reader = command.ExecuteReader();
            AdminView.DataSource = GetProduct(reader);
            connection.CloseConnection();
        }

        //This function will clear all avaliable fields 
        public void ClearFields()
        {
            TxtPid.Text = string.Empty;
            TxtPBrand.Text = string.Empty;
            CmbCategory.Text = string.Empty;
            TxtPowerUsage.Text = string.Empty;
            TxtPColor.Text = string.Empty;
            TxtPsize.Text = string.Empty;
            TxtPPrice.Text = string.Empty;
            TxtModel.Text = string.Empty;
            CmbBrand.Text = string.Empty;
            CmbAppliance.Text = string.Empty;
        }

        //This function perform addning new product to the database
        public void AddToProduct()
        {
            string query = "INSERT INTO Product (P_Brand,P_Category,P_Model,P_PowerUse,P_Color,P_Size,P_Price) VALUES " + "(@P_Brand,@P_Cat,@P_Model,@P_Power,@P_Color,@P_Size,@P_Price)";
            command = new SqlCommand(query, connection.GetCon());
            command.Parameters.AddWithValue("@P_Brand", TxtPBrand.Text);
            command.Parameters.AddWithValue("@P_Cat", CmbCategory.SelectedItem.ToString());
            command.Parameters.AddWithValue("@P_Model", TxtModel.Text);
            command.Parameters.AddWithValue("@P_Power", TxtPowerUsage.Text);
            command.Parameters.AddWithValue("@P_Color", TxtPColor.Text);
            command.Parameters.AddWithValue("@P_Size", TxtPsize.Text);
            command.Parameters.AddWithValue("@P_Price", int.Parse(TxtPPrice.Text));
            connection.OpenConnection();
            command.ExecuteNonQuery();
            connection.CloseConnection();
            MessageBox.Show("Product inserted", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //This function will perform delete operation of items from database
        public void DeleteProduct()
        {
            string query = "DELETE FROM Product WHERE P_ID=@ID";
            command = new SqlCommand(query, connection.GetCon());
            command.Parameters.AddWithValue("@ID", Convert.ToInt32(TxtPid.Text));
            connection.OpenConnection();
            command.ExecuteNonQuery();
            connection.CloseConnection();
            MessageBox.Show("Product Deleted", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //This function will perform update operation of the items from database
        public void UpdateProduct()
        {
            string query = "UPDATE Product " + " SET P_Brand=@P_Brand, P_Category=@P_Cat, P_Model=@P_Model, P_PowerUse=@P_Power, P_Color=@P_Color, P_Size=@P_Size, P_Price=@P_Price" + " WHERE P_ID=@P_ID";
            command = new SqlCommand(query, connection.GetCon());
            command.Parameters.AddWithValue("@P_Brand", TxtPBrand.Text);
            command.Parameters.AddWithValue("@P_Cat", CmbCategory.SelectedItem.ToString());
            command.Parameters.AddWithValue("@P_Model", TxtModel.Text);
            command.Parameters.AddWithValue("@P_Power", TxtPowerUsage.Text);
            command.Parameters.AddWithValue("@P_Color", TxtPColor.Text);
            command.Parameters.AddWithValue("@P_Size", TxtPsize.Text);
            command.Parameters.AddWithValue("@P_Price", int.Parse(TxtPPrice.Text));
            command.Parameters.AddWithValue("@P_ID", int.Parse(TxtPid.Text));
            connection.OpenConnection();
            command.ExecuteNonQuery();
            connection.CloseConnection();
            MessageBox.Show("Product updated", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
