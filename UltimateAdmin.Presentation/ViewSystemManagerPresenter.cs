using UltimateAdmin.Core.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using UltimateAdmin.Core.Network;
using System.Diagnostics;
using System.Management;
using System.Windows.Forms;
using UltimateAdmin.Core.WMI;
using System.Text.RegularExpressions;
using UltimateAdmin.Core.Logging;
using System.Reflection;
using UltimateAdmin.Core;
using System.Threading;

namespace UltimateAdmin.Presentation
{
    public class ViewSystemManagerPresenter : IViewSystemManagerPresenter
    {
        private WorkstationSearchPreferences searchPreferences;

        private IViewSystemManager _view;

        private IApplicationController _appController;

        private Computer _Computer;
        public Computer Computer
        {
            get
            {
                return _Computer;
            }
            private set
            {
                _Computer = value;
            }
        }

        public ViewSystemManagerPresenter(IViewSystemManager view, IApplicationController appController)
        {
            _view = view;
            _appController = appController;
            _appController.SystemManagerPresenter = this;
            searchPreferences = appController.AppSearchPreferences.WorkstationPreferences;
            if (_appController.DemoModeOn)
            {
                _view.DisableFunctionality();
            }
        }

        public IViewSystemManager View
        {
            get
            {
                return _view;
            }
        }

        public void ComputerSearch(string searchText, Category categoryType)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            List<Computer> computerList = ADWorkstationsQuery.GetComputers(searchText, categoryType, searchPreferences);
            if (computerList == null)
            {
                _view.DisplayMessage("Network error. Please try again.", "Network Error.");
            }
            else
            {
                _appController.RunViewSearchResults(computerList);
            }
            _view.ClearFields();
            Cursor.Current = Cursors.Default;
        }

        public void Run()
        {
            _view.Run();
        }

        public void Update(Computer computer)
        {
            Computer = computer;
            _view.ComputerNameLabel = computer.Name;
            _view.Description = computer.Description;
            _view.DescriptionBox.Text = _view.Description;
            HideDescriptionBox();
            _view.BitLockerRecoveryKey = ADInfo.GetBLRecoveryKey(computer.Name);
            _view.OrganizationalUnit = ADInfo.GetComputerOU(computer.Name);
            bool isOnline = IsOnlineStatus();
            SetSystemInformation(isOnline);
            _view.ShowSystemInfo();
        }

        private async void SetSystemInformation(bool isOnline)
        {
            if (isOnline)
            {
                string formFactor = "";
                string biosVersion = "";
                string makeModel = "";
                string serialNumber = "";
                string OS = "";
                _view.TypeValueLabel = "loading...";
                _view.BIOSValueLabel = "loading...";
                _view.MakeModelValueLabel = "loading...";
                _view.SerialValueLabel = "loading...";
                _view.OSValueLabel = "loading...";
                await Task.Run(() =>
                {
                    if (isOnline)
                    {
                        try
                        {
                            formFactor = WMIQuery.GetFormFactor(Computer.Name);
                            biosVersion = WMIQuery.GetBIOSVersion(Computer.Name);
                            makeModel = WMIQuery.GetMakeModel(Computer.Name);
                            serialNumber = WMIQuery.GetSystemSerialNumber(Computer.Name);
                            OS = WMIQuery.GetOSVersion(Computer.Name);
                        }
                        catch (Exception e)
                        {
                            _view.DisplayMessage(e.Message, "Set System Info Error");
                        }
                    }
                });
                if (String.IsNullOrEmpty(formFactor))
                {
                    _view.TypeValueLabel = "unavailable.";
                }
                else
                {
                    _view.TypeValueLabel = formFactor;
                }
                if (String.IsNullOrEmpty(biosVersion))
                {
                    _view.BIOSValueLabel = "unavailable.";
                }
                else
                {
                    _view.BIOSValueLabel = biosVersion;
                }
                if (String.IsNullOrEmpty(makeModel))
                {
                    _view.MakeModelValueLabel = "unavailable.";
                }
                else
                {
                    _view.MakeModelValueLabel = makeModel;
                }
                if (String.IsNullOrEmpty(serialNumber))
                {
                    _view.SerialValueLabel = "unavailable.";
                }
                else
                {
                    _view.SerialValueLabel = serialNumber;
                }
                if (String.IsNullOrEmpty(OS))
                {
                    _view.OSValueLabel = "unavailable.";
                }
                else
                {
                    _view.OSValueLabel = OS;
                }
            }
            else
            {
                _view.TypeValueLabel = "";
                _view.BIOSValueLabel = "";
                _view.MakeModelValueLabel = "";
                _view.SerialValueLabel = "";
                _view.OSValueLabel = "";
            }
        }

        //private bool isRemoteManagementEnabled()
        //{
        //    bool isRemoteRegistryStarted = Computer.CheckIfRemoteRegistryStarted();
        //    bool isRPCEnabled = Computer.EnableRPC();
        //    if(isRemoteRegistryStarted && isRPCEnabled)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        private bool IsOnlineStatus()
        {
            bool isOnline = false;
            PingResult result = NetStatus.GetPingWithResult(Computer.Name);
            if (result.IsOnline)
            {
                _view.OnlineStatusLabel = "Online";
                isOnline = true;
            }
            else
            {
                _view.OnlineStatusLabel = "Offline";
                isOnline = false;
            }
            _view.TimeLabel = GetCurrentTime();
            return isOnline;
        }

        private bool SystemInfoNotSet()
        {
            bool valuesNotSet = false;
            foreach (string s in _view.SystemInfoValues)
            {
                if (string.IsNullOrEmpty(s) || s.Equals("unavailable") || s.Equals("loading..."))
                {
                    valuesNotSet = true;
                }
            }
            return valuesNotSet;
        }

        private string GetCurrentTime()
        {
            return DateTime.Now.ToLongTimeString();
        }

        public void LaunchRemoteAssist()
        {
            Process p = new Process();
            p.StartInfo.FileName = "C:\\Windows\\System32\\msra.exe";
            p.StartInfo.Arguments = "/offerra " + Computer.Name;
            p.StartInfo.Verb = "runas";
            p.StartInfo.UseShellExecute = true;
            p.Start();
            p.Dispose();
        }

        public void LaunchRemoteDesktop()
        {
            if (IsOnlineStatus())
            {
                Process rdcProcess = new Process();
                rdcProcess.StartInfo.FileName = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\cmdkey.exe");
                rdcProcess.StartInfo.Arguments = "/generic:TERMSRV/" + Computer.Name;
                rdcProcess.Start();
                rdcProcess.StartInfo.FileName = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\mstsc.exe");
                rdcProcess.StartInfo.Arguments = "/v " + Computer.Name; // ip or name of computer to connect
                rdcProcess.Start();
                rdcProcess.Dispose();
            }
            else
            {
                _view.DisplayMessage("Computer is currently offline.", Computer.Name);
            }
        }

        public void LaunchPAEXEC()
        {
            if (IsOnlineStatus())
            {
                Process p = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = @"c:\windows\system32\cmd.exe";
                startInfo.WorkingDirectory = _appController.AppPath + "\\PAExec";
                startInfo.Arguments = @"/k paexec \\" + Computer.Name + " cmd";
                p.StartInfo = startInfo;
                p.Start();
            }
            else
            {
                _view.DisplayMessage("Host (" + Computer.Name + ") unreachable.", Computer.Name);
            }
        }

        public void ShowLoggedInUser()
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            if (IsOnlineStatus())
            {
                _appController.RunViewLoggedInUser();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Cursor.Current = Cursors.Default;
                _view.DisplayMessage("Computer is currently offline.", Computer.Name);
            }
        }

        public void ShowLocalUsers(LocalGroupType groupType)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            if (IsOnlineStatus())
            {
                _appController.RunViewLocalUsers(Computer, groupType);
            }
            else
            {
                _view.DisplayMessage(Computer.Name + " is currently offline.", "");
            }
            Cursor.Current = Cursors.Default;
        }

        public void ShowGroups()
        {
            List<string> ComputerADGroups = ADWorkstationsQuery.GetComputerGroups(Computer.Name);
            if (ComputerADGroups != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string s in ComputerADGroups)
                {
                    sb.Append(s + "\n");
                }
                _view.DisplayMessage(sb.ToString(), "Groups for: " + Computer.Name);
            }
            else
            {
                _view.DisplayMessage("", "Groups for: " + Computer.Name);
            }
        }

        public void RestartRemoteMachine()
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = "/c\"" + string.Format("shutdown /m \\\\{0} /f /t 00 /r", Computer.Name) + "\""
            };
            process.StartInfo = startInfo;
            process.Start();
            _view.DisplayMessage(Computer.Name + " is restarting...", "");
            process.Dispose();
        }

        public void ShutdownRemoteMachine()
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = "/c\"" + string.Format("shutdown /m \\\\{0} /f /t 00 /s", Computer.Name) + "\""
            };
            process.StartInfo = startInfo;
            process.Start();
            _view.DisplayMessage(Computer.Name + " is shutting down...", "");
            process.Dispose();
        }

        public void UpdateComputerDescription()
        {
            bool descUpdated = ADManager.UpdateComputerDescription(Computer.Name, _view.DescriptionBox.Text);
            if (descUpdated)
            {
                _view.DisplayMessage("AD Description has been updated for " + Computer.Name, "");
            }
            _view.DescriptionLabel.Text = ADInfo.GetComputerDescription(Computer.Name);
            HideDescriptionBox();
        }

        public void UnHideDescriptionBox()
        {
            _view.DescriptionBox.Show();
            _view.DescriptionLabel.Hide();
            _view.DescriptionBox.ReadOnly = false;
            _view.DescriptionBox.Focus();
        }

        public void HideDescriptionBox()
        {
            _view.DescriptionBox.Hide();
            _view.DescriptionLabel.Show();
            _view.DescriptionBox.ReadOnly = true;
        }

        public void RefreshSystem()
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            bool isOnline = IsOnlineStatus();
            if (SystemInfoNotSet())
            {
                SetSystemInformation(isOnline);
            }
            Cursor.Current = Cursors.Default;
        }

        public void RunWorkstationSettings()
        {
            DialogResult changeResult = _appController.RunWorkstationSettings();
            if(changeResult == DialogResult.OK)
            {

            }
        }
    }
}

