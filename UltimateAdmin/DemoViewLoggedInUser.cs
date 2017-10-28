using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UltimateAdmin
{
    public partial class DemoViewLoggedInUser : Form
    {
        public DemoViewLoggedInUser(string userName, string computerName)
        {
            InitializeComponent();
            userNameLabel.Text = userName;
            this.Text += " " + computerName;
        }

        private void OnOKClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
