using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UltimateAdmin.Presentation
{
    public interface IViewMain 
    {
        IViewMainPresenter Presenter { get; set; }
        UserControl SystemManager { get; }
        void Run();
        void Close();
    }
}
