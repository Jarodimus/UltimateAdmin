using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateAdmin.Core
{
    public class AppSearchPreferences
    {
        private WorkstationSearchPreferences _workstationPreferences;
        public WorkstationSearchPreferences WorkstationPreferences
        {
            get
            {
                return _workstationPreferences;
            }
        }

        public AppSearchPreferences()
        {
            _workstationPreferences = new WorkstationSearchPreferences();
            LoadPreferences();
        }

        public void LoadPreferences()
        {
            XmlPreferenceReader xmlReader = new XmlPreferenceReader(this);
            Dictionary<string,string> workstationPrefsMap = xmlReader.GetWorkstationPreferencesFromFile();
            _workstationPreferences.LoadSearchPreferences(workstationPrefsMap);
        }

        public void SavePreferences()
        {
            XmlPreferenceWriter xmlWriter = new XmlPreferenceWriter(this);
            xmlWriter.SavePreferences();
        }
    }
}
