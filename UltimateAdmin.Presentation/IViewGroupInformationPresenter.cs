using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateAdmin.Presentation
{
    public interface IViewGroupInformationPresenter
    {
        IApplicationController AppController { get; }
        void Run();
    }
}
