using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UltimateAdmin.Core.ActiveDirectory;

namespace UltimateAdmin.Presentation
{
    public interface IViewComputerSearch : IView<IViewComputerSearchPresenter>
    {
        DataGridView SearchDataGrid { get; }
        void CloseWindow();
    }
}
