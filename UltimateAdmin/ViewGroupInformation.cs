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
    public partial class ViewGroupInformation : Form, IViewGroupInformation
    {
        IViewGroupInformationPresenter _presenter;
        public IViewGroupInformationPresenter Presenter
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

        public DataGridView MembersDataGrid
        {
            get
            {
                return membersGridView;
            }
            set
            {
                membersGridView = value;
            }
        }

        public string TitleText
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        public string GroupLabelText
        {
            get
            {
                return groupNameLabel.Text;
            }
            set
            {
                groupNameLabel.Text = value;
            }
        }

        public string Description
        {
            get
            {
                return descriptionBox.Text;
            }
            set
            {
                descriptionBox.Text = value;
            }
        }

        public ViewGroupInformation()
        {
            InitializeComponent();
        }

        public void Run()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            ShowDialog(_presenter.AppController.MainWindow);
        }

        private void OnCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
