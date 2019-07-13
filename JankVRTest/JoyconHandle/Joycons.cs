
using Joycon4CS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace JankVRTest.JoyconHandle
{
    class Joycons
    {

        JoyconManager joyconManager = new JoyconManager();
        private void ScanJoycon()
        {
            joyconManager.Scan();

            UpdateDebugType();

            joyconManager.Start();


        }

        private void UpdateDebugType()
        {
            foreach (var j in joyconManager.j)
            {
                j.debug_type = Joycon.DebugType.NONE;
            }
        }

        private void StartJoycon()
        {
            ScanJoycon();
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(16);
            dt.Tick += Dt_Tick;

            dt.Start();
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            joyconManager.Update();

            var j1 = joyconManager.j[0];

            MainWindow.VridgeConnection.controllers[0].yaw = (float)(j1.GetVector().eulerAngles.Z * 180.0f / Math.PI);


        }

        private void Disconnect()
        {
            joyconManager.OnApplicationQuit();
        }
    }
}
