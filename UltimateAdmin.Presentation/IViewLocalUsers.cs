using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UltimateAdmin.Presentation
{
    public interface IViewLocalUsers : IView<IViewLocalUsersPresenter>
    {
        string TitleText { get; set; }
        string GroupLabelText { get; set; }
        void CloseWindow();
        ListBox UsersList { get; set; }
        void DisplayMessage(string msg, string title);
    }
}
