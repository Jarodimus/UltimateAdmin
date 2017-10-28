using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UltimateAdmin.Core;
using UltimateAdmin.Core.ActiveDirectory;
using UltimateAdmin.Presentation;

namespace UltimateAdmin
{
    public partial class ViewWorkstationSettings : Form
    {
        private WorkstationSearchPreferences _searchPreferences;
        private IApplicationController _appController;

        public ViewWorkstationSettings(WorkstationSearchPreferences searchPreferences, IApplicationController appController)
        {
            _searchPreferences = searchPreferences;
            _appController = appController;
            InitializeComponent();
            AddGroupCheckBoxes();
            HideCheckBoxes();
            SetWorkstationGroups();
            InitializeUIFromPreferences();
        }

        private void InitializeUIFromPreferences()
        {
            bool searchEntireDir = _searchPreferences.SearchEntireDirectory;
            if (searchEntireDir)
            {
                entireDirectoryCheckBox.Checked = true;
            }
            else
            {
                if(_searchPreferences.SelectedGroups != null)
                {
                    for (int i = 0; i < _searchPreferences.SelectedGroups.Count; i++)
                    {
                        for (int j = 0; j < _groupCheckBoxes.Length; j++)
                        {
                            if (_groupCheckBoxes[j].Text.Equals(_searchPreferences.SelectedGroups[i]))
                            {
                                _groupCheckBoxes[j].Checked = true;
                            }
                        }
                    }
                }
            }
        }

        private void AddGroupCheckBoxes()
        {
            CheckBox[] groupCheckBoxes = new CheckBox[6];
            groupCheckBoxes[0] = checkBox1;
            groupCheckBoxes[1] = checkBox2;
            groupCheckBoxes[2] = checkBox3;
            groupCheckBoxes[3] = checkBox4;
            groupCheckBoxes[4] = checkBox5;
            groupCheckBoxes[5] = checkBox6;
            _groupCheckBoxes = groupCheckBoxes;
        }

        private CheckBox[] _groupCheckBoxes;

        public CheckBox[] GroupCheckBoxes
        {
            get
            {
                return _groupCheckBoxes;
            }
            set
            {
                _groupCheckBoxes = value;
            }
        }

        public List<string> CheckedGroups
        {
            get
            {
                List<string> checkedGroups = new List<string>();
                for (int i = 0; i < _groupCheckBoxes.Length; i++)
                {
                    if (_groupCheckBoxes[i].Checked)
                    {
                        checkedGroups.Add(_groupCheckBoxes[i].Text);
                    }
                }
                return checkedGroups;
            }
        }

        public bool IsGroupCheckBoxChecked
        {
            get
            {
                for (int i = 0; i < _groupCheckBoxes.Length; i++)
                {
                    if (_groupCheckBoxes[i].Checked)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private void HideCheckBoxes()
        {
            foreach (CheckBox xBox in _groupCheckBoxes)
            {
                xBox.Hide();
            }
        }

        private void SetWorkstationGroups()
        {
            List<string> groups = ADContext.GetWorkstationGroups();
            if (groups != null)
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    _groupCheckBoxes[i].Text = groups[i];
                    _groupCheckBoxes[i].Show();
                }
            }
        }

        private void OnOKClick(object sender, EventArgs e)
        {
            if(!entireDirectoryCheckBox.Checked && CheckedGroups.Count < 1)
            {
                entireDirectoryCheckBox.Checked = true;
            }
            _searchPreferences.SetSearchEntireDirectory(entireDirectoryCheckBox.Checked);
            _searchPreferences.SetSearchGroups(CheckedGroups);
            _appController.SaveAppSearchPreferences();
            this.Close();
        }

        private void OnCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnEntireDirectoryCheckChanged(object sender, EventArgs e)
        {
            if (entireDirectoryCheckBox.Checked)
            {
                for (int i = 0; i < _groupCheckBoxes.Length; i++)
                {
                    _groupCheckBoxes[i].Checked = false;
                    _groupCheckBoxes[i].Enabled = false;
                }
            }
            else
            {
                for (int i = 0; i < _groupCheckBoxes.Length; i++)
                {
                    _groupCheckBoxes[i].Enabled = true;
                }
            }
        }
    }
}
