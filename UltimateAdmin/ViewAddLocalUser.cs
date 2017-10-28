using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UltimateAdmin.Core.ActiveDirectory;
using UltimateAdmin.Core.Network;
using UltimateAdmin.Presentation;

namespace UltimateAdmin
{
    public partial class ViewAddLocalUser : Form
    {
        private IViewLocalUsersPresenter _viewLocalUsersPresenter;
        private IApplicationController _appController;
        private Computer computer;
        private LocalGroupType groupType;
        private string groupName;
        private User selectedUser;
        public User SelectedUser
        {
            get
            {
                return selectedUser;
            }
        }

        public ViewAddLocalUser(IViewLocalUsersPresenter viewlocaluserspresenter, IApplicationController appController)
        {
            _viewLocalUsersPresenter = viewlocaluserspresenter;
            _appController = appController;
            groupType = _viewLocalUsersPresenter.GroupType;
            computer = _viewLocalUsersPresenter.Computer;
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
            SetTitleText();
        }

        private void SetTitleText()
        {
            if (groupType == LocalGroupType.ADMINS)
            {
                groupName = "Local Administrators";
                this.Text = "Add Local Administrator";
            }
            if (groupType == LocalGroupType.RDPUSERS)
            {
                groupName = "Remote Desktop Users";
                this.Text = "Add Remote Desktop User";
            }
        }

        private dynamic CheckADForUser()
        {
            List<User> usersResult = ADQuery.QueryUserAD(userNameBox.Text, UserField.Username);
            if(usersResult != null)
            {
                if (usersResult.Count() == 1)
                {
                    if (usersResult[0].Username.Equals(userNameBox.Text))
                    {
                        selectedUser = usersResult[0];
                        return selectedUser;
                    }
                    else
                    {
                        return usersResult;
                    }
                }
                if (usersResult.Count() > 1)
                {
                    return usersResult;
                }
            }
            return null;
        }

        private void FindAndSetUser()
        {
            if (userNameBox.Text.Length < 3)
            {
                MessageBox.Show("Please add more than two characters for the search.");
                return;
            }
            this.UseWaitCursor = true;
            this.Cursor = Cursors.WaitCursor;
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            var searchResult = CheckADForUser();
            if (searchResult != null)
            {
                Type returnType = searchResult.GetType();
                if (returnType == typeof(User))
                {
                    selectedUser = (User)searchResult;
                    this.DialogResult = DialogResult.OK;
                    this.UseWaitCursor = false;
                    this.Cursor = Cursors.Default;
                    Cursor.Current = Cursors.Default;
                    this.Close();
                }
                if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    using (var returnedResults = new ViewAddUserSearchResults((List<User>)searchResult))
                    {
                        var formResult = returnedResults.ShowDialog(this);
                        this.UseWaitCursor = false;
                        this.Cursor = Cursors.Default;
                        Cursor.Current = Cursors.Default;
                        if (formResult == DialogResult.OK)
                        {
                            selectedUser = returnedResults.SelectedUser;
                            userNameBox.Text = selectedUser.Username;
                            return;
                        }
                    }
                }
                this.UseWaitCursor = false;
                this.Cursor = Cursors.Default;
                Cursor.Current = Cursors.Default;
            }
            else
            {
                MessageBox.Show("No results found in Directory for user: " + userNameBox.Text);
            }
            this.UseWaitCursor = false;
            this.Cursor = Cursors.Default;
            Cursor.Current = Cursors.Default;
        }

        private bool AddUserToGroup()
        {
            bool userAdded = false;
            if (groupType == LocalGroupType.ADMINS)
            {
                userAdded = computer.AddLocalAdmin(SelectedUser.Username);
            }
            if (groupType == LocalGroupType.RDPUSERS)
            {
                userAdded = computer.AddRDPUser(SelectedUser.Username);
            }
            return userAdded;
        }

        private void OnOKClick(object sender, EventArgs e)
        {
            FindAndSetUser();
            if (SelectedUser != null)
            {
                if (AddUserToGroup())
                {
                    _viewLocalUsersPresenter.RefreshUsers();
                    MessageBox.Show("User: " + SelectedUser.Username + " successfully added to group:\n" + groupName);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error!  User not added to group:\n" + groupName);
                }
            }
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnUserNameFieldKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                OKButton.PerformClick();
            }
        }
    }
}
