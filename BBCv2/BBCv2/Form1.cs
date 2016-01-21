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
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.UI;
using Emgu.CV.Structure;

namespace BBCv2
{
    public partial class Form1 : Form
    {

        private Capture cap;
        private CascadeClassifier cascadeClassifier;

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

            cap = new Capture();

            imageBox1.Image = cap.QueryFrame();

            cascadeClassifier = new CascadeClassifier(Application.StartupPath + "/haarcascade_frontalface_default.xml");
            using (var imageFrame = cap.QueryFrame().ToImage<Bgr, Byte>())
            {
                if (imageFrame != null)
                {
                    var grayframe = imageFrame.Convert<Gray, byte>();
                    var faces = cascadeClassifier.DetectMultiScale(grayframe, 1.1, 10, Size.Empty); //the actual face detection happens here
                    foreach (var face in faces)
                    {
                        imageFrame.Draw(face, new Bgr(Color.BurlyWood), 3); //the detected face(s) is highlighted here using a box that is drawn around it/them

                    }
                }
                imageBox1.Image = imageFrame;
            }

        }
    }
}
