using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UltimateAdmin.Core.Logging;

namespace UltimateAdmin.Core.ActiveDirectory
{
    public class ADQuery
    {
        public static List<User> QueryUserAD(string searchText, UserField searchField)
        {
            List<User> FoundUsers = new List<User>();
            using (PrincipalContext corp = new PrincipalContext(ContextType.Domain, ADContext.DomainSimpleName))
            {
                UserPrincipal user = new UserPrincipal(corp);
                if (searchField == UserField.Username) { user.SamAccountName = "*" + searchText + "*"; }
                else if (searchField == UserField.Name) { user.Name = "*" + searchText + "*"; }
                else if (searchField == UserField.EmployeeID) { user.EmployeeId = "*" + searchText + "*"; }
                PrincipalSearcher searcher = new PrincipalSearcher();
                searcher.QueryFilter = user;
                try
                {
                    var results = searcher.FindAll().Cast<UserPrincipal>();
                    UserPrincipal[] users = results.ToArray();
                    for (int i = 0; i < users.Length; i++)
                    {
                        FoundUsers.Add(new User(users[i]));
                    }
                    return FoundUsers;
                }
                catch (Exception e)
                {
                    Logger.Log("Exception while searching the directory: " + e.Message, MethodBase.GetCurrentMethod().Name);
                    return null;
                }
            }
        }

        //Used by ViewLoggedInUser to check whether logged in account is a domain account (vs. a local account).
        public static bool? isDomainAccount(string userName)
        {
            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Domain, ADContext.DomainSimpleName))
                {
                    using (var foundUser = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName))
                    {
                        return foundUser != null;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.Message, MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public static User GetUserByUserName(string user)
        {
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, ADContext.DomainSimpleName))
            {
                try
                {
                    UserPrincipal userP = new UserPrincipal(context);
                    userP.SamAccountName = user;
                    PrincipalSearcher userSearcher = new PrincipalSearcher(userP);
                    userSearcher.QueryFilter = userP;
                    UserPrincipal result = (UserPrincipal)userSearcher.FindOne();
                    User userItem = new User(result);
                    return userItem;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
    }
}


