using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UltimateAdmin.Presentation;

namespace UltimateAdmin
{
    public class UAApplicationContext : ApplicationContext
    {
        private int formCount;
        private ViewLogon ViewLogon;
        private ViewMain ViewMain;

        public UAApplicationContext()
        {
            formCount = 0;
            ViewLogon = new ViewLogon();
            var appController = new ApplicationController();
            var logonFormPresenter = new ViewLogonPresenter(ViewLogon, appController);
            ViewLogon.Closed += new EventHandler(OnFormClosed);
            formCount++;

            ViewMain = new ViewMain();
            var viewMainPresenter = new ViewMainPresenter(ViewMain, appController);
            ViewMain.Closed += new EventHandler(OnFormClosed);
            
            logonFormPresenter.Run();
        }

        private void OnFormClosed(object sender, EventArgs e)
        {
            formCount--;
            if(formCount == 0)
            {
                this.ExitThread();
            }
        }
    }
}
