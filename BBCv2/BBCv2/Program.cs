using Constellation;
using Constellation.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BBCv2
{
    class Program : PackageBase
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args = null)
        {
            PackageHost.Start<Program>(args);
        }

        public override void OnStart()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public override void OnShutdown()
        {
            Application.Exit();
        }
    }
}
