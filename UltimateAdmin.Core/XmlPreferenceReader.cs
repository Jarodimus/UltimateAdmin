using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UltimateAdmin.Core
{
    public class XmlPreferenceReader
    {
        private string appPath = AppDomain.CurrentDomain.BaseDirectory;
        private string preferences = "preferences.xml";
        private ISearchPreferences _searchPreferences;

        private AppSearchPreferences _appPreferences;

        public XmlPreferenceReader(AppSearchPreferences appPreferences)
        {
            _appPreferences = appPreferences;
        }

        public XmlPreferenceReader(ISearchPreferences searchPreferences)
        {
            _searchPreferences = searchPreferences;
        }

        private string PreferenceType
        {
            get
            {
                Type t = _searchPreferences.GetType();
                if (t == typeof(WorkstationSearchPreferences))
                {
                    return "Workstation";
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public bool DoesFileExist()
        {
            if(File.Exists(appPath + preferences))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Dictionary<string,string> GetPreferences()
        {
            if (PreferenceType == "Workstation")
                return GetWorkstationPreferencesFromFile();
            return null;
        }

        public XmlDocument GetPreferencesFile()
        {
            if (DoesFileExist())
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(appPath + preferences);
                return xmlDoc;
            } else
            {
                return null;
            }
        }

        public Dictionary<string, string> GetWorkstationPreferencesFromFile()
        {
            Dictionary<string, string> workstationPrefs = null;
            bool searchEntireDir = false;
            StringBuilder groupString = new StringBuilder();
            if (DoesFileExist())
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(appPath + preferences);
                    XmlNode root = xmlDoc.DocumentElement.SelectSingleNode("WorkstationPrefs");
                    XmlNodeList groupNodes = root["Groups"].ChildNodes;

                    bool.TryParse(root.SelectSingleNode("SearchEntireDirectory").InnerText, out searchEntireDir);

                    if (groupNodes.Count > 0)
                    {
                        foreach (XmlNode node in groupNodes)
                        {
                            groupString.Append(node.Name + ";");
                        }
                    }

                    workstationPrefs = new Dictionary<string, string>();
                    workstationPrefs.Add("SearchEntireDirectory", searchEntireDir.ToString());
                    workstationPrefs.Add("Groups", groupString.ToString());
                }
                catch (XmlException e)
                {

                }
                return workstationPrefs;
            }
            return null;
        }

    }
}
