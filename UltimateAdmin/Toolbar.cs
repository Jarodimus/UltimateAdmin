using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UltimateAdmin.Presentation;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UltimateAdmin.Core.ActiveDirectory;

namespace UltimateAdmin
{
    public partial class Toolbar : UserControl
    {
        private IApplicationController _appController;
        public IApplicationController AppController
        {
            get
            {
                return _appController;
            }
            set
            {
                if (_appController == null)
                {
                    _appController = value;
                }
            }
        }

        public Toolbar()
        {
            InitializeComponent();
            aboutToolStripMenuItem.Margin = new Padding();
        }

        private void OnOpenADUsersComputers(object sender, EventArgs e)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.Arguments = "/k mmc.exe dsa.msc";
            info.UseShellExecute = false;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            Process p = new Process();
            p.StartInfo = info;
            p.Start();
        }

        private void OnOpenComputerMgmt(object sender, EventArgs e)
        {
            Process.Start("mmc.exe", @"c:\windows\system32\compmgmt.msc");
        }

        private void OnRegistryEditorClick(object sender, EventArgs e)
        {
            Process.Start(@"c:\windows\regedit.exe");
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        //Uses the above DLL's to use the 32-bit version of cmd.exe.  Normally it will use the 64-bit version which is incompatible with certain tools such as query user.
        private void OnCommandPromptClick(object sender, EventArgs e)
        {
            IntPtr ptr = new IntPtr();
            Wow64DisableWow64FsRedirection(ref ptr);
            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"cmd.exe";
            startInfo.WorkingDirectory = @"c:\";
            p.StartInfo = startInfo;
            p.Start();
        }

        private void OnPSEXECClick(object sender, EventArgs e)
        {
            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"c:\windows\system32\cmd.exe";
            startInfo.WorkingDirectory = _appController.AppPath + "\\PAExec";
            p.StartInfo = startInfo;
            p.Start();
        }

        private void OnConfigurationClick(object sender, EventArgs e)
        {
            _appController.RunViewConfiguration();
        }

        private void OnLoadSettingsClick(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = "ad_preferences";
            openFileDialog.Filter = "XML-File | *.xml";
            openFileDialog.ShowHelp = true;
            openFileDialog.InitialDirectory = Application.StartupPath;
            DialogResult dr = openFileDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            if (filePath != "")
            {
                string successFail = ADContextPreferenceLoader.LoadPreferencesFromFile(filePath);
                if (successFail.Equals("success"))
                {
                    MessageBox.Show("Settings uploaded successfully.");
                }
                else
                {
                    MessageBox.Show("Error importing settings file.");
                }
            }
        }

        private void OnAboutClick(object sender, EventArgs e)
        {
            ViewAbout viewAbout = new ViewAbout(AppController.AppVersion);
            viewAbout.StartPosition = FormStartPosition.CenterParent;
            viewAbout.ShowDialog(_appController.MainWindow);
        }

        private void OnExitClick(object sender, EventArgs e)
        {
            _appController.ExitApplication();
        }

        public void DisableFunctionality()
        {
            settingsToolbarItem.Enabled = false;
        }
    }
}
