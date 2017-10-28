using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UltimateAdmin.Core.ActiveDirectory;

namespace UltimateAdmin.Presentation
{
    public interface IViewSystemManager : IView<IViewSystemManagerPresenter>
    {
        List<string> SystemInfoValues { get; }
        string ComputerNameLabel { get; set; }
        string OnlineStatusLabel { get; set; }
        string TypeValueLabel { get; set; }
        string BIOSValueLabel { get; set; }
        string MakeModelValueLabel { get; set; }
        string SerialValueLabel { get; set; }
        string OSValueLabel { get; set; }
        string InformationForLabel { get; }
        string Description { get; set; }
        string OrganizationalUnit { get; set; }
        string BitLockerRecoveryKey { get; set; }
        string NameSearchText { get; }
        string DescriptionSearchText { get; }
        string TimeLabel { get; set; }
        Label DescriptionLabel { get; set; }
        TextBox DescriptionBox { get; }
        void ShowSystemInfo();
        void HideSystemInfo();
        void DisplayMessage(string msg, string title);
        void ClearFields();
        void DisableFunctionality();
    }
}
