using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateAdmin.Core.ActiveDirectory;
using UltimateAdmin.Core.Network;

namespace UltimateAdmin.Presentation
{
    public interface IViewLocalUsersPresenter
    {
        IApplicationController AppController { get; }
        LocalGroupType GroupType { get; }
        Computer Computer { get; }
        void Run();
        void RemoveLocalUser();
        void AddLocalUser();
        void RefreshUsers();
    }
}
