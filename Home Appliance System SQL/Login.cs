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
    public partial class Login : Form
    {
        DBConnection connection = new DBConnection();
        SqlCommand command;
        SqlDataReader reader;

        int Attempts, Count;

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            //initialize login attempts
            Attempts = 1;
        }

        //This below exit button click is to exit application
        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to exit application", "Exit Meassage", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        //This following statement is to go sign up page to signup
        private void LblSignup_Click(object sender, EventArgs e)
        {
            Register R = new Register();
            R.Show();
            this.Hide();
        }

        //This following login button click block of code will work for login process
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (UserType.Text != "")
            {
                try
                {
                    if (UserName.Text == "" || Password.Text == "" || UserType.Text == "")
                    {
                        MessageBox.Show("Please Provide Role, Username And Password", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    //This below block of else if statemnet is for Admin account with correct credentials
                    else if (UserType.Text == "Admin") 
                    {
                        if (UserName.Text == "Admin" && Password.Text == "Admin123")
                        {
                            Admin a = new Admin();
                            a.UserType = UserName.Text;
                            a.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("User Role And Password Mismatch!", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    //This below statement will work for customer account with correct credentials 
                    else
                    {
                        connection.OpenConnection();
                        command = new SqlCommand("SELECT * from User_Account WHERE C_Username=@User AND C_Password=@Pwd", connection.GetCon());
                        command.Parameters.AddWithValue("@User", UserName.Text);
                        command.Parameters.AddWithValue("@Pwd", Password.Text);
                        reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Home H = new Home();
                            H.UserName = UserName.Text;
                            H.Show();
                            this.Hide();
                        }
                        //This else statement will work when wrong credentials for customer login
                        else
                        {
                            MessageBox.Show("Please Try again! no. of attempts " + Attempts, "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            UserType.Text = "";
                            UserName.Clear();
                            Password.Clear();
                            UserName.Focus();
                            Attempts += 1;
                            AttemptCount();
                        }
                        connection.CloseConnection();
                    }
                }
                catch // IF something wrong with login procees this inform messaage will show
                {
                    MessageBox.Show("You are getting errors in login process please select correct way", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please Provide Exact Data In Each Fields", "Inform Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //This timer will set count to rest 
        private void LoginTimer_Tick(object sender, EventArgs e)
        {
            if (Count == 0)
            {
                LoginTimer.Enabled = false;
                UserName.Enabled = true;
                Password.Enabled = true;
                BtnLogin.Enabled = true;
                BtnLogin.Text = "LOGIN";
                UserName.Focus();
            }
            else
            {
                BtnLogin.Text = "Login " + Count;
                Count -= 1;
            }
        }

        //This function count the login attempts and rest login attempts
        public void AttemptCount()
        {
            if (Attempts == 4)
            {
                Attempts = 0;
                Count = 20;

                LoginTimer.Enabled = true;
                UserName.Enabled = false;
                Password.Enabled = false;
                BtnLogin.Enabled = false;
            }
        }
    }
}
