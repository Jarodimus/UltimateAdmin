using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UltimateAdmin.Core.ActiveDirectory
{
    public static class ADContextPreferenceLoader
    {
        public static string AppPath = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string fileName = "ad_preferences.xml";

        public static string FileName
        {
            get
            {
                return fileName;
            }
        }

        public static bool DoesFileExist()
        {
            if (File.Exists(AppPath + fileName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void LoadWorkstationPrefs(XmlNode workstationNode)
        {
            if(workstationNode != null)
            {
                XmlNodeList groups = workstationNode.ChildNodes;
                foreach (XmlNode groupNode in groups)
                {
                    ADContext.AddWorkstationGroup(groupNode.Name);
                    foreach (XmlNode ouNode in groupNode.ChildNodes)
                    {
                        ADContext.AddWorkstationGroupOU(groupNode.Name, ouNode.InnerText);
                    }
                }
            }
        }

        //private static void LoadUserPrefs(XmlNode userNode)
        //{
        //    if (userNode != null)
        //    {
        //        XmlNodeList aliases = userNode.ChildNodes;
        //        foreach(XmlNode alias in aliases)
        //        {
        //            ADContext.AddOUString(alias.Name, alias.InnerText);
        //        }
        //    }
        //}

        public static string LoadPreferencesFromFile(string filePath)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode root = xmlDoc.SelectSingleNode("ADContext");
                XmlNode workstations = root["WorkstationGroups"];
                XmlNode users = root["UserGroups"];

                LoadWorkstationPrefs(workstations);

                ADContext.SaveAllSettings();
                return "success";
            }
            catch (XmlException e)
            {
                return "fail: " + e.Message;
            }
        }

        public static void LoadPreferences()
        {
            if (DoesFileExist())
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(AppPath + FileName);

                    XmlNode root = xmlDoc.SelectSingleNode("ADContext");
                    XmlNode workstations = root["WorkstationGroups"];
                    XmlNode users = root["UserGroups"];

                    LoadWorkstationPrefs(workstations);
        
                }
                catch (XmlException e)
                {
                    if(File.Exists(AppPath + "ad_preferences_old.xml"))
                    {
                        File.Delete(AppPath + "ad_preferences_old.xml");
                    }
                    File.Move(AppPath + FileName, AppPath + "ad_preferences_old.xml");
                    return;
                }
            }
        }
    }
}
