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
    public partial class ViewLocalUsers : Form, IViewLocalUsers
    {
        IViewLocalUsersPresenter _presenter;
        public IViewLocalUsersPresenter Presenter
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

        public ViewLocalUsers()
        {
            InitializeComponent();
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
                return localUsersLabel.Text;
            }
            set
            {
                localUsersLabel.Text = value;
            }
        }

        public ListBox UsersList
        {
            get
            {
                return UsersListBox;
            }
            set
            {
                UsersListBox = value;
            }
        }

        public void DisplayMessage(string msg, string title)
        {
            MessageBox.Show(msg, title);
        }

        public void CloseWindow()
        {
            this.Close();
        }

        public void Run()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog(_presenter.AppController.MainWindow);
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            _presenter.AddLocalUser();
        }

        private void RemoveButtonClick(object sender, EventArgs e)
        {
            _presenter.RemoveLocalUser();
        }

        private void CloseButtonClick(object sender, EventArgs e)
        {
            CloseWindow();
        }
    }
}
