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
using UltimateAdmin.Presentation;

namespace UltimateAdmin
{
    public partial class ViewAddOU : Form
    {
        private IViewConfiguration viewConfig;

        private bool errorInForm;
        private bool isWorkstationAdd;

        private string ExampleText
        {
            get
            {
                return exampleLabel.Text;
            }
            set
            {
                exampleLabel.Text = value;
            }
        }

        private string Alias
        {
            get
            {
                return enterAliasValue.Text;
            }
        }

        private string OrgUnit
        {
            get
            {
                return ouTextBox.Text;
            }
        }

        private string workstationGroup;

        private string workstationExampleText = "ou=Workstations,ou=west,ou=us,dc=corp,dc=contoso,dc=com";
        private string userExampleText = "ou=Users,ou=east,ou=us,dc=corp,dc=contoso,dc=com";

        public ViewAddOU(IViewConfiguration viewConfig, string workstationGroup)
        {
            isWorkstationAdd = true;
            this.viewConfig = viewConfig;
            this.workstationGroup = workstationGroup;
            InitializeComponent();
            OUGroupValue.Text = workstationGroup;
            HideShowAliasEntry(true);
            HideShowGroupName(false);
            ExampleText = workstationExampleText;
        }

        private void HideShowGroupName(bool hide)
        {
            if (hide)
            {
                OUGroupLabel.Hide();
                OUGroupValue.Hide();
            }
            else
            {
                OUGroupLabel.Show();
                OUGroupValue.Show();
            }
        }

        private void HideShowAliasEntry(bool hide)
        {
            if (hide)
            {
                enterAliasLabel.Hide();
                enterAliasValue.Hide();
            }
            else
            {
                enterAliasLabel.Show();
                enterAliasValue.Show();
            }
        }

        private void OnAddClick(object sender, EventArgs e)
        {
            ValidateForm();
            if (errorInForm)
            {
                return;
            }
            else
            {
                var pathExists = ADInfo.DoesPathExist(OrgUnit);
                if (pathExists)
                {
                    if (isWorkstationAdd)
                    {
                        AddWorkstationOU();
                    }
                }
                else
                {
                    MessageBox.Show("Unable to locate path " + "\n" + OrgUnit + "\n" + "in the Directory for domain " + ADContext.DomainSimpleName + "." + "\n" + "Please enter a valid path.");
                }
            }
        }

        private void AddWorkstationOU()
        {
            bool? isAdded = ADContext.AddWorkstationGroupOU(workstationGroup, OrgUnit);
            if (isAdded == true)
            {
                MessageBox.Show("Workstation OU added to group: " + workstationGroup);
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else if(isAdded == false)
            {
                MessageBox.Show("OU already exists in group: " + workstationGroup, "Error!");
                DialogResult = DialogResult.Cancel;
                return;
            }
            else
            {
                MessageBox.Show("Workstation group does not exist and may have already been deleted.");
                DialogResult = DialogResult.Cancel;
                return;
            }
        }

        //private void OnChangeClick(object sender, EventArgs e)
        //{
        //    ValidateForm();
        //    if (errorInForm)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        ModifyOUString(true);
        //        DialogResult = DialogResult.OK;
        //        this.Close();
        //    }
        //}

        //private void ModifyOUString(bool isChange)
        //{
        //    var enteredPath = ouTextBox.Text;
        //    var pathExists = ADUsersQuery.DoesPathExist(enteredPath);
        //    if (pathExists)
        //    {
        //        string alias = enterAliasValue.Text;
        //        string ouString = ouTextBox.Text;
                
        //        if (isChange)
        //        {
        //            ADContext.ChangeOUString(alias, ouString);
        //            MessageBox.Show("OU Entry has been changed.");
        //        }
        //        else
        //        {
        //            MessageBox.Show("OU has been added.");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Unable to locate path in Active Directory.  Please enter a valid path.");
        //    }
        //}

        private void ValidateForm()
        {
            errorInForm = false;
            StringBuilder validationString = new StringBuilder();
            validationString.Append("Please fix the following errors:\n");
            if (string.IsNullOrEmpty(OrgUnit))
            {
                errorInForm = true;
                validationString.Append("OU path cannot be blank.");
            }
            if (!isWorkstationAdd && string.IsNullOrEmpty(Alias))
            {
                errorInForm = true;
                validationString.Append("OU Alias cannot be blank.");
            }
            if(!isWorkstationAdd && enterAliasValue.Text.Count() > 5)
            {
                errorInForm = true;
                validationString.Append("Alias cannot be more than 5 characters.");
            }
            if (errorInForm)
            {
                MessageBox.Show(validationString.ToString());
            }
        }

        private void OnCloseClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
