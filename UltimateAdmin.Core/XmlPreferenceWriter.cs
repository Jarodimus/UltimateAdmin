using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using UltimateAdmin.Core.ActiveDirectory;

namespace UltimateAdmin.Core
{
    public class XmlPreferenceWriter
    {
        private string appPath = AppDomain.CurrentDomain.BaseDirectory;
        private ISearchPreferences _searchPreferences;

        private AppSearchPreferences _appSearchPreferences;

        private string preferencesFile = "preferences.xml";

        public XmlPreferenceWriter(AppSearchPreferences preferences)
        {
            _appSearchPreferences = preferences;
        }


        public XmlPreferenceWriter(ISearchPreferences preferences)
        {
            _searchPreferences = preferences;
        }

        public void SavePreferences()
        {
            using (var file = File.Create(appPath + preferencesFile))
            using (var writer = XmlWriter.Create(file))
            {
                writer.WriteStartElement("SearchPreferences");
                SaveWorkstationSettings(writer);
                writer.WriteFullEndElement();
                writer.Close();
            }
        }

        private void SaveWorkstationSettings(XmlWriter writer)
        {
            WorkstationSearchPreferences workstationSearchPrefs = _appSearchPreferences.WorkstationPreferences;
            string searchEntireDirectory = workstationSearchPrefs.SearchEntireDirectory.ToString();
            List<string> selectedGroups = null;
            if (workstationSearchPrefs.SelectedGroups != null)
            {
                selectedGroups = workstationSearchPrefs.SelectedGroups;
            }
            string searchAllValue = workstationSearchPrefs.SearchEntireDirectory.ToString();
            writer.WriteStartElement("WorkstationPrefs");
            writer.WriteStartElement("SearchEntireDirectory");
            writer.WriteString(searchEntireDirectory);
            writer.WriteEndElement();
            writer.WriteStartElement("Groups");
            if (selectedGroups != null)
            {
                for (int i = 0; i < selectedGroups.Count; i++)
                {
                    writer.WriteStartElement(selectedGroups[i]);
                    writer.WriteEndElement();
                }
            } //else
            //{
            //    writer.WriteFullEndElement();
            //}
            writer.WriteFullEndElement();
            //writer.WriteFullEndElement();
        }
    }
}
