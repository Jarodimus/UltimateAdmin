using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UltimateAdmin.Core.Logging;
using UltimateAdmin.Core.WMI;

namespace UltimateAdmin.Core.ActiveDirectory
{
    public class Computer : ADObject, IComparable
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Computer(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        public List<string> GetRDPUsers()
        {
            List<string> rdpUsers = new List<string>();
            using (DirectoryEntry machine = new DirectoryEntry("WinNT://" + this.Name))
            {
                using (DirectoryEntry group = machine.Children.Find("Remote Desktop Users", "Group"))
                {
                    object members = group.Invoke("Members", null);
                    StringBuilder resultMessage = new StringBuilder();
                    foreach (object member in (IEnumerable)members)
                    {
                        rdpUsers.Add(new DirectoryEntry(member).Name);
                    }
                }
            }
            return rdpUsers;
        }

        public List<string> GetLocalAdmins()
        {
            List<string> localAdmins = new List<string>();
            try
            {
                using (DirectoryEntry machine = new DirectoryEntry("WinNT://" + this.Name))
                {
                    using (DirectoryEntry group = machine.Children.Find("Administrators", "Group"))
                    {
                        object members = group.Invoke("Members", null);
                        StringBuilder resultMessage = new StringBuilder();
                        foreach (object member in (IEnumerable)members)
                        {
                            localAdmins.Add(new DirectoryEntry(member).Name);
                        }
                    }
                }
                return localAdmins;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            return localAdmins;
        }

        public bool AddLocalAdmin(string userName)
        {
            bool AdminAdded = false;
            using (DirectoryEntry machine = new DirectoryEntry("WinNT://" + this.Name + "/Administrators,group"))
            {
                var oUSerPath = "WinNT://DOMAIN/" + ADContext.DomainSimpleName + "/" + userName;
                machine.Invoke("Add", new object[] { oUSerPath });
                List<string> LocalAdmins = GetLocalAdmins();
                foreach (string s in LocalAdmins)
                {
                    if (s.Equals(userName))
                        AdminAdded = true;
                }
            }
            return AdminAdded;
        }

        public bool? RemoveLocalAdmin(string userName)
        {
            bool? AdminRemoved = false;
            if (userName != "Administrator")
            {
                try
                {
                    using (DirectoryEntry machine = new DirectoryEntry("WinNT://" + this.Name + "/Administrators,group"))
                    {
                        var selectedOUserPath = "WinNT://DOMAIN/" + ADContext.DomainSimpleName + "/" + userName;
                        machine.Invoke("Remove", new object[] { selectedOUserPath });
                        List<string> LocalAdmins = GetLocalAdmins();
                        foreach (string s in LocalAdmins)
                        {
                            if (LocalAdmins.Contains(userName))
                            {
                                AdminRemoved = false;
                            }
                            else
                            {
                                AdminRemoved = true;
                            }
                        }
                    }
                }
                catch (TargetInvocationException e)
                {
                    AdminRemoved = null;
                    Logger.Log("Error removing local user from Admin group:\n" + e.Message + "\nUser account may be local to the machine and not Active Directory.", MethodBase.GetCurrentMethod().Name);
                }
            }
            return AdminRemoved;
        }

        public bool AddRDPUser(string userName)
        {
            bool UserAdded = false;
            using (DirectoryEntry machine = new DirectoryEntry("WinNT://" + this.Name + "/Remote Desktop Users,group"))
            {
                var oUSerPath = "WinNT://DOMAIN/" + ADContext.DomainSimpleName + "/" + userName;
                machine.Invoke("Add", new object[] { oUSerPath });
                List<string> LocalRDPUsers = GetRDPUsers();
                foreach (string s in LocalRDPUsers)
                {
                    if (s.Equals(userName))
                        UserAdded = true;
                }
            }
            return UserAdded;
        }

        public bool RemoveRDPUser(string userName)
        {
            bool AdminRemoved = false;
            if (userName != "Administrator")
            {
                using (DirectoryEntry machine = new DirectoryEntry("WinNT://" + this.Name + "/Remote Desktop Users,group"))
                {
                    var selectedOUserPath = "WinNT://DOMAIN/" + ADContext.DomainSimpleName + "/" + userName;
                    machine.Invoke("Remove", new object[] { selectedOUserPath });
                    List<string> LocalRDPUsers = GetRDPUsers();
                    foreach (string s in LocalRDPUsers)
                    {
                        if (LocalRDPUsers.Contains(userName))
                        {
                            AdminRemoved = false;
                        }
                        else
                        {
                            AdminRemoved = true;
                        }
                    }
                }
            }
            return AdminRemoved;
        }

        public int CompareTo(object obj)
        {
            Computer system = (Computer)obj;
            return string.Compare(this.Name, system.Name);
        }
    }
}
