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
    public partial class ViewUserGroups : Form
    {
        private readonly List<string> UserGroups;

        public ViewUserGroups(List<string> groups, string username)
        {
            UserGroups = groups;
            InitializeComponent();
            this.Text = "Security groups for: " + username;
            Setup();
        }

        private void Setup()
        {
            UserGroups.Sort();
            SecurityGroupsListBox.DataSource = UserGroups;
            SecurityGroupsListBox.Refresh();
        }

        private void OnCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
