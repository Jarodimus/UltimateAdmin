using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateAdmin.Core.ActiveDirectory;
using UltimateAdmin.Core.Network;

namespace UltimateAdmin.Presentation
{
    public class ViewLocalUsersPresenter : IViewLocalUsersPresenter
    {
        private LocalGroupType _groupType;
        public LocalGroupType GroupType
        {
            get
            {
                return _groupType;
            }
        }
        private List<string> localUsers;
        private Computer _computer;
        public Computer Computer
        {
            get
            {
                return _computer;
            }
        }
        private IViewLocalUsers _view;
        private IApplicationController _appController;
        public IApplicationController AppController
        {
            get
            {
                return _appController;
            }
        }
        private List<string> LocalAdmins = new List<string>();

        public ViewLocalUsersPresenter(IViewLocalUsers view, IApplicationController appController, Computer computer, LocalGroupType groupType)
        {
            _view = view;
            _appController = appController;
            _view.Presenter = this;
            _computer = computer;
            _groupType = groupType;
            Setup();
        }

        private void Setup()
        {
            GetRemoteGroupUsers();
            SetTitleText();
        }

        private void GetRemoteGroupUsers()
        {
            if (_groupType == LocalGroupType.ADMINS)
            {
                localUsers = _computer.GetLocalAdmins();
                _view.GroupLabelText = "Local Administrators";
            }
            if (_groupType == LocalGroupType.RDPUSERS)
            {
                localUsers = _computer.GetRDPUsers();
                _view.GroupLabelText = "Remote Desktop Users";
            }
            _view.UsersList.DataSource = localUsers;
            _view.UsersList.Refresh();
        }

        private void SetTitleText()
        {
            _view.TitleText = Computer.Name;
        }

        public void Run()
        {
            _view.Run();
        }

        public void RemoveLocalUser()
        {
            string selectedUser;
            if (_view.UsersList.SelectedItems.Count > 0)
            {
                selectedUser = (string)_view.UsersList.SelectedItem;
                if (_groupType == LocalGroupType.ADMINS)
                {
                    bool? removed = _computer.RemoveLocalAdmin(selectedUser);
                    if (removed == true)
                    {
                        _view.DisplayMessage(selectedUser + " has been removed from the group.", "");
                    }
                    else
                    {
                        if(removed == null)
                        {
                            _view.DisplayMessage("Error removing " + selectedUser + " from the local administrators group.  User may be local and not in Active Directory.", "Error removing user");
                        }
                    }
                }
                if (_groupType == LocalGroupType.RDPUSERS)
                {
                    bool removed = _computer.RemoveRDPUser(selectedUser);
                    if (removed)
                    {
                        _view.DisplayMessage(selectedUser + " has been removed from the group.", "");
                    }
                }
                GetRemoteGroupUsers();
            }
        }

        public void AddLocalUser()
        {
            _appController.RunAddLocalUser(this);
        }

        public void RefreshUsers()
        {
            GetRemoteGroupUsers();
        }
    }
}
