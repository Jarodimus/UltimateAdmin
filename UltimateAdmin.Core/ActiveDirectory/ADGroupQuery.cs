using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UltimateAdmin.Core.Logging;

namespace UltimateAdmin.Core.ActiveDirectory
{
    public static class ADGroupQuery
    {
        public static ADGroup GetGroup(string groupName)
        {
            using (PrincipalContext corp = new PrincipalContext(ContextType.Domain, ADContext.DomainSimpleName))
            {
                GroupPrincipal groupPrincipal = new GroupPrincipal(corp);
                groupPrincipal.Name = groupName;

                PrincipalSearcher searcher = new PrincipalSearcher();
                searcher.QueryFilter = groupPrincipal;
                try
                {
                    var group = (GroupPrincipal) searcher.FindOne();
                    var members = group.GetMembers();
                    Dictionary<string, string> memberNameToDescriptionMap = new Dictionary<string, string>();
                    if (members.Count() > 0)
                    {
                        foreach (var member in members)
                        {
                            memberNameToDescriptionMap.Add(member.Name, member.Description);
                        }
                    }
                    return new ADGroup(group.Name, group.Description, memberNameToDescriptionMap);
                }
                catch (Exception e)
                {
                    Logger.Log("Exception while searching the directory: " + e.Message, MethodBase.GetCurrentMethod().Name);
                    return null;
                }
            }
        }
    }
}
