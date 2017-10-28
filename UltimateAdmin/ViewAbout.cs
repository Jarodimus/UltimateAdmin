using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UltimateAdmin
{
    public partial class ViewAbout : Form
    {
        public ViewAbout(string appVersion)
        {
            InitializeComponent();
            version.Text = appVersion;
        }

        private void OnCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnLinkClick(object sender, EventArgs e)
        {
            Process launchWakeup = new Process();
            launchWakeup.StartInfo.UseShellExecute = true;
            launchWakeup.StartInfo.FileName = "https://icons8.com/";
            launchWakeup.Start();
        }
    }
}
