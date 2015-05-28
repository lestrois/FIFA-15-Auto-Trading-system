using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using FUT15_Center;
using FUT15_Center.Models;
using FUT15_JOB;
using FUT15_JOB.Models;
using FUT_MonitorCenter.Modle;

namespace FUT_MonitorCenter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Common.fut15_config = new FUT15Configs();
            Common.fut15_config.Center = new FUT15_Center.FUT15_Center();
            Common.fut15_config.FUT_Client_Facade = new FUTClientFacade(Common.fut15_config);

            Application.Run(new MDIParent1());
        }
    }
}
