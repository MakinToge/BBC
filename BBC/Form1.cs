using Constellation;
using Constellation.Host;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BBC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Attach the MessageCallbacks on this class & update the PackageDescriptor
            PackageHost.AttachMessageCallbacks(this);
            PackageHost.DeclarePackageDescriptor();

            this.Text = string.Format("IsRunning: {0} - IsConnected: {1} - IsStandAlone: {2}", PackageHost.IsRunning, PackageHost.IsConnected, PackageHost.IsStandAlone);
            PackageHost.WriteInfo("I'm running !");
        }
    }
}
