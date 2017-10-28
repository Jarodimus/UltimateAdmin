using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UltimateAdmin.Core.ActiveDirectory;
using UltimateAdmin.Core.Logging;

namespace UltimateAdmin.Core.WMI
{
    public static class WMIQuery
    {
        public static string GetLoggedOnLocalUser(Computer Computer)
        {
            ConnectionOptions options = new ConnectionOptions();
            options.EnablePrivileges = true;
            options.Impersonation = ImpersonationLevel.Impersonate;
            options.Authentication = AuthenticationLevel.Packet;
            options.Timeout = TimeSpan.FromSeconds(25);
            if (Environment.MachineName != Computer.Name)
            {
                ManagementScope scope = new ManagementScope(string.Format(@"\\{0}\root\cimv2", Computer.Name), options);
                try
                {
                    scope.Connect();
                }
                catch (Exception e)
                {
                    return "Unable to query remote machine.  Try again later.";
                }
                ObjectQuery query = new ObjectQuery("Select * from win32_computersystem");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                ManagementObjectCollection querycollection = searcher.Get();
                if (querycollection.Count > 0)
                {
                    foreach (ManagementObject mobj in querycollection)
                    {
                        if (mobj["UserName"] != null)
                            return mobj["UserName"].ToString();
                    }
                    return "No users currently logged on.";
                }
                else
                {
                    return "No users currently logged on.";
                }
            }
            else
            {
                return Environment.UserDomainName + "\\" + Environment.UserName;
            }
        }

        public static List<string> GetLoggedOnUser(string computerName)
        {
            List<string> users = new List<string>();
            try
            {
                var scope = GetManagementScope(computerName);
                scope.Connect();
                var Query = new SelectQuery("SELECT LogonId  FROM Win32_LogonSession Where LogonType=10");
                var Searcher = new ManagementObjectSearcher(scope, Query);
                var regName = new Regex(@"(?<=Name="").*(?="")");

                foreach (ManagementObject WmiObject in Searcher.Get())
                {
                    foreach (ManagementObject LWmiObject in WmiObject.GetRelationships("Win32_LoggedOnUser"))
                    {
                        users.Add(regName.Match(LWmiObject["Antecedent"].ToString()).Value);
                    }
                }
            }
            catch (Exception ex)
            {
                users.Add(ex.Message);
            }

            return users;
        }

        private static ManagementScope GetManagementScope(string machineName)
        {
            ManagementScope Scope;

            if (machineName.Equals("localhost", StringComparison.OrdinalIgnoreCase))
                Scope = new ManagementScope(String.Format("\\\\{0}\\root\\CIMV2", "."), GetConnectionOptions());
            else
            {
                Scope = new ManagementScope(String.Format("\\\\{0}\\root\\CIMV2", machineName), GetConnectionOptions());
            }
            return Scope;
        }

        private static ConnectionOptions GetConnectionOptions()
        {
            var connection = new ConnectionOptions
            {
                EnablePrivileges = true,
                Authentication = AuthenticationLevel.PacketPrivacy,
                Impersonation = ImpersonationLevel.Impersonate,
            };
            return connection;
        }

        public static string GetBIOSVersion(string computerName)
        {
            string BIOSVersion = "Unknown";
            try
            {
                ConnectionOptions connection = new ConnectionOptions();
                connection.Authority = "ntlmdomain:CORP";

                ManagementScope scope = new ManagementScope(
                    "\\\\" + computerName + "\\root\\CIMV2", connection);
                scope.Connect();

                ObjectQuery query = new ObjectQuery(
                    "SELECT * FROM Win32_BIOS");

                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(scope, query);

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    BIOSVersion = queryObj["SMBIOSBIOSVersion"].ToString();
                }
                return BIOSVersion;
            }
            catch (ManagementException err)
            {
                Logger.Log("An error occurred while querying for WMI data: " + err.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (System.UnauthorizedAccessException unauthorizedErr)
            {
                Logger.Log("Connection error (user name or password might be incorrect): " + unauthorizedErr.Message, MethodBase.GetCurrentMethod().Name);
            }
            return BIOSVersion;
        }

        public static string GetMakeModel(string computerName)
        {
            string Make = "";
            string Model = "";
            string MakeModel = "";

            //Get Model
            try
            {
                ConnectionOptions connection = new ConnectionOptions();
                connection.Authority = "ntlmdomain:" + ADContext.DomainSimpleName;
                 
                ManagementScope scope = new ManagementScope(
                    "\\\\" + computerName + "\\root\\CIMV2", connection);
                scope.Connect();

                ObjectQuery query = new ObjectQuery(
                    "SELECT * FROM Win32_ComputerSystem");

                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(scope, query);

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Make = FormatManufacturer(queryObj["Manufacturer"].ToString());
                    Model = queryObj["Model"].ToString();
                    if (Model.Contains("HP"))
                    {
                        Make = "";
                    }
                    MakeModel = (Make + Model).Trim();
                }
            }
            catch (ManagementException err)
            {
                Logger.Log("An error occurred while querying for WMI data: " + err.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (System.UnauthorizedAccessException unauthorizedErr)
            {
                Logger.Log("Connection error (user name or password might be incorrect): " + unauthorizedErr.Message, MethodBase.GetCurrentMethod().Name);
            }
            return MakeModel;
        }

        private static string FormatManufacturer(string make)
        {
            if (make.Contains("Dell"))
            {
                return "Dell";
            }
            if (make.Contains("HP") || make.Contains("Hewlett"))
            {
                return "HP";
            }
            if (make.Contains("Lenovo"))
            {
                return "Lenovo";
            }
            return make;
        }

        public static string GetSystemSerialNumber(string computerName)
        {
            string serial = "Unknown";

            try
            {
                ConnectionOptions connection = new ConnectionOptions();
                connection.Authority = "ntlmdomain:" + ADContext.DomainSimpleName;

                ManagementScope scope = new ManagementScope(
                    "\\\\" + computerName + "\\root\\CIMV2", connection);
                scope.Connect();

                ObjectQuery query = new ObjectQuery(
                    "SELECT * FROM Win32_BIOS");

                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(scope, query);

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    serial = queryObj["SerialNumber"].ToString();
                }
                return serial;
            }
            catch (ManagementException err)
            {
                Logger.Log("An error occurred while querying for WMI data: " + err.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (System.UnauthorizedAccessException unauthorizedErr)
            {
                Logger.Log("Connection error (user name or password might be incorrect): " + unauthorizedErr.Message, MethodBase.GetCurrentMethod().Name);
            }
            return serial;
        }

        public static string GetOSVersion(string computerName)
        {
            string OSVersion = "Unknown";
            try
            {
                ConnectionOptions connection = new ConnectionOptions();
                connection.Authority = "ntlmdomain:" + ADContext.DomainSimpleName;

                ManagementScope scope = new ManagementScope(
                    "\\\\" + computerName + "\\root\\CIMV2", connection);
                scope.Connect();

                ObjectQuery query = new ObjectQuery(
                    "SELECT * FROM Win32_OperatingSystem");

                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(scope, query);

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    OSVersion = queryObj["Caption"].ToString();
                    queryObj.Dispose();
                    return OSVersion;
                }
            }
            catch (ManagementException err)
            {
                Logger.Log("An error occurred while querying for WMI data: " + err.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (System.UnauthorizedAccessException unauthorizedErr)
            {
                Logger.Log("Connection error (user name or password might be incorrect): " + unauthorizedErr.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception e)
            {

            }
            return OSVersion;
        }

        public static string GetFormFactor(string computerName)
        {
            string SystemType = "unavailable";
            try
            {
                ConnectionOptions connection = new ConnectionOptions();
                connection.Authority = "ntlmdomain:" + ADContext.DomainSimpleName;

                ManagementScope scope = new ManagementScope(
                    "\\\\" + computerName + "\\root\\CIMV2", connection);
                scope.Connect();

                ObjectQuery query = new ObjectQuery(
                    "SELECT * FROM Win32_Battery");

                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(scope, query);

                var queryObjCollection = searcher.Get();
                if (queryObjCollection.Count < 1)
                {
                    return "Desktop";
                }
                else
                {
                    foreach (ManagementObject queryObj in queryObjCollection)
                    {
                        string value = queryObj["Availability"].ToString();
                        int intValue = int.Parse(value);
                        if (intValue > 0)
                        {
                            SystemType = "Laptop";
                            return SystemType;
                        }
                        else
                        {
                            SystemType = "Desktop";
                            return SystemType;
                        }
                    }
                }
            }
            catch (COMException comErr)
            {
                Logger.Log(comErr.Message + "\n\nUnable to communicate with machine.  Check Firewall.", MethodBase.GetCurrentMethod().Name);
            }
            catch (ManagementException err)
            {
                Logger.Log("An error occurred while querying for WMI data: " + err.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (System.UnauthorizedAccessException unauthorizedErr)
            {
                Logger.Log("Connection error (user name or password might be incorrect): " + unauthorizedErr.Message, MethodBase.GetCurrentMethod().Name);
            }
            return SystemType;
        }

        public static string GetSystemType(string computerName)
        {
            string systemType = "Unknown";
            try
            {
                ConnectionOptions connection = new ConnectionOptions();
                connection.Authority = "ntlmdomain:" + ADContext.DomainSimpleName;

                ManagementScope scope = new ManagementScope(
                    "\\\\CHW-7PRINTROOM\\root\\CIMV2", connection);
                scope.Connect();

                ObjectQuery query = new ObjectQuery(
                    "SELECT * FROM Win32_ComputerSystem");

                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(scope, query);

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    int value = 0;
                    int.TryParse(queryObj["PCSystemType"].ToString(), out value);
                    systemType = GetTypeName(value);
                }
            }
            catch (ManagementException err)
            {
                Logger.Log("An error occurred while querying for WMI data: " + err.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (System.UnauthorizedAccessException unauthorizedErr)
            {
                Logger.Log("Connection error (user name or password might be incorrect): " + unauthorizedErr.Message, MethodBase.GetCurrentMethod().Name);
            }
            return systemType;
        }

        private static string GetTypeName(int typeValue)
        {
            switch (typeValue)
            {
                case 0:
                    return "Unspecified";
                case 1:
                    return "Desktop";
                case 2:
                    return "Mobile";
                case 3:
                    return "Workstation";
                case 4:
                    return "Enterprise Server";
                case 5:
                    return "Small/Home Office Server";
                case 6:
                    return "Appliance PC";
                case 7:
                    return "Performance Server";
                case 8:
                    return "Maximum";
                default:
                    return "Unknown";
            }
        }

        public static string GetEnclosureType(string computerName)
        {
            string systemType = "Unknown";
            try
            {
                ConnectionOptions connection = new ConnectionOptions();
                connection.Authority = "ntlmdomain:" + ADContext.DomainSimpleName;

                ManagementScope scope = new ManagementScope(
                    "\\\\CHW-7PRINTROOM\\root\\CIMV2", connection);
                scope.Connect();

                ObjectQuery query = new ObjectQuery(
                    "SELECT * FROM Win32_SystemEnclosure");

                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(scope, query);

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    int value = 0;
                    int.TryParse(queryObj["ChassisTypes"].ToString(), out value);
                    systemType = GetEnclosureType(value);
                }
            }
            catch (ManagementException err)
            {
                Logger.Log("An error occurred while querying for WMI data: " + err.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (System.UnauthorizedAccessException unauthorizedErr)
            {
                Logger.Log("Connection error (user name or password might be incorrect): " + unauthorizedErr.Message, MethodBase.GetCurrentMethod().Name);
            }
            return systemType;
        }

        private static string GetEnclosureType(int typeValue)
        {
            switch (typeValue)
            {
                case 1:
                    return "Other";
                case 2:
                    return "Unknown";
                case 3:
                    return "Desktop";
                case 4:
                    return "Low Profile Desktop";
                case 5:
                    return "Pizza Box";
                case 6:
                    return "Mini Tower";
                case 7:
                    return "Tower";
                case 8:
                    return "Portable";
                case 9:
                    return "Laptop";
                case 10:
                    return "Notebook";
                case 11:
                    return "Hand Held";
                case 12:
                    return "Docking Station";
                case 13:
                    return "All in One";
                case 14:
                    return "Sub Notebook";
                case 15:
                    return "Space-Saving";
                case 16:
                    return "Lunch Box";
                case 17:
                    return "Main System Chassis";
                case 18:
                    return "Expansion Chassis";
                case 19:
                    return "Sub Chassis";
                case 20:
                    return "Bus Expansion Chassis";
                case 21:
                    return "Peripheral Chassis";
                case 22:
                    return "Storage Chassis";
                case 23:
                    return "Rack Mount Chassis";
                case 24:
                    return "Sealed-Case PC";
                default:
                    return "Unknown";
            }
        }

        public static List<string> ReadRemoteRegistryusingWMI(string machineName)
        {
            List<string> programs = new List<string>();

            ConnectionOptions connectionOptions = new ConnectionOptions();
            //connectionOptions.Impersonation = ImpersonationLevel.Impersonate;
            ManagementScope scope = new ManagementScope("\\\\" + machineName + "\\root\\CIMV2", connectionOptions);
            scope.Connect();

            string softwareRegLoc = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";

            ManagementClass registry = new ManagementClass(scope, new ManagementPath("StdRegProv"), null);
            ManagementBaseObject inParams = registry.GetMethodParameters("EnumKey");
            inParams["hDefKey"] = 0x80000002;//HKEY_LOCAL_MACHINE
            inParams["sSubKeyName"] = softwareRegLoc;

            // Read Registry Key Names 
            ManagementBaseObject outParams = registry.InvokeMethod("EnumKey", inParams, null);
            string[] programGuids = outParams["sNames"] as string[];

            foreach (string subKeyName in programGuids)
            {
                inParams = registry.GetMethodParameters("GetStringValue");
                inParams["hDefKey"] = 0x80000002;//HKEY_LOCAL_MACHINE
                inParams["sSubKeyName"] = softwareRegLoc + @"\" + subKeyName;
                inParams["sValueName"] = "DisplayName";
                // Read Registry Value 
                outParams = registry.InvokeMethod("GetStringValue", inParams, null);

                if (outParams.Properties["sValue"].Value != null)
                {
                    string softwareName = outParams.Properties["sValue"].Value.ToString();
                    programs.Add(softwareName);
                }
            }

            return programs;
        }


        // Used to see who is logged on a Remote Machine by querying explorer.exe and associated user account. 
        // Tested, but found better implementation using Process & query user.
        //
        //public static List<string> GetLoggedOnUsers(string computerName)
        //{
        //    List<string> remoteUsers = new List<string>();
        //    try
        //    {
        //        ConnectionOptions connection = new ConnectionOptions();
        //        //connection.Authority = "ntlmdomain:CORP";
        //        ManagementScope scope = new ManagementScope(
        //            "\\\\" + computerName + "\\root\\CIMV2", connection);
        //        scope.Connect();
        //        ObjectQuery query = new ObjectQuery(
        //            "SELECT * FROM Win32_Process WHERE Name = 'explorer.exe'");
        //        ManagementObjectSearcher searcher =
        //            new ManagementObjectSearcher(scope, query);
        //        foreach (ManagementObject queryObj in searcher.Get())
        //        {
        //            ManagementPath path = new ManagementPath("Win32_Process.Handle='" + queryObj["Handle"] + "'");
        //            ManagementObject classInstance = new ManagementObject(scope, path, null);
        //            ManagementBaseObject outParams = classInstance.InvokeMethod("GetOwner", null, null);
        //            remoteUsers.Add(outParams["User"].ToString());
        //        }
        //        return remoteUsers;
        //    }
        //    catch (ManagementException err)
        //    {
        //        return null;
        //    }
        //    catch (System.UnauthorizedAccessException unauthorizedErr)
        //    {
        //        return null;
        //    }
        //}
    }
}