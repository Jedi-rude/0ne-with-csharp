using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace Home_Appliance_System_SQL
{
    public partial class Register : Form
    {
        DBConnection connection = new DBConnection();

        SqlCommand command;
        Regex check;

        public Register()
        {
            InitializeComponent();
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }

        //This button back click will go back to login form
        private void BtnBack_Click(object sender, EventArgs e)
        {
            Login L = new Login();
            L.Show();
            this.Hide();
        }

        //This button exit click will perform exit process
        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to exit application", "Exit Meassage", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        //This button register click will check the validation requirements of registration
        private void BtnReg_Click(object sender, EventArgs e)
        {
            if (txtname.Text != "" && txtuser.Text != "" && txtcpwd.Text != "" && txtpwd.Text != "")
            {
                //There is valid to create new account the follwing block of code will work
                try
                {
                    if (ValidPwd(txtpwd.Text) == true && ValidUsername(txtuser.Text) == true)
                    {
                        RegisterAccount();
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("Please Enter Data Format in Correctly \nPlease Click About to see data format", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields();
                    }

                }
                catch //If there is no valid credentails for registration this statement will catch
                {
                    MessageBox.Show("Please Correctly Register Your Credentials", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please Provide Data In Each Fields", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtcpwd_Leave(object sender, EventArgs e)
        {

        }

        //This txtpwd leave event  will check strong password or weak password 
        private void txtpwd_Leave(object sender, EventArgs e)
        {
            if (txtpwd.Text == "")
            {
                lblpwdtxt.Text = "";
            }
            else
            {
                if (ValidPwd(txtpwd.Text))
                {
                    lblpwdtxt.ForeColor = Color.Green;
                    lblpwdtxt.Text = "Strong Password";
                    BtnReg.Enabled = true;
                }
                else
                {
                    lblpwdtxt.ForeColor = Color.Red;
                    lblpwdtxt.Text = "Weak Password";
                    BtnReg.Enabled = false;
                }
            }
        }

        //This button about click will work for going about page
        private void BtnAbout_Click(object sender, EventArgs e)
        {
            About A = new About();
            A.Show();
            this.Hide();
        }

        private void txtcpwd_Validating(object sender, CancelEventArgs e)
        {

        }

        //This txtcpwd textchanged will work for password and confirm password validation
        private void txtcpwd_TextChanged(object sender, EventArgs e)
        {
            if (txtpwd.Text == txtcpwd.Text)
            {
                lblcpwd.ForeColor = Color.Green;
                lblcpwd.Text = "Password Correct";
                BtnReg.Enabled = true;
            }
            else
            {
                lblcpwd.ForeColor = Color.Red;
                lblcpwd.Text = "Password Mismatch";
                BtnReg.Enabled = false;
            }
        }

        //This function will clear avaliable fields
        public void ClearFields()
        {
            txtname.Clear();
            txtuser.Clear();
            txtpwd.Clear();
            txtcpwd.Clear();
            lblpwdtxt.Text = string.Empty;
            lblcpwd.Text = string.Empty;
        }

        //This function will work for useraccount creating process
        public void RegisterAccount()
        {
            string query = "INSERT INTO User_Account (C_Name,C_Username,C_Password) VALUES " + "(@C_name,@C_user,@C_pwd)";
            command = new SqlCommand(query, connection.GetCon());
            command.Parameters.AddWithValue("@C_name", txtname.Text);
            command.Parameters.AddWithValue("@C_user", txtuser.Text);
            command.Parameters.AddWithValue("@C_pwd", txtpwd.Text);
            connection.OpenConnection();
            command.ExecuteNonQuery();
            connection.CloseConnection();
            MessageBox.Show("Registration Success Now You can Login to your account", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //This function validate the password field and return boolean
        private bool ValidPwd(string P)
        {
            if (P.Length >= 8 && P.Length <= 16)
            {
                char[] pwd = P.ToCharArray();
                for (int i = 0; i < pwd.Length; i++)
                {

                    if (P[i] >= 'a' && P[i] <= 'z')
                    {
                        return true;
                    }
                    if (P[i] >= 'A' && P[i] <= 'Z')
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //This function will work for username return boolean
        public bool ValidUsername(string n)
        {
            check = new Regex("^[a-zA-Z0-9]*$");
            bool valid = false;
            valid = check.IsMatch(n);
            if (valid == true)
            {
                return valid;
            }
            else
            {
                MessageBox.Show("Username Format is Characters and numbers", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return valid;
            }
        }
    }
}
