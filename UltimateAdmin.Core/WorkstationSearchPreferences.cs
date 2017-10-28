using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UltimateAdmin.Core.ActiveDirectory;

namespace UltimateAdmin.Core
{
    public class WorkstationSearchPreferences : ISearchPreferences
    {
        private bool searchEntireDirectory;
        public bool SearchEntireDirectory
        {
            get
            {
                return searchEntireDirectory;
            }
        }

        private List<string> selectedGroups;

        public List<string> SelectedGroups
        {
            get
            {
                return selectedGroups;
            }
        }

        public void LoadSearchPreferences(Dictionary<string, string> savedPreferences)
        {
            if (savedPreferences != null)
            {
                bool parseSuccessful = bool.TryParse(savedPreferences["SearchEntireDirectory"], out searchEntireDirectory);
                if (!parseSuccessful)
                {
                    searchEntireDirectory = true;
                }
                string groups = savedPreferences["Groups"];
                if (string.IsNullOrEmpty(groups))
                {
                    return;
                }
                else
                {
                    ParseGroupsString(groups);
                }
            }
            else
            {
                searchEntireDirectory = true;
            }
        }

        private void LoadPreferences()
        {
            XmlPreferenceReader xmlReader = new XmlPreferenceReader(this);
            Dictionary<string,string> preferences = xmlReader.GetPreferences();
            if(preferences != null)
            {
                bool parseSuccessful = bool.TryParse(preferences["SearchEntireDirectory"], out searchEntireDirectory);
                if (!parseSuccessful)
                {
                    searchEntireDirectory = true;
                }
                string groups = preferences["Groups"];
                if (string.IsNullOrEmpty(groups))
                {
                    return;
                }
                else
                {
                    ParseGroupsString(groups);
                }
            } else
            {
                searchEntireDirectory = true;
            }
        }

        public void SetSearchEntireDirectory(bool searchAll)
        {
            searchEntireDirectory = searchAll;
        }

        public void SetSearchGroups(List<string> searchGroups)
        {
            selectedGroups = searchGroups;
        }

        private void ParseGroupsString(string groupsString)
        {
            selectedGroups = new List<string>();
            if (groupsString.Length > 0)
            {
                if (groupsString.Contains(";"))
                {
                    string[] rawGroupStrings = groupsString.Split(';');
                    for (int i = 0; i < rawGroupStrings.Length; i++)
                    {
                        if (string.IsNullOrEmpty(rawGroupStrings[i]))
                        {
                            continue;
                        }
                        selectedGroups.Add(rawGroupStrings[i]);
                    }
                }
                else
                {
                    selectedGroups.Add(groupsString);
                }
            }
        }

        public void SaveAllSettings()
        {
            XmlPreferenceWriter xmlPrefWriter = new XmlPreferenceWriter(this);
            xmlPrefWriter.SavePreferences();
        }
    }
}
