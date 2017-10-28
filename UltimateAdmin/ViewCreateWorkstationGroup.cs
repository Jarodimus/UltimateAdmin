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
    public partial class ViewCreateWorkstationGroup : Form
    {
        private string groupName;

        public string GroupName
        {
            get
            {
                return groupName;
            }
        }

        public ViewCreateWorkstationGroup()
        {
            InitializeComponent();
        }

        private void OnOKClick(object sender, EventArgs e)
        {
            if(aliasNameBox.Text != "")
            {
                groupName = aliasNameBox.Text;
                bool newGroupAdded = ADContext.AddWorkstationGroup(groupName);
                if (newGroupAdded)
                {
                    MessageBox.Show("Group: " + groupName + " has been created.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Group: " + aliasNameBox.Text + " already exists.");
                }
            }
            else
            {
                MessageBox.Show("Please enter a group name.");
            }
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
