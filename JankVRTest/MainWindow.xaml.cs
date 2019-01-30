//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace JankVRTest
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using JankVRTest.KinectHandle;
    using JankVRTest.VridgeHandle;
    using Microsoft.Kinect;
    using VRE.Vridge.API.Client.Messages.BasicTypes;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public static KinectSensor sensor;
        
        private Kinect kinect = new Kinect();
        public static SkeletonView skeletonView = new SkeletonView();
        private static VridgeConnection vridgeConnection = new VridgeConnection();

        internal static VridgeConnection VridgeConnection { get => vridgeConnection; set => vridgeConnection = value; }


        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Execute startup tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            vridgeConnection.controllers[0] = new Controller(0, HandType.Left);
            vridgeConnection.controllers[1] = new Controller(2, HandType.Right);
            vridgeConnection.headset = new Headset();
            kinect.Connect();
            skeletonView.SetupSkeletonView();

            Image.Source = skeletonView.colorBitmap;
            Image2.Source = skeletonView.imageSource;
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            kinect.Disconnect();
        }

 

        

        

        
        private void CheckBoxSeatedModeChanged(object sender, RoutedEventArgs e)
        {
            if (null != sensor)
            {
                if (this.checkBoxSeatedMode.IsChecked.GetValueOrDefault())
                {
                    sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
                }
                else
                {
                    sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sensor.ElevationAngle = (int)sensorAngle.Value;
            }
            catch (InvalidOperationException)
            {

            }
            
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            VridgeConnection.GetStatus();
        }
    }
}