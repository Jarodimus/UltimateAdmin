using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UltimateAdmin.Core.ActiveDirectory;

namespace UltimateAdmin.Core
{
    public static class ADContextPreferenceWriter
    {
        private static string appPath = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string fileName = "ad_preferences.xml";

        public static bool DoesFileExist()
        {
            if (File.Exists(appPath + fileName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void WriteWorkstationPrefs(XmlWriter writer)
        {
            writer.WriteStartElement("WorkstationGroups");
            var groups = ADContext.GetWorkstationGroups();
            for (int i = 0; i < groups.Count; i++)
            {
                writer.WriteStartElement(groups[i]);
                var groupOUs = ADContext.GetWorkstationGroupOUs(groups[i]);
                if(groupOUs != null)
                {
                    for (int j = 0; j < groupOUs.Count; j++)
                    {
                        writer.WriteStartElement("OU" + (j + 1));
                        writer.WriteString(groupOUs[j]);
                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        private static void WriteUserPrefs(XmlWriter writer)
        {
            writer.WriteStartElement("UserGroups");
            var userAliasToOUs = ADContext.UserAliasToOUMap;
            foreach(KeyValuePair<string,string> aliasToOU in userAliasToOUs)
            {
                writer.WriteStartElement(aliasToOU.Key);
                writer.WriteString(aliasToOU.Value);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        public static void SavePreferences()
        {
            using (var file = File.Create(appPath + fileName))
            using (var writer = XmlWriter.Create(file))
            {
                writer.WriteStartElement("ADContext");
                WriteWorkstationPrefs(writer);
                WriteUserPrefs(writer);
                writer.Close();
            }
        }
    }
}