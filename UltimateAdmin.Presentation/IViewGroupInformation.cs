using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UltimateAdmin.Presentation
{
    public interface IViewGroupInformation : IView<IViewGroupInformationPresenter>
    {
        string TitleText { get; set; }
        string GroupLabelText { get; set; }
        string Description { get; set; }
        DataGridView MembersDataGrid { get; set; }
    }
}
