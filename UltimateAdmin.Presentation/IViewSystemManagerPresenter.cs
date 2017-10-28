using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateAdmin.Core.ActiveDirectory;
using UltimateAdmin.Core.Network;

namespace UltimateAdmin.Presentation
{
    public interface IViewSystemManagerPresenter : IPresenter<IViewSystemManager>
    {
        Computer Computer { get; }
        void ComputerSearch(string searchText, Category categoryType);
        void Update(Computer adObject);
        void LaunchRemoteAssist();
        void LaunchRemoteDesktop();
        void LaunchPAEXEC();
        void RefreshSystem();
        void ShowGroups();
        void ShowLoggedInUser();
        void ShowLocalUsers(LocalGroupType groupType);
        void RestartRemoteMachine();
        void ShutdownRemoteMachine();
        void HideDescriptionBox();
        void UnHideDescriptionBox();
        void UpdateComputerDescription();
        void RunWorkstationSettings();
    }
}
