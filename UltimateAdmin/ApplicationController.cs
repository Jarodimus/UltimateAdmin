using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateAdmin.Presentation;
using UltimateAdmin.Core.ActiveDirectory;
using System.Windows.Forms;
using UltimateAdmin.Core.Network;
using UltimateAdmin.Core;
using UltimateAdmin.Core.Logging;
using System.Reflection;
using System.IO;

namespace UltimateAdmin
{
    public class ApplicationController : IApplicationController
    {
        private string appVersion = "1.0.4";
        public string AppVersion
        {
            get
            {
                return appVersion;
            }
        }
        public string AppPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
        private Form _mainWindow;
        public Form MainWindow
        {
            get
            {
                return _mainWindow;
            }
            set
            {
                if(_mainWindow == null)
                {
                    _mainWindow = value;
                }
            }
        }
        private IViewSystemManagerPresenter _systemManagerPresenter;
        public IViewSystemManagerPresenter SystemManagerPresenter
        {
            get
            {
                return _systemManagerPresenter;
            }
            set
            {
                if(_systemManagerPresenter == null)
                {
                    _systemManagerPresenter = value;
                }
            }
        }

        private AppSearchPreferences _appSearchPreferences;
        public AppSearchPreferences AppSearchPreferences
        {
            get
            {
                return _appSearchPreferences;
            }
        }

        public void SaveAppSearchPreferences()
        {
            _appSearchPreferences.SavePreferences();
        }

        private bool isDemoMode;
        public bool DemoModeOn
        {
            get
            {
                return isDemoMode;
            }
        }

        public ApplicationController()
        {
            try
            {
                ADContext.Initialize();
            }
            catch(TypeInitializationException e)
            {
                isDemoMode = true;
            }
            _appSearchPreferences = new AppSearchPreferences();
        }

        public void RunMain()
        {
            using (var view = new ViewMain(this))
            {
                MainWindow = view;
                var presenter = new ViewMainPresenter(view, this);
                presenter.Run();
            }
        }

        public void RunViewConfiguration()
        {
            using (var viewConfig = new ViewConfiguration(this))
            {
                viewConfig.StartPosition = FormStartPosition.CenterParent;
                viewConfig.ShowDialog(MainWindow);
            }
        }

        public DialogResult RunViewAddOU(IViewConfiguration viewConfig, string workstationGroup)
        {
            using (ViewAddOU viewAddOU = new ViewAddOU(viewConfig, workstationGroup))
            {
                viewAddOU.StartPosition = FormStartPosition.CenterParent;
                viewAddOU.ShowDialog(MainWindow);
                return viewAddOU.DialogResult;
            }
        }

        public DialogResult RunViewAddWorkstationGroup()
        {
            using (var viewCreateGroup = new ViewCreateWorkstationGroup())
            {
                viewCreateGroup.StartPosition = FormStartPosition.CenterParent;
                viewCreateGroup.ShowDialog(MainWindow);
                return viewCreateGroup.DialogResult;
            }
        }

        public void RunViewSearchResults<T>(List<T> adObjects)
        {
            if (adObjects.GetType() == typeof(List<Computer>))
            {
                using (var view = new ViewComputerSearch())
                {
                    List<Computer> computerList = adObjects.Cast<Computer>().ToList();
                    var presenter = new ViewComputerSearchPresenter(view, this, computerList);
                    presenter.Run();
                }
            }
        }

        public void RunViewLocalUsers(Computer computer, LocalGroupType groupType)
        {
            using (var view = new ViewLocalUsers())
            {
                var presenter = new ViewLocalUsersPresenter(view, this, computer, groupType);
                presenter.Run();
            }
        }

        public void RunViewLoggedInUser()
        {
            using (var viewUser = new ViewLoggedInUser(SystemManagerPresenter))
            {
                viewUser.StartPosition = FormStartPosition.CenterParent;
                viewUser.ShowDialog(MainWindow);
            }
        }

        public void RunAddLocalUser(IViewLocalUsersPresenter presenter)
        {
            var addUser = new ViewAddLocalUser(presenter, this);
            addUser.StartPosition = FormStartPosition.CenterParent;
            addUser.ShowDialog(MainWindow);
        }

        public void RunShowGroupInformation(ADGroup group)
        {
            var viewGroupInfo = new ViewGroupInformation();
            var viewGroupInfoPresenter = new ViewGroupInformationPresenter(viewGroupInfo, this, group);
            viewGroupInfoPresenter.Run();
        }

        public DialogResult RunWorkstationSettings()
        {
            ViewWorkstationSettings viewSettings = new ViewWorkstationSettings(AppSearchPreferences.WorkstationPreferences, this);
            viewSettings.StartPosition = FormStartPosition.CenterParent;
            DialogResult result = viewSettings.ShowDialog(MainWindow);
            return result;
        }

        public void UpdateSystemManager(Computer computer)
        {
            _systemManagerPresenter.Update(computer);
        }

        public void ExitApplication()
        {
            MainWindow.Close();
        }
    }
}
