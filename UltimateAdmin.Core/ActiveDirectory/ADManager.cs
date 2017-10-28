using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using UltimateAdmin.Core.Logging;

namespace UltimateAdmin.Core.ActiveDirectory
{
    public static class ADManager
    {
        public static bool UnlockAccount(string displayName)
        {
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, ADContext.DomainSimpleName))
            {
                UserPrincipal lockedUser = new UserPrincipal(context);
                lockedUser.DisplayName = displayName;
                PrincipalSearcher userSearcher = new PrincipalSearcher();
                userSearcher.QueryFilter = lockedUser;
                try
                {
                    UserPrincipal result = (UserPrincipal)userSearcher.FindOne();
                    if (result.IsAccountLockedOut())
                    {
                        result.UnlockAccount();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    Logger.Log(e.Message, MethodBase.GetCurrentMethod().Name);
                    return false;
                }
            }
        }

        public static bool UpdateComputerDescription(string computerName, string newDescription)
        {
            try
            {
                DirectoryEntry entry = new DirectoryEntry(ADContext.LDAP_Path);
                DirectorySearcher ds = new DirectorySearcher(entry);
                ds.Filter = "(&(objectCategory=Computer)(cn=" + computerName + "))";
                ds.PropertiesToLoad.Add("Description");
                SearchResult result = ds.FindOne();
                if (result != null)
                {
                    DirectoryEntry entryToUpdate = result.GetDirectoryEntry();
                    entryToUpdate.Properties["Description"].Value = newDescription;
                    entryToUpdate.CommitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.Message, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
    }
}
