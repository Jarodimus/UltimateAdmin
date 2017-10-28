using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateAdmin.Core.Logging
{
    public static class Logger
    {
        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;
        private static readonly string Path = AppDomain.CurrentDomain.BaseDirectory;
        private static string LogName
        {
            get
            {
                string CurrentDate = DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString();
                return @"Log_" + CurrentDate;
            }
        }

        public static void Log(string msg, string source)
        {
            using (var writer = new StreamWriter(Path + "\\" + LogName + ".txt", append: true))
            {
                writer.WriteLine(DateTime.Now.ToShortTimeString());
                writer.WriteLine("==============");
                writer.WriteLine(msg + "(" + source + ")" + writer.NewLine);
            }
        }

        public static void LogToEventViewer(string msg)
        {

        }

    }
}
