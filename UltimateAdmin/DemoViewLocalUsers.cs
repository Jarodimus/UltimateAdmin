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
    public partial class DemoViewLocalUsers : Form
    {
        private LocalGroupType _groupType;
        public LocalGroupType GroupType
        {
            get
            {
                return _groupType;
            }
        }
        private List<string> localUsers;
        public List<string> LocalUsers
        {
            get
            {
                return localUsers;
            }
        }

        string computerName;

        public DemoViewLocalUsers(string computerName, LocalGroupType groupType)
        {
            _groupType = groupType;
            localUsers = new List<string>();
            this.computerName = computerName;
            InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            GetRemoteGroupUsers();
            SetTitleText();
        }

        private void GetRemoteGroupUsers()
        {
            if (_groupType == LocalGroupType.ADMINS)
            {
                localUsers = new List<string>
                {
                    "Adminstrator",
                    "jsmith",
                };
                localUsersLabel.Text = "Local Administrators";
            }
            if (_groupType == LocalGroupType.RDPUSERS)
            {
                localUsers = new List<string>
                {
                    "jsmith",
                };
                localUsersLabel.Text = "Remote Desktop Users";
            }
            RefreshListBox();
        }

        private void SetTitleText()
        {
            this.Text = computerName;
        }

        public void RefreshListBox()
        {
            UsersListBox.DataSource = null;
            UsersListBox.DataSource = localUsers;
            UsersListBox.Refresh();
        }

        private void OnCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RemoveLocalUser(object sender, EventArgs e)
        {
            string selectedUser;
            if (UsersListBox.SelectedItems.Count > 0)
            {
                selectedUser = (string)UsersListBox.SelectedItem;
                localUsers.Remove(selectedUser);
                RefreshListBox();
            }
        }

        private void AddLocalUser(object sender, EventArgs e)
        {
            DemoViewAddLocalUser viewAddLocalUser = new DemoViewAddLocalUser(this, _groupType, computerName);
            viewAddLocalUser.Show();
        }
    }
}
