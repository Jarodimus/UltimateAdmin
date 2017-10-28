using UltimateAdmin.Presentation;
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
using System.DirectoryServices.AccountManagement;

namespace UltimateAdmin
{
    public partial class ViewComputerSearch : Form, IViewComputerSearch
    {
        IViewComputerSearchPresenter _presenter;
        public IViewComputerSearchPresenter Presenter
        {
            get
            {
                return _presenter;
            }
            set
            {
                if (_presenter == null)
                {
                    _presenter = value;
                }
            }
        }

        public DataGridView SearchDataGrid
        {
            get
            {
                return searchGridView;
            }
            set
            {
                searchGridView = value;
            }
        }

        public ViewComputerSearch()
        {
            InitializeComponent();
        }

        public void OnSelectComputer(object sender, DataGridViewCellEventArgs e)
        {
            var row = searchGridView.SelectedRows[0];
            var computer = (Computer) row.DataBoundItem;
            _presenter.ComputerSelected(computer);
        }

        public void CloseWindow()
        {
            this.Close();
        }

        public void Run()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            ShowDialog(_presenter.AppController.MainWindow);
        }
    }
}