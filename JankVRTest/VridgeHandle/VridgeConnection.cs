using System;
using System.Numerics;
using System.Windows;
using VRE.Vridge.API.Client.Helpers;
using VRE.Vridge.API.Client.Messages.v3.Broadcast;
using VRE.Vridge.API.Client.Remotes;

namespace JankVRTest.VridgeHandle
{
    class VridgeConnection
    {

        private readonly VridgeRemote vridge;

        
        private DateTime? lastHapticPulseTime = null;
        private uint lastHapticPulseLengthUs = 0;

        public Controller[] controllers = new Controller[2];
        public Headset headset;




        public VridgeConnection()
        {
            vridge = new VridgeRemote(
               "localhost",
               "Desktop-Tester",
               Capabilities.Controllers | Capabilities.HeadTracking);

            

            vridge.HapticPulse += OnHapticFeedbackReceived;
        }



        private void CheckStatus()
        {
            var currentStatus = vridge.Status;
            MessageBox.Show(string.Join("\n", currentStatus.Endpoints));
        }

        private void ResetAsyncRotation()
        {
            vridge.Head?.SetAsyncOffset(0);
        }

        private void RecenterView()
        {
            vridge.Head?.Recenter();
        }

        private void OnHapticFeedbackReceived(object sender, HapticPulse hapticPulse)
        {
            lastHapticPulseLengthUs = hapticPulse.LengthUs;
            lastHapticPulseTime = DateTime.Now;
        }

        private void UpdateControllerState(Controller controller)
        {
            
            vridge.Controller?.SetControllerState(controller.controllerId,
            controller.headRelation,
            controller.suggestedHand,
            Quaternion.CreateFromYawPitchRoll(
                (float)MathHelpers.DegToRad(controller.yaw),
                (float)MathHelpers.DegToRad(controller.pitch),
                (float)MathHelpers.DegToRad(controller.roll)),
            new Vector3((float)controller.posX, (float)controller.posY, (float)controller.posZ),
            controller.analogX, controller.analogY, controller.analogTrigger,
            controller.isMenuPressed, controller.isSystemPressed, 
            controller.isTriggerPressed, controller.isGripPressed, 
            controller.isTouchpadPressed, controller.isTouchpadTouched);
        }

        private void UpdateHeadsetState(Headset headset)
        {
            vridge.Head?.SetPosition(headset.posX, headset.posY, headset.posZ);
        }

        public void UpdateVRPositions()
        {
            UpdateHeadsetState(headset);
            foreach (Controller controller in controllers)
            {
                UpdateControllerState(controller);
            }
            
        }

        public void GetStatus()
        {
            var currentStatus = vridge.Status;
            MessageBox.Show(string.Join("\n", currentStatus.Endpoints));
        }

    }
}
