using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UltimateAdmin.Core.Network;

namespace UltimateAdmin
{
    public partial class DemoViewAddLocalUser : Form
    {
        DemoViewLocalUsers _demoLocalUsersView;
        LocalGroupType groupType;
        public DemoViewAddLocalUser(DemoViewLocalUsers demoLocalUsersView, LocalGroupType groupType, string computerName)
        {
            _demoLocalUsersView = demoLocalUsersView;
            this.groupType = groupType;
            InitializeComponent();
        }

        private void OnOKClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(userNameBox.Text))
            {
                _demoLocalUsersView.LocalUsers.Add(userNameBox.Text);
                _demoLocalUsersView.RefreshListBox();
                MessageBox.Show("User " + userNameBox.Text + " has been added to the group:\n" + GetGroup());
                this.Close();
            }
        }

        private string GetGroup()
        {
            switch (groupType)
            {
                case LocalGroupType.ADMINS:
                    return "Local Administrators";
                case LocalGroupType.RDPUSERS:
                    return "Remote Desktop Users";
                default:
                    return "";
            }
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
