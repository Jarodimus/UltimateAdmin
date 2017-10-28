using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UltimateAdmin.Core.ActiveDirectory;

namespace UltimateAdmin
{
    public partial class ViewUserSearchResults : Form
    {
        private List<User> UserResultsList;

        public ViewUserSearchResults()
        {
            InitializeComponent();
        }
    }
}
