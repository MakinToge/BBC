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
using System.Data.SQLite;
using System.IO;

namespace BBCv2
{
    public partial class Form1 : Form
    {

        private Capture cap;
        private CascadeClassifier cascadeClassifier;

        public Form1()
        {
            InitializeComponent();

            cap = new Capture();

            timer1.Tick += timer1_Tick;


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Attach the MessageCallbacks on this class & update the PackageDescriptor
            //PackageHost.AttachMessageCallbacks(this);
            //PackageHost.DeclarePackageDescriptor();

            this.Text = string.Format("IsRunning: {0} - IsConnected: {1} - IsStandAlone: {2}", PackageHost.IsRunning, PackageHost.IsConnected, PackageHost.IsStandAlone);
            //PackageHost.WriteInfo("I'm running !");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            imageBox1.Image = cap.QueryFrame();

            cascadeClassifier = new CascadeClassifier(Application.StartupPath + "/haarcascade_frontalface_default.xml");
            using (var imageFrame = cap.QueryFrame().ToImage<Bgr, Byte>())
            {
                if (imageFrame != null)
                {
                    var grayframe = imageFrame.Convert<Gray, byte>();
                    var faces = cascadeClassifier.DetectMultiScale(grayframe, 1.2, 10, Size.Empty); //the actual face detection happens here

                    PackageHost.PushStateObject<Rectangle[]>("faces", faces);

                    foreach (var face in faces)
                    {


                        imageFrame.Draw(face, new Bgr(Color.BurlyWood), 3); //the detected face(s) is highlighted here using a box that is drawn around it/them

                    }
                    if (faces.GetLength(0) > 0)
                    {
                        imageFrame.Draw(faces[0], new Bgr(Color.Red), 3);
                    }
                }
                imageBox1.Image = imageFrame;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //var faceToSave = new Image<Gray, Byte>(imageBox1.Image.Bitmap);
            var faceToSave = cap.QueryFrame();
            Byte[] file;
            IDBAccess dataStore = new DBAccess("facesDB.db");

            var frmSaveDialog = new FrmSaveDialog();
            if (frmSaveDialog.ShowDialog() == DialogResult.OK)
            {
                if (frmSaveDialog._identificationNumber.Trim() != String.Empty)
                {
                    var username = frmSaveDialog._identificationNumber.Trim().ToLower();
                    var filePath = Application.StartupPath + String.Format("/{0}.bmp", username);
                    faceToSave.Save(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            file = reader.ReadBytes((int)stream.Length);
                        }
                    }
                    var result = dataStore.SaveFace(username, file);
                    MessageBox.Show(result, "Save Result", MessageBoxButtons.OK);
                }

            }
        }
    }
}
