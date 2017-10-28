using UltimateAdmin.Core.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.DirectoryServices.AccountManagement;

namespace UltimateAdmin.Presentation
{
    public class ViewComputerSearchPresenter : IViewComputerSearchPresenter
    {
        private readonly IViewComputerSearch _view;
        private readonly List<Computer> _searchResults = new List<Computer>();
        private readonly IApplicationController _appController;
        public IApplicationController AppController
        {
            get
            {
                return _appController;
            }
        }
        private BindingSource listBinding;

        public ViewComputerSearchPresenter(IViewComputerSearch view, IApplicationController appController, List<Computer> searchResults)
        {
            _view = view;
            _appController = appController;
            _view.Presenter = this;
            _searchResults = searchResults;
            _searchResults.Sort();
            InitializeGrid();
        }

        public void InitializeGrid()
        {
            listBinding = new BindingSource(_searchResults, null);
            _view.SearchDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _view.SearchDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _view.SearchDataGrid.MultiSelect = false;
            _view.SearchDataGrid.RowHeadersVisible = false;
            _view.SearchDataGrid.AutoGenerateColumns = false;
            _view.SearchDataGrid.AllowUserToAddRows = false;
            _view.SearchDataGrid.DataSource = listBinding;

            DataGridViewColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.DataPropertyName = "Name";
            nameColumn.HeaderText = "Name";
            _view.SearchDataGrid.Columns.Add(nameColumn);

            DataGridViewColumn descColumn = new DataGridViewTextBoxColumn();
            descColumn.DataPropertyName = "Description";
            descColumn.HeaderText = "Description";
            _view.SearchDataGrid.Columns.Add(descColumn);
        }

        public void Run()
        {
            _view.Run();
        }

        public void ComputerSelected(Computer computer)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            _appController.UpdateSystemManager(computer);
            Cursor.Current = Cursors.Default;
            _view.CloseWindow();
        }
    }
}
