using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UltimateAdmin.Core.ActiveDirectory;
using UltimateAdmin.Presentation;

namespace UltimateAdmin
{
    public partial class ViewConfiguration : Form, IViewConfiguration
    {
        private IApplicationController _appController;
        public IApplicationController AppController
        {
            get
            {
                return _appController;
            }
            set
            {
                if (_appController == null)
                {
                    _appController = value;
                }
            }
        }

        private List<string> _workstationGroups = new List<string>();
        BindingSource workstationGroupsListBinding = new BindingSource();

        private List<string> _workstationOUs = new List<string>();
        BindingSource workstationOUsListBinding = new BindingSource();

        private string WorkstationGroupSelected
        {
            get
            {
                return (string)workstationGroupsComboBox.SelectedItem;
            }
        }

        private string WorkstationOUSelected
        {
            get
            {
                return (string)workstationOUComboBox.SelectedItem;
            }
        }

        public ViewConfiguration(IApplicationController appController)
        {
            _appController = appController;
            InitializeComponent();
            SetValues();
            Setup();
        }

        private void Setup()
        {
            workstationGroupsComboBox.DataSource = _workstationGroups;
            workstationOUComboBox.DataSource = _workstationOUs;
        }

        private void SetValues()
        {
            domainValueBox.Text = ADContext.DomainSimpleName;
            ldapValueBox.Text = ADContext.LDAP_Path;
            SetWorkstationData();
        }

        private void SetWorkstationData()
        {
            _workstationGroups = ADContext.GetWorkstationGroups();
            workstationGroupsComboBox.DataSource = _workstationGroups;
            if(_workstationGroups.Count > 0)
            {
                SetWorkstationOUs();
            }
        }

        private void SetWorkstationOUs()
        {
            if (string.IsNullOrEmpty(WorkstationGroupSelected))
            {
                return;
            }
            else
            {
                workstationOUComboBox.DataSource = null;
                _workstationOUs = ADContext.GetWorkstationGroupOUs(WorkstationGroupSelected);
                workstationOUComboBox.DataSource = _workstationOUs;
                workstationOUComboBox.Refresh();
            }
        }


        private void OnSelectedWorkstationGroupIndexChange(object sender, EventArgs e)
        {
            SetWorkstationOUs();
        }

        private void UpdateWorkstationData()
        {
            workstationGroupsComboBox.DataSource = null;
            workstationOUComboBox.DataSource = null;
            SetWorkstationData();
            ADContext.SaveAllSettings();
        }

        private void OnWorkstationsAddOUClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(WorkstationGroupSelected))
            {
                DialogResult result = _appController.RunViewAddOU(this, WorkstationGroupSelected);
                if (result == DialogResult.OK)
                {
                    //SetOUStrings();
                    //UpdateUserManagerOUs();
                    //userOUComboBox.SelectedIndex = 0;
                }
            }
            else
            {
                MessageBox.Show("Please select a Workstation Group to add to.", "Add Workstation OU");
            }
        }

        private void OnCloseClick(object sender, EventArgs e)
        {
            ADContext.SaveAllSettings();
            this.Close();
        }

        private void OnCreateGroupClick(object sender, EventArgs e)
        {
            var count = ADContext.GetWorkstationGroupCount();
            if(count == 6)
            {
                MessageBox.Show("Group limit (6) has been reached.  Please remove or consolidate groups before adding another.");
                return;
            }
            DialogResult addGroupResult = _appController.RunViewAddWorkstationGroup();
            if(addGroupResult == DialogResult.OK)
            {
                UpdateWorkstationData();
            }
        }

        private void OnDeleteGroupClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(WorkstationGroupSelected))
            {
                return;
            }
            else
            {
                DialogResult confirmDeleteGroup = MessageBox.Show("Are you sure you want to delete group: " + WorkstationGroupSelected + "?", "Delete Workstation Group", MessageBoxButtons.YesNoCancel);
                if(confirmDeleteGroup == DialogResult.Yes)
                {
                    DialogResult verifyDelete = MessageBox.Show("Deleting the group will also remove all of the group members (OU references).  Proceed?", "Warning", MessageBoxButtons.YesNoCancel);
                    if(verifyDelete == DialogResult.Yes)
                    {
                        bool isRemoved = ADContext.RemoveWorkstationGroup(WorkstationGroupSelected);
                        if (isRemoved)
                        {
                            MessageBox.Show("Group has been deleted.", "Delete Workstation Group");
                            UpdateWorkstationData();
                        }
                        else
                        {
                            MessageBox.Show("Group has already been removed.", "Error");
                        }
                    }
                }
            }
        }

        private void OnAddWorkstationOUClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(WorkstationGroupSelected))
            {
                MessageBox.Show("Please select a workstation group.", "Add OU to Workstation Group");
                return;
            }
            else
            {
                DialogResult addOUResult = _appController.RunViewAddOU(this, WorkstationGroupSelected);
                if(addOUResult == DialogResult.OK)
                {
                    UpdateWorkstationData();
                }
            }
        }

        private void OnRemoveWorkstationOUClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(WorkstationOUSelected))
            {
                MessageBox.Show("Please select a workstation OU to remove.", "Remove Workstation OU.");
                return;
            }
            else
            {
                DialogResult confirmRemoveResult = MessageBox.Show("Are you sure you want to remove:" + "\n" + WorkstationOUSelected + "\n" + "from group: " + WorkstationGroupSelected + "?", "Confirm Remove OU", MessageBoxButtons.YesNoCancel);
                if(confirmRemoveResult == DialogResult.Yes)
                {
                    bool isRemoved = ADContext.RemoveWorkstationGroupOU(WorkstationGroupSelected, WorkstationOUSelected);
                    if(isRemoved)
                    {
                        MessageBox.Show("OU has been removed from group: " + WorkstationGroupSelected + ".", "OU Removed");
                        UpdateWorkstationData();
                    } 
                    else 
                    {
                        MessageBox.Show("Error removing OU from group.", "Error!");
                    }
                }
                else
                {
                    return;
                }
            }
        }

    }
}
