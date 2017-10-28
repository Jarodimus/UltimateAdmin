using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UltimateAdmin.Presentation;

namespace UltimateAdmin
{
    public partial class ViewMain : Form, IViewMain
    {
        private IApplicationController _appController;
        private IViewMainPresenter _presenter;
        public IViewMainPresenter Presenter
        {
            get { return _presenter;}

            set
            {
                if(_presenter == null)
                {
                    _presenter = value;
                }
            }
        }

        public UserControl SystemManager
        {
            get
            {
                return SystemMgr;
            }
        }

        public ViewMain(IApplicationController appController)
        {
            _appController = appController;
            InitializeComponent();
            SystemMgr.Presenter = new ViewSystemManagerPresenter(SystemMgr, _appController);
            Toolbar.AppController = _appController;
            if (_appController.DemoModeOn)
            {
                this.Text = "UltimateAdmin ---- DEMO MODE";
                Toolbar.DisableFunctionality();
                MessageBox.Show("Unable to contact an Active Directory domain controller.\n\nThe application is running in Demo mode.  Not all functionality is available.");
            }
        }

        public void Run()
        {
            ShowDialog();
        }

        private void onClose(object sender, FormClosedEventArgs e)
        {
            _presenter.Close();
        }

        private void OnSystemManager(object sender, EventArgs e)
        {
            _presenter.ShowSystemManager();
        }
    }
}
