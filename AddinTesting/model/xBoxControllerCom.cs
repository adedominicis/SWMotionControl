using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using SharpDX.XInput;

namespace AddinTesting.model
{
    public class xBoxControllerCom
    {
        #region private Fields
        private DispatcherTimer timer;
        private Controller controller;

        // Thumbstick percentage
        double ltsXPercent;
        double ltsYPercent;
        double rtsXPercent;
        double rtsYPercent;

        //Trigger percentage
        double rtrgPercent;
        double ltrgPercent;


        // Thumbstick + limits. - Limits are symmetrical
        private const int tsRightLimit = 32767;
        private const int tsUpperLimit = 32767;
        private const int triggerLimit = 255;


        #endregion

        #region Properties

        public string RtrgPercent
        {
            get
            {
                return rtrgPercent.ToString();
            }

        }

        #endregion

        #region Metodos

        public void initXboxControllerComm()
        {
            controller = new Controller(UserIndex.One);
            if (controller.IsConnected)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(20);
                timer.Tick += timer_Tick;
                timer.Start();
            }
            else
            {
                MessageBox.Show("Conecta tu Gamepad!");
            }

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateControllerState();
        }

        private void UpdateControllerState()
        {
            var state = controller.GetState();

            // Thumbstick percentage
            ltsXPercent = Math.Round(100 - ((double)(tsRightLimit - state.Gamepad.LeftThumbX) / tsRightLimit) * 100);
            ltsYPercent = Math.Round(100 - ((double)(tsUpperLimit - state.Gamepad.LeftThumbY) / tsUpperLimit) * 100);
            rtsXPercent = Math.Round(100 - ((double)(tsRightLimit - state.Gamepad.RightThumbX) / tsRightLimit) * 100);
            rtsYPercent = Math.Round(100 - ((double)(tsUpperLimit - state.Gamepad.RightThumbY) / tsUpperLimit) * 100);

            // Trigger percentage

            rtrgPercent = Math.Round(100 - ((double)(triggerLimit - state.Gamepad.RightTrigger) / triggerLimit) * 100);
            ltrgPercent = Math.Round(100 - ((double)(triggerLimit - state.Gamepad.LeftTrigger) / triggerLimit) * 100);

        }

        #endregion

    }
}
