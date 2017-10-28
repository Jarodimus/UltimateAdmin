using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UltimateAdmin.Core.ActiveDirectory
{
    public static class ADInfo
    {
        public static bool DoesPathExist(string objectPath)
        {
            try
            {
                if (DirectoryEntry.Exists("LDAP://" + objectPath))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine();
            }
            return false;
        }

        private static string GetComputerDN(string computerName)
        {
            string computerDN;
            try
            {
                DirectoryEntry entry = new DirectoryEntry(ADContext.LDAP_Path);
                DirectoryEntries entries = entry.Children;
                DirectorySearcher ds = new DirectorySearcher(entry);
                ds.Filter = "(&(objectCategory=Computer)(cn=" + computerName + "))";
                ds.SearchScope = SearchScope.Subtree;
                SearchResult item = ds.FindOne();
                if (item != null)
                {
                    computerDN = item.GetDirectoryEntry().Properties["distinguishedname"].Value.ToString();
                    return computerDN;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        public static string GetComputerOU(string computerName)
        {
            string computerDN = GetComputerDN(computerName);
            if (computerDN != null)
            {
                string result = "";
                string ouPattern = @"OU=(\w*),";
                MatchCollection matches = Regex.Matches(computerDN, ouPattern);
                if (matches.Count != 0)
                {
                    foreach (Match match in matches)
                    {
                        if (result.Contains("OU=us,"))
                        {
                            string subMatch = match.ToString();
                            MessageBox.Show(subMatch);
                        }
                        else
                        {
                            result += match.Value;
                        }
                    }
                    result = result.Remove(result.Length - 1);
                    return result;
                }
                else
                {
                    string cnPattern = @"CN=(\w*),";
                    string cnResult = "";
                    MatchCollection cnMatches = Regex.Matches(computerDN, cnPattern);
                    if (cnMatches != null)
                    {
                        foreach (Match match in cnMatches)
                        {
                            cnResult = match.ToString();
                        }
                        cnResult = cnResult.Remove(cnResult.Length - 1);
                        return cnResult;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        public static string GetBLRecoveryKey(string computerName)
        {
            string recoveryKey;
            string computerDN;
            try
            {
                DirectoryEntry entry = new DirectoryEntry(ADContext.LDAP_Path);
                DirectoryEntries entries = entry.Children;
                DirectorySearcher ds = new DirectorySearcher(entry);
                ds.Filter = "(&(objectCategory=Computer)(cn=" + computerName + "))";
                ds.SearchScope = SearchScope.Subtree;
                SearchResult item = ds.FindOne();
                if (item != null)
                {
                    computerDN = item.GetDirectoryEntry().Properties["distinguishedname"].Value.ToString();
                    entry = new DirectoryEntry(("LDAP://" + computerDN));
                    entries = entry.Children;
                    ds = new DirectorySearcher(entry);
                    ds.Filter = "(objectClass=msFVE-RecoveryInformation)";
                    ds.SearchScope = SearchScope.Subtree;
                    SearchResultCollection result = ds.FindAll();
                    if (result.Count == 1)
                    {
                        SearchResult sr = result[0];
                        string recoveryPassword = sr.GetDirectoryEntry().Properties["msFVE-RecoveryPassword"].Value.ToString();
                        recoveryKey = recoveryPassword;
                        return recoveryKey;
                    }
                    if (result.Count > 1)
                    {
                        DateTime latest = default(DateTime);
                        string newestBLKey = "Recovery key unavailable.";

                        Dictionary<string, DateTime> bitLockerEntries = new Dictionary<string, DateTime>();
                        SearchResultCollection searchResults = result;
                        foreach (SearchResult resultValue in searchResults)
                        {
                            string key = resultValue.GetDirectoryEntry().Properties["msFVE-RecoveryPassword"].Value.ToString();
                            DateTime timeValue = (DateTime)resultValue.GetDirectoryEntry().Properties["whencreated"].Value;
                            bitLockerEntries.Add(key, timeValue);
                            newestBLKey = key;
                            latest = timeValue;
                        }
                        foreach (KeyValuePair<string, DateTime> pair in bitLockerEntries)
                        {
                            if (DateTime.Compare(latest, pair.Value) < 0)
                            {
                                newestBLKey = pair.Key;
                                latest = pair.Value;
                            }
                        }
                        recoveryKey = newestBLKey;
                        return recoveryKey;
                    }
                    else
                    {
                        return "Recovery key unavailable.";
                    }
                }
                return "Recovery key unavailable.";
            }
            catch (Exception e)
            {
                MessageBox.Show("GetBLRecovery " + e.Message);
                return null;
            }
        }

        public static string GetComputerDescription(string computerName)
        {
            string machineDescription = "";
            try
            {
                DirectoryEntry entry = new DirectoryEntry(ADContext.LDAP_Path);
                DirectoryEntries entries = entry.Children;
                DirectorySearcher ds = new DirectorySearcher(entry);
                ds.Filter = "(&(objectCategory=Computer)(cn=" + computerName + "))";
                ds.SearchScope = SearchScope.Subtree;
                SearchResult item = ds.FindOne();
                if (item != null)
                {
                    string computerDN = GetComputerDN(computerName);
                    entry = new DirectoryEntry(("LDAP://" + computerDN));
                    entries = entry.Children;
                    ds = new DirectorySearcher(entry);
                    ds.SearchScope = SearchScope.Subtree;
                    SearchResult result = ds.FindOne();
                    if (result != null)
                    {
                        machineDescription = result.GetDirectoryEntry().Properties["Description"].Value.ToString();
                        return machineDescription;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        ///////////////
        /////////////
        ///////////////////
        /// TEST METHODS ////
        //////////////////
        ////////////
        //////////////
        public static Dictionary<string, List<SearchResult>> GetMachinesWithMultipleBLKeys()
        {
            Dictionary<string, List<SearchResult>> ComputersMappedToMultipleKeys = new Dictionary<string, List<SearchResult>>();
            string recoveryKey;
            string computerDN;
            try
            {
                DirectoryEntry entry = new DirectoryEntry(ADContext.LDAP_Path);
                DirectoryEntries entries = entry.Children;
                DirectorySearcher ds = new DirectorySearcher(entry);
                ds.Filter = "(&(objectCategory=Computer)(cn=*FLD-5CD*))";
                ds.SearchScope = SearchScope.Subtree;
                SearchResultCollection items = ds.FindAll();
                if (items != null)
                {
                    foreach (SearchResult resultItem in items)
                    {
                        List<SearchResult> BitLockerKeysList = new List<SearchResult>();
                        computerDN = resultItem.GetDirectoryEntry().Properties["distinguishedname"].Value.ToString();
                        string computerName = "";
                        string pattern = @"CN=\w+\-\w+";
                        Match match = Regex.Match(computerDN, pattern);
                        computerName = match.Groups[0].Value;
                        entry = new DirectoryEntry(("LDAP://" + computerDN));
                        entries = entry.Children;
                        ds = new DirectorySearcher(entry);
                        ds.Filter = "(objectClass=msFVE-RecoveryInformation)";
                        ds.SearchScope = SearchScope.Subtree;
                        SearchResultCollection blKeys = ds.FindAll();
                        if (blKeys.Count > 1)
                        {
                            foreach (SearchResult blKey in blKeys)
                            {
                                BitLockerKeysList.Add(blKey);
                            }
                            ComputersMappedToMultipleKeys.Add(computerName, BitLockerKeysList);
                        }
                    }
                    if (ComputersMappedToMultipleKeys.Count < 1)
                        return null;
                    return ComputersMappedToMultipleKeys;
                }
                return null;
            }
            catch (Exception e)
            {
                MessageBox.Show("GetMachinesWithMultipleBLKeys:\n" + e.Message);
                return null;
            }
        }


        public static Dictionary<string, string> GetLatestKey(List<SearchResult> bitLockerEntries)
        {
            Dictionary<string, string> LatestKeyDate = new Dictionary<string, string>();

            string newestBLKey = "";
            DateTime latestTime = default(DateTime);

            Dictionary<string, DateTime> bitLockerKeys = new Dictionary<string, DateTime>();
            foreach(SearchResult result in bitLockerEntries)
            {
                string key = result.GetDirectoryEntry().Properties["msFVE-RecoveryPassword"].Value.ToString();
                DateTime timeValue = (DateTime)result.GetDirectoryEntry().Properties["whencreated"].Value;
                try
                {
                    bitLockerKeys.Add(key, timeValue);
                } catch(ArgumentException e)
                {
                    string newKey = key + "*****Appended******";
                    bitLockerKeys.Add(newKey, timeValue);
                }
                latestTime = timeValue;
            }

            foreach (KeyValuePair<string, DateTime> pair in bitLockerKeys)
            {
                newestBLKey = pair.Key;
                if (DateTime.Compare(latestTime, pair.Value) < 0)
                {
                    newestBLKey = pair.Key;
                    latestTime = pair.Value;
                }
            }

            LatestKeyDate.Add(newestBLKey, latestTime.ToLongDateString());
            return LatestKeyDate;
        }
    }
}
