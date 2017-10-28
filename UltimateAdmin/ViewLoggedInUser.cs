using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UltimateAdmin.Core.ActiveDirectory;
using UltimateAdmin.Core.Network;
using UltimateAdmin.Core.WMI;
using UltimateAdmin.Presentation;

namespace UltimateAdmin
{
    public partial class ViewLoggedInUser : Form
    {
        private User User;
        private string TitleText = "Logged on to: ";
        private string DisplayedName;
        private bool isUserLoggedOn;
        private bool? isDomainAccount;
        private IViewSystemManagerPresenter SystemManager;

        public ViewLoggedInUser(IViewSystemManagerPresenter _systemManager)
        {
            SystemManager = _systemManager;
            GetLoggedInUser();
            InitializeComponent();
            this.Text = TitleText + SystemManager.Computer.Name;
            Setup();
        }

        public void GetLoggedInUser()
        {
            var userName = WMIQuery.GetLoggedOnLocalUser(SystemManager.Computer);
            if(userName.Equals("No users currently logged on.") || userName.Equals("Unable to query remote machine.  Try again later."))
            {
                isUserLoggedOn = false;
                DisplayedName = userName;
            } else if (ADQuery.isDomainAccount(userName) == true)
            {
                isUserLoggedOn = true;
                isDomainAccount = true;
                DisplayedName = userName;
                User = ADQuery.GetUserByUserName(userName.Split('\\')[1]);
            }
            else
            {
                DisplayedName = SystemManager.Computer.Name + "\\" + userName;
            }
        }

        private void Setup()
        {
            if (isUserLoggedOn)
            {
                if (isDomainAccount == true)
                {
                    domainNameLabel.Text = DisplayedName;
                    domainNameLabel.BringToFront();
                }
                else
                {
                    nonDomainUserNameLabel.Text = DisplayedName;
                    nonDomainUserNameLabel.BringToFront();
                }
            }
            else
            {
                noUsersLoggedOnLabel.Text = DisplayedName;
                noUsersLoggedOnLabel.BringToFront();
            }
        }

        private void OnOKClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}