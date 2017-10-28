using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UltimateAdmin.Core.Network;
using UltimateAdmin.Presentation;

namespace UltimateAdmin
{
    public partial class ViewSystemManager : UserControl, IViewSystemManager
    {
        private IViewSystemManagerPresenter _presenter;

        public IViewSystemManagerPresenter Presenter
        {
            get { return _presenter; }
            set
            {
                if (_presenter == null)
                {
                    _presenter = value;
                }
            }
        }

        public ViewSystemManager()
        {
            InitializeComponent();
            HideSystemInfo();
            descriptionValue.UseMnemonic = false;
        }

        public List<string> SystemInfoValues
        {
            get
            {
                return new List<string>()
                {
                    TypeValueLabel,
                    BIOSValueLabel,
                    MakeModelValueLabel,
                    SerialValueLabel,
                    OSValueLabel,
                };
            }
        }

        public string NameSearchText
        {
            get { return NameSearchTextBox.Text; }
        }

        public string DescriptionSearchText
        {
            get { return DescriptionSearchTextBox.Text; }
        }

        string IViewSystemManager.ComputerNameLabel
        {
            get
            {
                return ComputerNameLabel.Text;
            }

            set
            {
                ComputerNameLabel.Text = value;
                InformationForLabel.TextAlign = ContentAlignment.MiddleCenter;
                ComputerNameLabel.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

        string IViewSystemManager.InformationForLabel
        {
            get
            {
                return InformationForLabel.Text;
            }
        }

        public string OnlineStatusLabel
        {
            get
            {
                return onlineStatusLabel.Text;
            }

            set
            {
                onlineStatusLabel.Text = value;
                SetOnlineStatusLabelColor();
            }
        }

        private void SetOnlineStatusLabelColor()
        {
            if (onlineStatusLabel.Text.Equals("Online"))
            {
                onlineStatusLabel.ForeColor = Color.LimeGreen;
            }
            else
            {
                onlineStatusLabel.ForeColor = Color.Red;
            }
        }

        public string TimeLabel
        {
            get
            {
                return timeLabel.Text;
            }
            set
            {
                timeLabel.Text = value;
            }
        }

        public string TypeValueLabel
        {
            get
            {
                return typeValue.Text;
            }
            set
            {
                typeValue.Text = value;
            }
        }

        public string BIOSValueLabel
        {
            get
            {
                return biosValue.Text;
            }
            set
            {
                biosValue.Text = value;
            }
        }

        public string MakeModelValueLabel
        {
            get
            {
                return makeModelValue.Text;
            }
            set
            {
                makeModelValue.Text = value;
            }
        }

        public string SerialValueLabel
        {
            get
            {
                return serialValue.Text;
            }
            set
            {
                serialValue.Text = value;
            }
        }

        public string OSValueLabel
        {
            get
            {
                return osValue.Text;
            }
            set
            {
                osValue.Text = value;
            }
        }

        public string Description
        {
            get
            {
                return descriptionValue.Text;
            }
            set
            {
                descriptionValue.Text = value;
            }
        }

        public Label DescriptionLabel
        {
            get
            {
                return descriptionValue;
            }
            set
            {
                descriptionValue = value;
            }
        }

        public TextBox DescriptionBox
        {
            get
            {
                return descriptionTextBox;
            }
        }

        public string OrganizationalUnit
        {
            get
            {
                return organizationalUnitValue.Text;
            }
            set
            {
                organizationalUnitValue.Text = value;
            }
        }

        public string BitLockerRecoveryKey
        {
            get
            {
                return bitLockerKeyBox.Text;
            }
            set
            {
                bitLockerKeyBox.Text = value;
            }
        }

        public void ShowSystemInfo()
        {
            BlankPanel.SendToBack();
        }

        public void HideSystemInfo()
        {
            SysInfoPanel.SendToBack();
        }

        public void DisplayMessage(string msg, string title)
        {
            MessageBox.Show(msg, title);
        }

        private void OnNameSearch(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(NameSearchText))
                return;
            _presenter.ComputerSearch(NameSearchText, Category.Name);
        }

        private void OnDescriptionSearch(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(DescriptionSearchText))
                return;
            _presenter.ComputerSearch(DescriptionSearchText, Category.Description);
        }

        public void ClearFields()
        {
            NameSearchTextBox.Text = "";
            DescriptionSearchTextBox.Text = "";
        }

        private void OnRemoteDesktopClick(object sender, EventArgs e)
        {
            _presenter.LaunchRemoteDesktop();
        }

        private void OnRemoteAssistClick(object sender, EventArgs e)
        {
            _presenter.LaunchRemoteAssist();
        }

        private void OnPSEXECClick(object sender, EventArgs e)
        {
            _presenter.LaunchPAEXEC();
        }

        private void OnNameSearchKeyPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                NameSearchButton.PerformClick();
            }
        }

        private void OnDescriptionSearchKeyPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                DescriptionSearchButton.PerformClick();
            }
        }

        private void OnLocalAdminsClick(object sender, EventArgs e)
        {
            _presenter.ShowLocalUsers(LocalGroupType.ADMINS);
        }

        private void OnRDPUsersClick(object sender, EventArgs e)
        {
            _presenter.ShowLocalUsers(LocalGroupType.RDPUSERS);
        }

        private void OnRestartClick(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to restart: " + _presenter.Computer.Name + "?", "Restart", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                DialogResult confirmResult = MessageBox.Show("Warning!  User may lose data if currently logged in.  Are you sure you want to restart?", "Confirm Restart", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    _presenter.RestartRemoteMachine();
                }
            }
        }

        private void OnShutdownClick(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to shut down: " + _presenter.Computer.Name + "?", "Warning: Shutdown", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                DialogResult confirmResult = MessageBox.Show("Warning!  User may lose data if currently logged in.  Are you sure you want to restart?", "Confirm Restart", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    _presenter.ShutdownRemoteMachine();
                }
            }
        }

        private void OnLoggedInUserClick(object sender, EventArgs e)
        {
            _presenter.ShowLoggedInUser();
        }

        private void OnGroupsClick(object sender, EventArgs e)
        {
            _presenter.ShowGroups();
        }

        private void OnDescriptionLinkClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _presenter.UnHideDescriptionBox();
        }

        private void OnDescriptionTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                PromptDescriptionUpdate();
            }
        }

        private void PromptDescriptionUpdate()
        {
            DialogResult dialogResult = MessageBox.Show("Update description to: " + descriptionTextBox.Text + "?", "Update Description", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                _presenter.UpdateComputerDescription();
            }
            else
            {
                _presenter.HideDescriptionBox();
            }
        }

        private void OnUIElementClick(object sender, MouseEventArgs e)
        {
            if (descriptionTextBox.Focused)
            {
                if (sender.GetType() == typeof(Panel))
                {
                    Panel senderControl = (Panel)sender;
                    senderControl.Focus();
                }
                if (sender.GetType() == typeof(Label))
                {
                    Label senderControl = (Label)sender;
                    senderControl.Focus();
                }
                if (sender.GetType() == typeof(LinkLabel))
                {
                    LinkLabel senderControl = (LinkLabel)sender;
                    senderControl.Focus();
                }
            }
        }

        private void OnDescriptionBoxLeave(object sender, EventArgs e)
        {
            _presenter.HideDescriptionBox();
        }

        private void OnRefreshClick(object sender, EventArgs e)
        {
            _presenter.RefreshSystem();
        }

        private void InfoOnClick(object sender, EventArgs e)
        {
            MessageBox.Show("See Configuration to add Workstation OU's to search.  By default the entire directory is searched.");
        }

        private void OnWorkstationSearchSettingsClick(object sender, EventArgs e)
        {
            _presenter.RunWorkstationSettings();
        }

        public void Run()
        {
        }

        private void OnCopyBitLockerClick(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(bitLockerKeyBox.Text);
            MessageBox.Show("Key has been copied to clipboard.");
        }

        /////////////
        //Demo Mode ///////////////////
        ////////////

        public void DisableFunctionality()
        {
            NameSearchButton.Enabled = false;
            DescriptionSearchButton.Enabled = false;
            SettingsButton.Enabled = false;
            SetDisabledLabelValues();
            DescriptionBox.Hide();
            ShowSystemInfo();
            DisableEventHandlers();
            SetDisabledHandlers();
        }

        private void SetDisabledLabelValues()
        {
            ComputerNameLabel.Text = "COM-UX9381Z";
            Description = "John Smith's PC";
            DescriptionBox.Text = Description;
            bitLockerKeyBox.Text = "12345678910111231415161718";
            organizationalUnitValue.Text = "ou=Workstations,ou=us,dc=test,dc=global,dc=com";
            OnlineStatusLabel = "Online";
            TypeValueLabel = "Desktop";
            BIOSValueLabel = "Alpha 1.9";
            MakeModelValueLabel = "BYOPC X9Z";
            SerialValueLabel = "UX9381Z";
            OSValueLabel = "Windows 10 Pro";
        }

        private void DisableEventHandlers()
        {
            loggedInUserLink.Click -= OnLoggedInUserClick;
            localAdminsLink.Click -= OnLocalAdminsClick;
            rdpUsersLink.Click -= OnRDPUsersClick;
            shutdownLink.Click -= OnShutdownClick;
            restartLink.Click -= OnRestartClick;
            groupsLink.Click -= OnGroupsClick;
            refreshIcon.Click -= OnRefreshClick;
            descriptionTextBox.KeyDown -= OnDescriptionTextBoxKeyDown;
            remoteDesktopButton.Click -= OnRemoteDesktopClick;
            remoteAssistButton.Click -= OnRemoteAssistClick;
            paExecButton.Click -= OnPSEXECClick;
        }

        private void SetDisabledHandlers()
        {
            loggedInUserLink.Click += new EventHandler(OnDisabledLoggedInUserClick);
            localAdminsLink.Click += new EventHandler(OnDisabledLocalAdminsClick);
            rdpUsersLink.Click += new EventHandler(OnDisabledRDPUsersClick);
            shutdownLink.Click += new EventHandler(OnDisabledShutdownClick);
            restartLink.Click += new EventHandler(OnDisabledRestartClick);
            groupsLink.Click += new EventHandler(OnDisabledGroupLinkClick);
            refreshIcon.Click += new EventHandler(OnDisabledRefreshClick);
            descriptionTextBox.KeyDown += new KeyEventHandler(OnDisabledDescriptionTextBoxKeyDown);
            remoteDesktopButton.Click += new EventHandler(OnDisabledButtonClick);
            remoteAssistButton.Click += new EventHandler(OnDisabledButtonClick);
            paExecButton.Click += new EventHandler(OnDisabledButtonClick);
        }

        private void OnDisabledLoggedInUserClick(object sender, EventArgs e)
        {
            DemoViewLoggedInUser demoViewLoggedOn = new DemoViewLoggedInUser("jsmith", ComputerNameLabel.Text);
            demoViewLoggedOn.StartPosition = FormStartPosition.CenterScreen;
            demoViewLoggedOn.Show();
        }

        private void OnDisabledLocalAdminsClick(object sender, EventArgs e)
        {
            DemoViewLocalUsers viewLocalUsers = new DemoViewLocalUsers(ComputerNameLabel.Text, LocalGroupType.ADMINS);
            viewLocalUsers.StartPosition = FormStartPosition.CenterScreen;
            viewLocalUsers.ShowDialog();
        }

        private void OnDisabledRDPUsersClick(object sender, EventArgs e)
        {
            DemoViewLocalUsers viewLocalUsers = new DemoViewLocalUsers(ComputerNameLabel.Text, LocalGroupType.RDPUSERS);
            viewLocalUsers.StartPosition = FormStartPosition.CenterScreen;
            viewLocalUsers.ShowDialog();
        }

        private void OnDisabledShutdownClick(object sender, EventArgs e)
        {
            return;
        }

        private void OnDisabledRestartClick(object sender, EventArgs e)
        {
            return;
        }

        private void OnDisabledRefreshClick(object sender, EventArgs e)
        {
            return;
        }

        private void OnDisabledButtonClick(object sender, EventArgs e)
        {
            DisplayMessage("This functionality is not available in the demo version", "");
        }

        private void OnDisabledDescriptionTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                DialogResult dialogResult = MessageBox.Show("Update description to: " + descriptionTextBox.Text + "?", "Update Description", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    var newValue = descriptionTextBox.Text;
                    descriptionValue.Text = newValue;
                    DisplayMessage("AD Description has been updated for " + ComputerNameLabel.Text, "");
                    _presenter.HideDescriptionBox();
                }
                else
                {
                    _presenter.HideDescriptionBox();
                }
            }
        }

        private void OnDisabledGroupLinkClick(object sender, EventArgs e)
        {
            DisplayMessage("Domain Users\n", "Groups for: " + ComputerNameLabel.Text);
        }
    }
}