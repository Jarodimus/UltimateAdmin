using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using System.Windows.Forms;
using System.Threading;
using UltimateAdmin.Core.Logging;
using System.Reflection;
using System.DirectoryServices;

namespace UltimateAdmin.Core.ActiveDirectory
{
    public class ADWorkstationsQuery
    {
        public static List<Computer> GetComputers(string searchText, Category type, WorkstationSearchPreferences searchPrefs)
        {
            var selectedGroups = searchPrefs.SelectedGroups;
            if(searchPrefs.SearchEntireDirectory == true)
            {
                return QueryAllMachinesAD(searchText, type);
            }
            var ousToSearch = GetGroupOUs(searchPrefs.SelectedGroups);
            List<Computer> searchResults = QueryADMachines(searchText, type, ousToSearch);
            return searchResults;
        }

        public static List<Computer> QueryADMachines(string searchText, Category type, List<string> ousToSearch)
        {
            List<Computer> computerResults = new List<Computer>();
            List<Task<List<Computer>>> searchTaskList = new List<Task<List<Computer>>>();
            foreach(string ou in ousToSearch)
            {
                var queryTask = new Task<List<Computer>>(() => QueryADMachinesTask(searchText, ou, type));
                searchTaskList.Add(queryTask);
                queryTask.Start();
            }
            var result = Task.WhenAll(searchTaskList.ToArray());
            List<Computer>[] value = result.Result;
            for(int i = 0; i < value.Length; i++)
            {
                computerResults.AddRange(value[i]);
            }
            return computerResults;
        }

        public static List<Computer> QueryADMachinesTask(string searchText, string orgUnit, Category type)
        {
            if (string.IsNullOrEmpty(orgUnit))
                orgUnit = ADContext.GetDomainPath();
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, ADContext.DomainSimpleName, orgUnit))
            {
                ComputerPrincipal comp = new ComputerPrincipal(context);
                if (type == Category.Name) { comp.Name = "*" + searchText + "*"; }
                if (type == Category.Description) { comp.Description = "*" + searchText + "*"; }
                PrincipalSearcher searcher = new PrincipalSearcher();
                searcher.QueryFilter = comp;
                try
                {
                    List<Computer> computers = new List<Computer>();
                    var results = searcher.FindAll().Cast<ComputerPrincipal>();
                    ComputerPrincipal[] comps = results.ToArray();
                    for (int i = 0; i < comps.Length; i++)
                    {
                        computers.Add(new Computer(comps[i].Name, comps[i].Description));
                    }
                    return computers;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return null;
                }
            }
        }

        public static List<Computer> QueryAllMachinesAD(string searchText, Category type)
        {
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, ADContext.DomainSimpleName))
            {
                ComputerPrincipal comp = new ComputerPrincipal(context);
                if (type == Category.Name) { comp.Name = "*" + searchText + "*"; }
                if (type == Category.Description) { comp.Description = "*" + searchText + "*"; }
                PrincipalSearcher searcher = new PrincipalSearcher();
                searcher.QueryFilter = comp;
                try
                {
                    List<Computer> computers = new List<Computer>();
                    var results = searcher.FindAll().Cast<ComputerPrincipal>();
                    ComputerPrincipal[] comps = results.ToArray();
                    for (int i = 0; i < comps.Length; i++)
                    {
                        computers.Add(new Computer(comps[i].Name, comps[i].Description));
                    }
                    return computers;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return null;
                }
            }
        }

        private static List<string> GetGroupOUs(List<string> selectedGroups)
        {
            List<string> ousToSearch = new List<string>();
            for (int i = 0; i < selectedGroups.Count; i++)
            {
                List<string> groupOUs = ADContext.GetWorkstationGroupOUs(selectedGroups[i]);
                for (int j = 0; j < groupOUs.Count; j++)
                {
                    ousToSearch.Add(groupOUs[j]);
                }
            }
            return ousToSearch;
        }

        public static List<string> GetComputerGroups(string computerName)
        {
            List<string> Groups = new List<string>();
            using (PrincipalContext corp = new PrincipalContext(ContextType.Domain))
            {
                ComputerPrincipal comp = new ComputerPrincipal(corp);
                comp.Name = computerName;
                PrincipalSearcher search = new PrincipalSearcher();
                search.QueryFilter = comp;
                try
                {
                    StringBuilder groupList = new StringBuilder();
                    Principal result = search.FindOne();
                    foreach (Principal group in result.GetGroups())
                    {
                        Groups.Add(group.ToString());
                    }
                    return Groups;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    return null;
                }
            }
        }
    }
}

