using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UltimateAdmin.Core.ActiveDirectory;

namespace UltimateAdmin.Presentation
{
    public class ViewGroupInformationPresenter : IViewGroupInformationPresenter
    {
        private IApplicationController _appController;
        public IApplicationController AppController
        {
            get
            {
                return _appController;
            }
        }

        private readonly IViewGroupInformation _view;

        private Dictionary<string, string> membersDescriptionMap;
        private BindingSource listBinding;

    

        public ViewGroupInformationPresenter(IViewGroupInformation view, IApplicationController appController, ADGroup group)
        {
            _view = view;
            _appController = appController;
            membersDescriptionMap = group.MemberDescriptionMap;
            _view.Presenter = this;
            _view.GroupLabelText = group.Name;
            _view.TitleText = "Information for: " + group.Name + " (security group)";
            _view.Description = group.Description;
        }

        public void Run()
        {
            _view.Run();
        }

        public void InitializeGrid()
        {
            listBinding = new BindingSource(membersDescriptionMap, null);
            _view.MembersDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _view.MembersDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _view.MembersDataGrid.MultiSelect = false;
            _view.MembersDataGrid.RowHeadersVisible = false;
            _view.MembersDataGrid.AutoGenerateColumns = false;
            _view.MembersDataGrid.AllowUserToAddRows = false;
            _view.MembersDataGrid.DataSource = listBinding;

            DataGridViewColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.DataPropertyName = "Name";
            nameColumn.HeaderText = "Name";
            _view.MembersDataGrid.Columns.Add(nameColumn);

            DataGridViewColumn descColumn = new DataGridViewTextBoxColumn();
            descColumn.DataPropertyName = "Description";
            descColumn.HeaderText = "Description";
            _view.MembersDataGrid.Columns.Add(descColumn);
        }
    }
}
