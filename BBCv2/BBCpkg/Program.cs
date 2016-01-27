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
using System.IO;

namespace BBCpkg
{
    public class Program : PackageBase
    {

        private Capture cap;
        private CascadeClassifier cascadeClassifier;
        private RecognizerEngine recoEngine;

        static void Main(string[] args)
        {
            PackageHost.Start<Program>(args);
        }

        public override void OnStart()
        {
            PackageHost.WriteInfo("Package starting - IsRunning: {0} - IsConnected: {1}", PackageHost.IsRunning, PackageHost.IsConnected);


            string startupPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            IDBAccess dataStore = new DBAccess("facesDB.db");
            recoEngine = new RecognizerEngine(
                Path.Combine(Environment.CurrentDirectory, "data/facesDB.db"),
                Path.Combine(Environment.CurrentDirectory, "data/RecognizerEngineData.YAML"));    //"/data/facesDB.db", startupPath + "/data/RecognizerEngineData.YAML");
            cap = new Capture();

            Task.Factory.StartNew(() =>
            {
                while (PackageHost.IsRunning)
                {
                    Rectangle[] faces;
                    //string bla = System.Reflection.Assembly.GetExecutingAssembly().    CodeBase + "/haarcascade_frontalface_default.xml";
		            cascadeClassifier = new CascadeClassifier( Path.Combine(Environment.CurrentDirectory, "haarcascade_frontalface_default.xml"));
		            using (var imageFrame = cap.QueryFrame().ToImage<Bgr, Byte>())
		            {
			            if (imageFrame != null)
			            {
				            var grayframe = imageFrame.Convert<Gray, byte>();
				            faces = cascadeClassifier.DetectMultiScale(grayframe, 1.2, 10, Size.Empty); //the actual face detection happens here

				            PackageHost.PushStateObject<Rectangle[]>("faces", faces);
				            foreach (var face in faces)
				            {

                                int nameID = recoEngine.RecognizeUser(imageFrame.GetSubRect(face).Convert<Gray, byte>());
                                if (nameID == 0)
                                {
                                    PackageHost.WriteWarn("unknown face");
                                    PackageHost.PushStateObject<String>("Face", "Unknown");
                                }
                                else
                                {
                                    string name = dataStore.GetUsername(nameID);
                                    PackageHost.WriteInfo("face recognized : {0}", name);
                                    PackageHost.PushStateObject<String>("Face", name);
                                }
				            }
			            }
		            }
		    	    Thread.Sleep(PackageHost.GetSettingValue<int>("RefreshRate"));
				}
			});
        }
    }
}
