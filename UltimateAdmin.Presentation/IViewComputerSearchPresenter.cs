using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateAdmin.Core.ActiveDirectory;

namespace UltimateAdmin.Presentation
{
    public interface IViewComputerSearchPresenter
    {
        void Run();
        void ComputerSelected(Computer computer);
        IApplicationController AppController { get; }
    }
}
