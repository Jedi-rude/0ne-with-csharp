using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Home_Appliance_System_SQL
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            registerlbl.Text = "Username must Contain only numbers and characters, \nDo not allow for special characters.\nPassword must contain at least one upper case and one lower case and \nPassword length is betwween 8 and 16 characters. \nPassword field and confirm password field must be same!";
            lbllogin.Text = "First, User Type require to choose, depends on user type username \nand password will need to enter. \nIf incorrect login credentials on customer user type, \nThe login attempts will count.";
            lblshop.Text = "First, Prodcut row need to select and enter month and quantity \nAnd check produtcs and make payments. Other functions like sorting, \nSearching items can do by selecting type and enter button.";
        }

        private void Btnback_Click(object sender, EventArgs e)
        {
            Register R = new Register();
            R.Show();
            this.Hide();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to exit application", "Exit Meassage", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void LblName_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
