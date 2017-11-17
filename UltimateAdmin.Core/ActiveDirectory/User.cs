using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UltimateAdmin.Core.ActiveDirectory
{
    public class User : ADObject, IComparable
    {
        public string Name { get; private set; }
        public string Username { get; private set; }

        public User(UserPrincipal user)
        {
            Name = user.DisplayName;
            Username = user.SamAccountName;
        }

        public int CompareTo(object obj)
        {
            User user = (User)obj;
            return string.Compare(this.Name, user.Name);
        }
    }
}
