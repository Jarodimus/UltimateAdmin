using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Security;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UltimateAdmin.Core.Logging;

namespace UltimateAdmin.Core.ActiveDirectory
{
    public class PowerShellEnvironment
    {
        public static Dictionary<string, PSResult> ResetADPassword(string userName, SecureString newPassword, bool forceChange, bool unlock)
        {
            Logger.Log("Initiating AD password reset...", MethodBase.GetCurrentMethod().Name);
            Dictionary<string, PSResult> ResultValueMap = new Dictionary<string, PSResult>();
            PSResult resetSuccess = new PSResult("ResetSuccess");
            PSResult unlockStatus = new PSResult("UnlockSuccess");
            if (newPassword != null)
            {
                Logger.Log("New password not null.  Continuing...", MethodBase.GetCurrentMethod().Name);
                if (unlock && IsADAccountLocked(userName))
                {
                    Logger.Log("AD Account is locked. User selected option to unlock.  Attempting ot unlock.", MethodBase.GetCurrentMethod().Name);
                    bool unlockSuccess = UnlockADAccount(userName);
                    Logger.Log("Unlock successful " + unlockSuccess, MethodBase.GetCurrentMethod().Name);
                    unlockStatus.SetSuccessValue(unlockSuccess);
                    ResultValueMap.Add(unlockStatus.Name, unlockStatus);
                } else
                {
                    if (unlock)
                    {
                        Logger.Log("User selected option to unlock but account was not locked.", MethodBase.GetCurrentMethod().Name);
                    }
                    else
                    {
                        Logger.Log("User did not select option to unlock.", MethodBase.GetCurrentMethod().Name);
                    }
                    unlockStatus.SetSuccessValue(false);
                    unlockStatus.SetMessage("");
                    ResultValueMap.Add(unlockStatus.Name, unlockStatus);
                }

                using (PowerShell PowerShellInstance = PowerShell.Create())
                {
                    PowerShellInstance.AddCommand("Set-ADAccountPassword");
                    PowerShellInstance.AddParameter("-Reset");
                    PowerShellInstance.AddParameter("-NewPassword", newPassword);
                    PowerShellInstance.AddParameter("Identity", userName);
                    PowerShellInstance.AddStatement();
                    if (forceChange)
                    {
                        Logger.Log("User selected option to have the password force changed.", MethodBase.GetCurrentMethod().Name);
                        PowerShellInstance.AddCommand("Set-ADUser");
                        PowerShellInstance.AddParameter("Identity", userName);
                        PowerShellInstance.AddParameter("-ChangePasswordAtLogon", true);
                        PowerShellInstance.AddStatement();
                    }
                    try
                    {
                        Collection<PSObject> psOutput = PowerShellInstance.Invoke();
                        resetSuccess.SetSuccessValue(true);
                        resetSuccess.SetMessage("Password reset successfully.");
                        ResultValueMap.Add(resetSuccess.Name, resetSuccess);
                        Logger.Log("Password was reset.", MethodBase.GetCurrentMethod().Name);
                    } catch(CmdletInvocationException e)
                    {
                        if(e.Message.Equals("The password does not meet the length, complexity, or history requirement of the domain."))
                        {
                            Logger.Log(e.Message, MethodBase.GetCurrentMethod().Name);
                            resetSuccess.SetSuccessValue(false);
                            resetSuccess.SetMessage(e.Message);
                            ResultValueMap.Add(resetSuccess.Name, resetSuccess);
                        }
                    } catch(Exception e)
                    {
                        resetSuccess.SetSuccessValue(false);
                        resetSuccess.SetMessage(e.Message);
                        ResultValueMap.Add(resetSuccess.Name, resetSuccess);
                        Logger.Log("Exception resetting password for user: " + userName, MethodBase.GetCurrentMethod().Name);
                    }
                }
                return ResultValueMap;
            }
            return null;
        }

        //public static bool GetLoggedInUsers(string computer)
        //{
        //    bool isLocked = false;
        //    using (PowerShell PowerShellInstance = PowerShell.Create())
        //    {
        //        PowerShellInstance.AddCommand("Get-ADUser");
        //        PowerShellInstance.AddParameter("-Identity", userName);
        //        PowerShellInstance.AddParameter("-Properties", "LockedOut");
        //        Collection<PSObject> psOutput = PowerShellInstance.Invoke();
        //        foreach (PSObject psObj in psOutput)
        //        {
        //            string lockedOutStatus = psObj.Properties["LockedOut"].Value.ToString();
        //            isLocked = bool.Parse(lockedOutStatus);
        //        }
        //    }
        //    return isLocked;
        //}

        public static bool UnlockADAccount(string userName)
        {
            bool Success = false;
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddCommand("Unlock-ADAccount");
                PowerShellInstance.AddParameter("-Identity", userName);
                PowerShellInstance.AddParameter("-PassThru");
                Collection<PSObject> psOutput = PowerShellInstance.Invoke();
                foreach (PSObject psObj in psOutput)
                {
                    if (psObj != null)
                    {
                        bool isLocked = IsADAccountLocked(userName);
                        if (isLocked)
                        {
                            Success = false;
                        }
                        else
                        {
                            Success = true;
                        }
                    }
                }
            }
            return Success;
        }

        public static bool IsADAccountLocked(string userName)
        {
            bool isLocked = false;
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                StringBuilder sb = new StringBuilder();
                PowerShellInstance.AddCommand("Get-ADUser");
                PowerShellInstance.AddParameter("-Identity", userName);
                PowerShellInstance.AddParameter("-Properties", "LockedOut");
                Collection<PSObject> psOutput = PowerShellInstance.Invoke();
                foreach(PSObject psObj in psOutput)
                {
                    string lockedOutStatus = psObj.Properties["LockedOut"].Value.ToString();
                    isLocked = bool.Parse(lockedOutStatus);
                }
            }
            return isLocked;
        }
    }
}

