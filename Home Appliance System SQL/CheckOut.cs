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
    public partial class CheckOut : Form
    {
        DBConnection connection = new DBConnection();
        SqlCommand command;
        SqlDataReader reader;
        
        public string UserName { get; set; }
        public int PriceValue { get; set; }

        public CheckOut()
        {
            InitializeComponent();
        }

        private void CheckOut_Load(object sender, EventArgs e)
        {
            GetProduct();
            LblDate.Text = DateTime.Today.ToShortDateString();
            LblTotal.Text = PriceValue.ToString();
            LblUsername.Text = UserName;
        }

        //This button back click will work for go back to 
        private void BtnBack_Click(object sender, EventArgs e)
        {
            Home H = new Home();
            H.UserName = UserName;
            if (H.UserName != "")
            {
                H.Totalvalue = PriceValue;
                H.Show();
                this.Hide();
            }
            else
            {
                H.Show();
                this.Hide();
                ClearProductlist();
            }                    
        }

        // This button pay click will perform payment process
        private void BtnPay_Click(object sender, EventArgs e)
        {
            if (textAddress.Text != "" && textphno.Text != "" && textzipcode.Text != "" && radiogpay.Text != "" && radiopaypal.Text != "" && radiovisa.Text != "")
            {
                //If there is no amount to pay this remind message will show
                if (PriceValue == 0)
                {
                    MessageBox.Show("You have no purchaselist for current! Please Make Order", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //There is payment process to pay below code will work
                else
                {
                    ClearProductlist();
                    PriceValue = 0;
                    LblTotal.Text = "";
                    MessageBox.Show("Thanks For Purchasing! Please make another order", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                }
            }
            else
            {
                MessageBox.Show("Please Fill Buyer Inoformation", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //This button exit click will perform exit process
        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure want to exit application", "Exit Meassage", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private List<CartProduct> GetProduct(SqlDataReader reader)
        {
            List<CartProduct> lis = new List<CartProduct>();

            while (reader.Read())
            {
                lis.Add(new CartProduct { ID = reader.GetInt32(0), Brand = reader.GetString(1), Model = reader.GetString(2), Price = reader.GetInt32(3), Quantity = reader.GetInt32(4), Duration = reader.GetInt32(5) });
            }

            return lis;
        }

        //This function will get all products from productlist database
        public void GetProduct()
        {
            connection.OpenConnection();
            command = new SqlCommand("SELECT * FROM ProductList", connection.GetCon());
            reader = command.ExecuteReader();
            CheckOutList.DataSource = GetProduct(reader);
            connection.CloseConnection();
        }

        //This function will clear all fields 
        public void ClearFields()
        {
            textAddress.Text = string.Empty;
            textphno.Text = string.Empty;
            textzipcode.Text = string.Empty;
            radiogpay.Checked = false;
            radiopaypal.Checked = false;
            radiovisa.Checked = false;
        }

        //This function will wrok for clear product list from database
        public void ClearProductlist()
        {
            string query = "DELETE FROM Productlist";
            command = new SqlCommand(query, connection.GetCon());
            connection.OpenConnection();
            command.ExecuteNonQuery();
            connection.CloseConnection();
            GetProduct();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
