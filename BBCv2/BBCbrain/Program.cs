using Constellation;
using Constellation.Control;
using Constellation.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBCbrain
{
    public class Program : PackageBase
    {

        [StateObjectLink("BBCpkg", "Face")]
        public StateObjectNotifier Face { get; set; }

        static void Main(string[] args)
        {
            PackageHost.Start<Program>(args);
        }

        public override void OnStart()
        {
            PackageHost.WriteInfo("Package starting - IsRunning: {0} - IsConnected: {1}", PackageHost.IsRunning, PackageHost.IsConnected);

            if (!PackageHost.HasControlManager)
            {

                PackageHost.WriteError("unable to connect");
                return;
            }

            PackageHost.ControlManager.RegisterStateObjectLinks(this);

            this.Face.ValueChanged += Face_ValueChanged;

        }

        void Face_ValueChanged(object sender, StateObjectChangedEventArgs e)
        {
            if (((string)e.NewState.DynamicValue) == "Unknown")
            {
                PackageHost.WriteWarn("Warning unknown face detected");

                PackageHost.CreateScope("PushBullet").Proxy.SendPush(
                    new
                    {
                        Title = "Warning",
                        Message = "BBC report an unknown face "
                    }
                    );
            }
            else
            {
                PackageHost.WriteInfo("{0} detected", ((string)e.NewState.DynamicValue));

                PackageHost.CreateScope("PushBullet").Proxy.SendPush(
                    new
                    {
                        Title = "BBC report",
                        Message = ((string)e.NewState.DynamicValue) + " detected"
                    }
                    );
            }
        }
        
    }
}
