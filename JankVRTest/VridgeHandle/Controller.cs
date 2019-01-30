using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VRE.Vridge.API.Client.Messages.BasicTypes;
using VRE.Vridge.API.Client.Messages.v3.Controller;

namespace JankVRTest.VridgeHandle
{
    class Controller
    {
        public int controllerId;

        // Pose data
        public HeadRelation headRelation = HeadRelation.Unrelated;
        public HandType suggestedHand;

        public double posX = 0;
        public double posY = 0;
        public double posZ = 0;
        public double yaw = 0;
        public double pitch = 0;
        public double roll = 0;

        // Touchpad state [-1,1]
        public double analogX = 0;
        public double analogY = 0;

        // Trigger state
        public double analogTrigger = 0;

        // Button states
        public bool isMenuPressed = false;
        public bool isSystemPressed = false;
        public bool isTriggerPressed = false;
        public bool isGripPressed = false;
        public bool isTouchpadPressed = false;
        public bool isTouchpadTouched = false;

        public Controller()
        {

        }

        public Controller(int id, HandType handType)
        {
            controllerId = id;
            suggestedHand = handType;
        }
    }
}
