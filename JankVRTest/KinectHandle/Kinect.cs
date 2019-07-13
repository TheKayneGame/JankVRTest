using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using JankVRTest.VridgeHandle;
using Microsoft.Kinect;
using VRE.Vridge.API.Client.Messages.BasicTypes;

namespace JankVRTest.KinectHandle
{
    public class Kinect
    {
        /// <summary>
        /// Active Kinect sensor
        /// </summary>



        public void Connect()
        {

            // Look through all sensors and start the first connected one.
            // This requires that a Kinect is connected at the time of app startup.
            // To make your app robust against plug/unplug, 
            // it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit (See components in Toolkit Browser

            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {

                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    MainWindow.sensor = potentialSensor;

                    break;
                }
            }
            if (null != MainWindow.sensor)
            {


                // Turn on the skeleton stream to receive skeleton frames
                MainWindow.sensor.SkeletonStream.Enable(new TransformSmoothParameters
                {
                    Smoothing = 0.5f,
                    Correction = 0.1f,
                    Prediction = 0.0f,
                    JitterRadius = 0.05f,
                    MaxDeviationRadius = 0.08f
                });

                // Add an event handler to be called whenever there is new color frame data
                MainWindow.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;

                // Turn on the color stream to receive color frames
                MainWindow.sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

                // Add an event handler to be called whenever there is new color frame data
                MainWindow.sensor.ColorFrameReady += this.SensorColorFrameReady;

                // Start the sensor!
                try
                {
                    MainWindow.sensor.Start();
                }

                catch (IOException)
                {
                    MainWindow.sensor = null;
                }
            }

            if (null == MainWindow.sensor)
            {

            }

        }

        public void Disconnect()
        {
            if (null != MainWindow.sensor)
            {
                MainWindow.sensor.Stop();
            }
        }

        /// <summary>
        /// Event handler for Kinect sensor's ColorFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>

        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            MainWindow.skeletonView.DrawCamera(e);

        }

        /// <summary>
        /// Event handler for Kinect sensor's SkeletonFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            MainWindow.skeletonView.DrawSkeleton(e);
            Skeleton[] skeletons = new Skeleton[0];
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            if (skeletons.Length != 0)
            {
                foreach (Skeleton skel in skeletons)
                {
                    if (skel.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        var controller = MainWindow.VridgeConnection.controllers[0];
                        controller.posX = skel.Joints[JointType.HandLeft].Position.X;
                        controller.posY = skel.Joints[JointType.HandLeft].Position.Y;
                        controller.posZ = skel.Joints[JointType.HandLeft].Position.Z;

                        MainWindow.VridgeConnection.controllers[1].posX = skel.Joints[JointType.HandRight].Position.X;
                        MainWindow.VridgeConnection.controllers[1].posY = skel.Joints[JointType.HandRight].Position.Y;
                        MainWindow.VridgeConnection.controllers[1].posZ = skel.Joints[JointType.HandRight].Position.Z;

                        MainWindow.VridgeConnection.headset.posX = skel.Joints[JointType.Head].Position.X;
                        MainWindow.VridgeConnection.headset.posY = skel.Joints[JointType.Head].Position.Y;
                        MainWindow.VridgeConnection.headset.posZ = skel.Joints[JointType.Head].Position.Z;

                        MainWindow.VridgeConnection.UpdateVRPositions();

                    }
                    else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
                    {



                    }
                }
            }


        }
    }


}
