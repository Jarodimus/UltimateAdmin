using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace UltimateAdmin.Core.ActiveDirectory
{
    public static class ADContext
    {
        private static string _domainSimpleName;

        public static string DomainSimpleName
        {
            get
            {
                return _domainSimpleName;
            }
        }

        private static string _domainFQDN;

        public static string DomainFullyQualifiedName
        {
            get
            {
                return _domainFQDN;
            }
        }

        private static string _ldapPath;

        public static string LDAP_Path
        {
            get
            {
                return _ldapPath;
            }
        }

        private static string aliasOUString;

        private static Dictionary<string, string> _ouAliasToStringMap = new Dictionary<string, string>();
        public static Dictionary<string,string> UserAliasToOUMap
        {
            get
            {
                return _ouAliasToStringMap;
            }
        }
        public static Dictionary<string, string> OUStrings
        {
            get
            {
                return _ouAliasToStringMap;
            }
        }

        private static Dictionary<string, string> _ouStringToAliasMap = new Dictionary<string, string>();

        private static Dictionary<string, List<string>> workstationGroups = new Dictionary<string, List<string>>();

        static ADContext()
        {
            try
            {
                _domainFQDN = Domain.GetComputerDomain().Name;
                _domainSimpleName = GetDomainSimpleName();
                if (_domainFQDN != null)
                {
                    _ldapPath = GetLDAPPath(_domainFQDN);
                }
                else
                {
                    _ldapPath = null;
                }
            }
            catch (COMException e)
            {
                string msg = e.Message;
                _domainFQDN = null;
            }
        }

        private static void LoadPreferences()
        {
            ADContextPreferenceLoader.LoadPreferences();
        }

        //private static void SetUserOUsFromSettings()
        //{
        //    string userOUString = "";
        //    if (userOUString.Length > 0)
        //    {
        //        if (userOUString.Contains(";"))
        //        {
        //            string[] rawOUStrings = userOUString.Split(';');
        //            for (int i = 0; i < rawOUStrings.Length; i++)
        //            {
        //                if (string.IsNullOrEmpty(rawOUStrings[i]))
        //                {
        //                    continue;
        //                }
        //                ParseAndSetOUString(rawOUStrings[i]);
        //            }
        //        }
        //        else
        //        {
        //            ParseAndSetOUString(userOUString);
        //        }
        //    }
        //}

        public static void Initialize()
        {
            LoadPreferences();
        }

        public static string GetDomainPath()
        {
            string ldapPath = null;
            string pattern = "LDAP://(?<ldapShort>.*)";
            Regex reg = new Regex(pattern);
            if (reg.IsMatch(LDAP_Path))
            {
                var matches = reg.Matches(LDAP_Path);
                foreach(Match match in matches)
                {
                    ldapPath = match.Groups["ldapShort"].Value;
                }
            }
            return ldapPath;
        }

        public static int GetWorkstationGroupCount()
        {
            return workstationGroups.Count;
        }

        public static bool AddWorkstationGroup(string alias)
        {
            if (workstationGroups.ContainsKey(alias))
            {
                return false;
            }
            else
            {
                workstationGroups.Add(alias, null);
                return true;
            }
        }

        public static bool? AddWorkstationGroupOU(string alias, string OU)
        {
            if (workstationGroups.Keys.Contains(alias))
            {
                List<string> currentList;
                if (workstationGroups.TryGetValue(alias, out currentList))
                {
                    if (currentList != null)
                    {
                        if (currentList.Contains(OU))
                        {
                            return false;
                        }
                        else
                        {
                            workstationGroups.Remove(alias);
                            currentList.Add(OU);
                            workstationGroups.Add(alias, currentList);
                            return true;
                        }
                    }
                    else
                    {
                        workstationGroups.Remove(alias);
                        workstationGroups.Add(alias, new List<string> { OU });
                        return true;
                    }
                }
            }
            return null;
        }

        public static bool RemoveWorkstationGroup(string alias)
        {
            if (!workstationGroups.ContainsKey(alias))
            {
                return false;
            }
            else
            {
                workstationGroups.Remove(alias);
                return true;
            }
        }

        public static bool RemoveWorkstationGroupOU(string alias, string OU)
        {
            if (workstationGroups.ContainsKey(alias))
            {
                List<string> groups = workstationGroups[alias];
                groups.Remove(OU);
                workstationGroups.Remove(alias);
                workstationGroups.Add(alias, groups);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<string> GetWorkstationGroups()
        {
            return workstationGroups.Keys.ToList();
        }

        public static List<string> GetWorkstationGroupOUs(string groupName)
        {
            List<string> groups = new List<string>();
            workstationGroups.TryGetValue(groupName, out groups);
            if(groups != null && groups.Count > 0)
            {
                groups.Sort();
            }
            return groups;
        }

        //public static List<string> GetUserAliases()
        //{
        //    return _ouAliasToStringMap.Keys.ToList();
        //}

        //private static void ParseAndSetOUString(string ouString)
        //{
        //    string[] aliasOU = ouString.Split('_');
        //    _ouAliasToStringMap.Add(aliasOU[0], aliasOU[1]);
        //    _ouStringToAliasMap.Add(aliasOU[1], aliasOU[0]);
        //}

        //public static bool? AddOUString(string alias, string ou)
        //{
        //    if (_ouAliasToStringMap.ContainsKey(alias))
        //    {
        //        return null;
        //    }
        //    else if (_ouStringToAliasMap.ContainsKey(ou))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        _ouAliasToStringMap.Add(alias, ou);
        //        _ouStringToAliasMap.Add(ou, alias);
        //    }
        //    aliasOUString = "";
        //    CreateUserOUSettingString();
        //    return true;
        //}

        //public static void ChangeOUString(string alias, string ou)
        //{
        //    if (_ouAliasToStringMap.ContainsKey(alias))
        //    {
        //        if (_ouAliasToStringMap[alias] == ou)
        //        {
        //            return;
        //        }
        //        else
        //        {
        //            _ouAliasToStringMap.Remove(alias);
        //            _ouAliasToStringMap.Add(alias, ou);
        //            RemovePairFromStringToAliasMap(ou);
        //            _ouStringToAliasMap.Add(ou, alias);
        //        }
        //    }
        //    else if (_ouStringToAliasMap.ContainsKey(ou))
        //    {
        //        if (_ouStringToAliasMap[ou] == alias)
        //        {
        //            return;
        //        }
        //        else
        //        {
        //            _ouStringToAliasMap.Remove(ou);
        //            _ouStringToAliasMap.Add(ou, alias);
        //            RemovePairFromAliasToStringMap(ou);
        //            _ouAliasToStringMap.Add(alias, ou);
        //        }
        //    }
        //    else
        //    {
        //        AddOUString(alias, ou);
        //        return;
        //    }
        //    aliasOUString = "";
        //    CreateUserOUSettingString();
        //}

        //private static void RemovePairFromAliasToStringMap(string ouToRemove)
        //{
        //    foreach (KeyValuePair<string, string> aliasOUPair in _ouAliasToStringMap)
        //    {
        //        if (aliasOUPair.Value == ouToRemove)
        //        {
        //            _ouAliasToStringMap.Remove(aliasOUPair.Key);
        //            return;
        //        }
        //    }
        //}

        //private static void RemovePairFromStringToAliasMap(string aliasToRemove)
        //{
        //    foreach (KeyValuePair<string, string> ouAliasPair in _ouStringToAliasMap)
        //    {
        //        if (ouAliasPair.Value == aliasToRemove)
        //        {
        //            _ouStringToAliasMap.Remove(ouAliasPair.Key);
        //            return;
        //        }
        //    }
        //}

        //public static void RemoveUserOUString(string ouString)
        //{
        //    if (_ouStringToAliasMap.ContainsKey(ouString))
        //    {
        //        _ouAliasToStringMap.Remove(_ouStringToAliasMap[ouString]);
        //        _ouStringToAliasMap.Remove(ouString);
        //    }
        //    aliasOUString = "";
        //    CreateUserOUSettingString();
        //}

        //private static void CreateUserOUSettingString()
        //{
        //    char separator = ';';
        //    int numberOUStrings = _ouAliasToStringMap.Count;
        //    if (numberOUStrings > 0)
        //    {
        //        StringBuilder ouSettingString = new StringBuilder();
        //        foreach (KeyValuePair<string, string> ouStringPair in _ouAliasToStringMap)
        //        {
        //            string alias = ouStringPair.Key;
        //            string ouString = ouStringPair.Value;
        //            if (_ouAliasToStringMap.Count > 1)
        //            {
        //                ouSettingString.Append(string.Format("{0}_{1}{2}", alias, ouString, separator));
        //            }
        //            else
        //            {
        //                ouSettingString.Append(string.Format("{0}_{1}", alias, ouString));
        //            }
        //        }
        //        aliasOUString = ouSettingString.ToString();
        //    }
        //}

        private static string GetDomainFQDN()
        {
            try
            {
                string _domainFQDN = Domain.GetComputerDomain().Name;
                return _domainFQDN;
            }
            catch (COMException e)
            {
                string msg = e.Message;
                return null;
            }
        }

        private static string GetDomainSimpleName()
        {
            if (_domainFQDN != null)
            {
                string simpleName = GetDomainNetBIOS(_domainFQDN);
                return simpleName;
            }
            else
            {
                return null;
            }
        }

        public static string GetDomainNetBIOS(string fqdn)
        {
            string netbiosDomainName = string.Empty;

            DirectoryEntry rootDSE = new DirectoryEntry(string.Format("LDAP://{0}/RootDSE", fqdn));

            try
            {
                string configurationNamingContext = rootDSE.Properties["configurationNamingContext"][0].ToString();

                DirectoryEntry searchRoot = new DirectoryEntry("LDAP://cn=Partitions," + configurationNamingContext);

                DirectorySearcher searcher = new DirectorySearcher(searchRoot);
                searcher.SearchScope = SearchScope.OneLevel;
                searcher.PropertiesToLoad.Add("netbiosname");
                searcher.Filter = string.Format("(&(objectcategory=Crossref)(dnsRoot={0})(netBIOSName=*))", fqdn);

                SearchResult result = searcher.FindOne();

                if (result != null)
                {
                    netbiosDomainName = result.Properties["netbiosname"][0].ToString();
                }
            }
            catch (COMException e)
            {
                var type = e.GetType();
                var baseException = e.GetBaseException();
                string q = "";
            }
            return netbiosDomainName;
        }

        public static string GetLDAPPath(string fqdn)
        {
            string beginURI = "LDAP://";
            bool firstDC = true;
            StringBuilder sb = new StringBuilder(fqdn.Length);
            string[] domainComponents = fqdn.Split('.');
            sb.Append(beginURI);
            foreach (string dc in domainComponents)
            {
                if (firstDC)
                {
                    sb.Append("DC=");
                    firstDC = false;
                }
                else
                    sb.Append(",DC=");
                sb.Append(dc);
            }
            return sb.ToString();
        }

        //public static string GetUserOUString(string abbreviation)
        //{
        //    if (_ouAliasToStringMap.ContainsKey(abbreviation))
        //    {
        //        return _ouAliasToStringMap[abbreviation];
        //    }
        //    return string.Empty;
        //}

        //public static string GetUserOUAlias(string ouString)
        //{
        //    if (_ouStringToAliasMap.ContainsKey(ouString))
        //    {
        //        return _ouStringToAliasMap[ouString];
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

        public static void SaveAllSettings()
        {
            ADContextPreferenceWriter.SavePreferences();
        }
    }
}
