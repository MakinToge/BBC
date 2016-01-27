using Constellation;
using Constellation.Host;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using Emgu.CV.Face;
using Emgu.CV.CvEnum;

namespace BBCpkg
{
    public class Program : PackageBase
    {

        private Capture cap;
        private CascadeClassifier cascadeClassifier;
        private EigenFaceRecognizer faceRecognizer;

        static void Main(string[] args)
        {
            PackageHost.Start<Program>(args);
        }

        public int RecognizeUser(Image<Gray, byte> userImage)
        {
            faceRecognizer.Load("recognizerFilePath");
            var result = faceRecognizer.Predict(userImage.Resize(100, 100, Inter.Cubic));
            return result.Label;
        }

        public override void OnStart()
        {
            PackageHost.WriteInfo("Package starting - IsRunning: {0} - IsConnected: {1}", PackageHost.IsRunning, PackageHost.IsConnected);

            if (PackageHost.HasControlManager)
            {
                // Change the package manifest (PackageInfo.xml) to enable the control hub access

                // Register the StateObjectLinks on this class
                PackageHost.ControlManager.RegisterStateObjectLinks(this);

                PackageHost.WriteInfo("ControlHub access granted");
            }

            Task.Factory.StartNew(() =>
            {
                while (PackageHost.IsRunning)
                {
                    Rectangle[] faces;
		            cascadeClassifier = new CascadeClassifier( System.Reflection.Assembly.GetExecutingAssembly().CodeBase + "/haarcascade_frontalface_default.xml");
		            using (var imageFrame = cap.QueryFrame().ToImage<Bgr, Byte>())
		            {
			            if (imageFrame != null)
			            {
				            var grayframe = imageFrame.Convert<Gray, byte>();
				            faces = cascadeClassifier.DetectMultiScale(grayframe, 1.2, 10, Size.Empty); //the actual face detection happens here

				            PackageHost.PushStateObject<Rectangle[]>("faces", faces);

				            foreach (var face in faces)
				            {
                                PackageHost.WriteInfo("face recognized", face);
                                PackageHost.PushStateObject<String>("Face", face.ToString());
				            }
			            }
		            }

                    faces = new Rectangle[0];
		    	    Thread.Sleep(PackageHost.GetSettingValue<int>("RefreshRate"));
				}
			});
        }
    }
}
