using Constellation;
using Constellation.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BBC
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
            int a = 5;
            int b = 6;
            int c = a + b;
        }

        public override void OnShutdown()
        {
            Application.Exit();
        }
    }
}
