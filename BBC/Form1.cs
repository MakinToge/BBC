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
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace BBC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //private FilterInfoCollection webcam;
        //private VideoCaptureDevice cam;

        private CascadeClassifier cascadeClassifier;

        private Capture cap;
        //private HaarCascade haar;

        private void Form1_Load(object sender, EventArgs e)
        {
            // Attach the MessageCallbacks on this class & update the PackageDescriptor
            PackageHost.AttachMessageCallbacks(this);
            PackageHost.DeclarePackageDescriptor();

            this.Text = string.Format("IsRunning: {0} - IsConnected: {1} - IsStandAlone: {2}", PackageHost.IsRunning, PackageHost.IsConnected, PackageHost.IsStandAlone);
            PackageHost.WriteInfo("I'm running !");




           


            //webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //foreach (FilterInfo VideoCaptureDevice in webcam)
            //{
            //    comboBox1.Items.Add(VideoCaptureDevice.Name);
            //}
            //comboBox1.SelectedIndex = 0;

            //// passing 0 gets zeroth webcam
            //cap = new Capture(0);
            //// adjust path to find your xml
            //haar = new HaarCascade("..\\..\\..\\..\\lib\\haarcascade_frontalface_default.xml");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //cam = new VideoCaptureDevice(webcam[comboBox1.SelectedIndex].MonikerString);
            //cam.NewFrame += cam_NewFrame;
            //cam.Start();
        }

        //void cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        //{
        //    Bitmap pic = (Bitmap)eventArgs.Frame.Clone();
        //    imageBox1.Image = pic;
        //}

        private void button3_Click(object sender, EventArgs e)
        {
            //if (cam.IsRunning)
            //{
            //    cam.Stop();
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
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






            /*using (Image<Bgr, byte> nextFrame = cap.QueryFrame())
            {
                if (nextFrame != null)
                {
                    // there's only one channel (greyscale), hence the zero index
                    //var faces = nextFrame.DetectHaarCascade(haar)[0];
                    Image<Gray, byte> grayframe = nextFrame.Convert<Gray, byte>();
                    var faces =
                            grayframe.DetectHaarCascade(
                                    haar, 1.4, 4,
                                    HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                                    new Size(nextFrame.Width / 8, nextFrame.Height / 8)
                                    )[0];

                    foreach (var face in faces)
                    {
                        nextFrame.Draw(face.rect, new Bgr(0, double.MaxValue, 0), 3);
                    }
                    pictureBox1.Image = nextFrame.ToBitmap();
                }
            }*/
        }

        private void imageBox1_Click(object sender, EventArgs e)
        {

        }

        
    }
}
