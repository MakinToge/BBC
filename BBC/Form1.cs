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
using AForge.Video;
using AForge.Video.DirectShow;

namespace BBC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private FilterInfoCollection webcam;
        private VideoCaptureDevice cam;

        private void Form1_Load(object sender, EventArgs e)
        {
            // Attach the MessageCallbacks on this class & update the PackageDescriptor
            PackageHost.AttachMessageCallbacks(this);
            PackageHost.DeclarePackageDescriptor();

            this.Text = string.Format("IsRunning: {0} - IsConnected: {1} - IsStandAlone: {2}", PackageHost.IsRunning, PackageHost.IsConnected, PackageHost.IsStandAlone);
            PackageHost.WriteInfo("I'm running !");

            webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            
            cam = new VideoCaptureDevice(webcam[0].MonikerString);
            cam.NewFrame += cam_NewFrame;
            cam.Start();
        }

        void cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap pic = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = pic;
        }
    }
}
