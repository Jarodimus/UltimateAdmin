using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UltimateAdmin.Core;
using UltimateAdmin.Core.ActiveDirectory;
using UltimateAdmin.Core.Network;

namespace UltimateAdmin.Presentation
{
    public interface IApplicationController
    {
        string AppVersion { get; }
        string AppPath { get; }
        AppSearchPreferences AppSearchPreferences { get; }
        void SaveAppSearchPreferences();
        void RunMain();
        Form MainWindow { get; set; }
        void RunViewSearchResults<T>(List<T> adObjects);
        IViewSystemManagerPresenter SystemManagerPresenter { get; set; }
        void UpdateSystemManager(Computer computer);
        void RunViewLocalUsers(Computer computer, LocalGroupType groupType);
        void RunAddLocalUser(IViewLocalUsersPresenter presenter);
        void RunViewLoggedInUser();
        void RunViewConfiguration();
        void RunShowGroupInformation(ADGroup group);
        void ExitApplication();
        bool DemoModeOn { get; }
        DialogResult RunViewAddOU(IViewConfiguration viewConfig, string workstationGroup);
        DialogResult RunViewAddWorkstationGroup();
        DialogResult RunWorkstationSettings();
    }
}
