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

namespace UltimateAdmin
{
    public partial class ViewAddUserSearchResults : Form
    {
        private BindingSource listBinding;
        private List<User> _userSearchResults;
        private User _selectedUser;
        public User SelectedUser
        {
            get
            {
                return _selectedUser;
            }
        }


        public ViewAddUserSearchResults(List<User> userSearchResults)
        {
            _userSearchResults = userSearchResults;
            _userSearchResults.Sort();
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
            Setup();
        }

        public void Setup()
        {
            listBinding = new BindingSource(_userSearchResults, null);
            userResultGridView.MultiSelect = false;
            userResultGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            userResultGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            userResultGridView.RowHeadersVisible = false;
            userResultGridView.AutoGenerateColumns = false;
            userResultGridView.AllowUserToAddRows = false;
            userResultGridView.DataSource = listBinding;

            DataGridViewColumn userNameColumn = new DataGridViewTextBoxColumn();
            userNameColumn.DataPropertyName = "UserName";
            userNameColumn.HeaderText = "Username";
            userResultGridView.Columns.Add(userNameColumn);

            DataGridViewColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.DataPropertyName = "Name";
            nameColumn.HeaderText = "Name";
            userResultGridView.Columns.Add(nameColumn);

            DataGridViewColumn titleColumn = new DataGridViewTextBoxColumn();
            titleColumn.DataPropertyName = "Title";
            titleColumn.HeaderText = "Title";
            userResultGridView.Columns.Add(titleColumn);
        }

        private void SetSelectedUser()
        {
            var row = userResultGridView.SelectedRows[0];
            _selectedUser = (User)row.DataBoundItem;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OnUserDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SetSelectedUser();
        }

        private void OnOKClick(object sender, EventArgs e)
        {
            if(userResultGridView.SelectedRows.Count < 1)
            {
                return;
            } else
            {
                SetSelectedUser();
            }
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
