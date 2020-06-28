using SharpDX.XInput;
using System;
using System.Windows;
using System.Windows.Threading;

namespace AddinTesting.model
{
    public class xBoxControllerCom
    {
        #region private Fields
        private DispatcherTimer timer;
        private Controller controller;
        

        // Thumbstick percentage
        private decimal leftThumbX;
        private decimal leftThumbY;
        private decimal rightThumbX;
        private decimal rightThumbY;



        // Thumbstick percentage
        private decimal oldLeftThumbX;
        private decimal accLeftThumbX;
        private decimal accLeftThumbY;
        private decimal accRightThumbX;
        private decimal accRightThumbY;
        private decimal accLeftTrigger;
        private decimal accRightTrigger;
        private decimal oldLeftThumbY;
        private decimal oldRightThumbX;
        private decimal oldRightThumbY;
        private decimal oldLeftTrigger;
        private decimal oldRightTrigger;

        //Trigger percentage
        decimal rightTrigger;
        decimal leftTrigger;


        // Thumbstick + limits. - Limits are symmetrical
        private const int tsRightLimit = 32767;
        private const int tsUpperLimit = 32767;
        private const int triggerLimit = 255;

        //Timing constant
        private const int tickDelta = 20;



        #endregion

        #region public

        public event EventHandler RaiseControllerUpdatedEvent;

        #endregion

        #region Properties

        // Analog percent coordinates.
        public string LeftThumbX
        {
            get
            {
                return leftThumbX.ToString();
            }

        }

        public string LeftThumbY
        {
            get
            {
                return leftThumbY.ToString();
            }

        }

        public string RightThumbX
        {
            get
            {
                return rightThumbX.ToString();
            }

        }

        public string RightThumbY
        {
            get
            {
                return rightThumbY.ToString();
            }

        }

        public string RightTrigger
        {
            get
            {
                return rightTrigger.ToString();
            }

        }

        public string LeftTrigger
        {
            get
            {
                return leftTrigger.ToString();
            }

        }
        
        // Analog accelerations.


        #endregion

        #region Metodos

        public void initXboxControllerComm()
        {
            controller = new Controller(UserIndex.One);
            if (controller.IsConnected)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(tickDelta);
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
            updateAnalogs();
        }


        private void updateAnalogs()
        {
            var state = controller.GetState();

            // Guardar valores antes de la actualización --> reemplazar por aceleracion o alguna medida de la variacion

            saveAnalogPositions();

            // Thumbstick percentage
            leftThumbX = Math.Round(100 - ((decimal)(tsRightLimit - state.Gamepad.LeftThumbX) / tsRightLimit) * 100);
            leftThumbY = Math.Round(100 - ((decimal)(tsUpperLimit - state.Gamepad.LeftThumbY) / tsUpperLimit) * 100);
            rightThumbX = Math.Round(100 - ((decimal)(tsRightLimit - state.Gamepad.RightThumbX) / tsRightLimit) * 100);
            rightThumbY = Math.Round(100 - ((decimal)(tsUpperLimit - state.Gamepad.RightThumbY) / tsUpperLimit) * 100);

            // Trigger percentage

            rightTrigger = Math.Round(100 - ((decimal)(triggerLimit - state.Gamepad.RightTrigger) / triggerLimit) * 100);
            leftTrigger = Math.Round(100 - ((decimal)(triggerLimit - state.Gamepad.LeftTrigger) / triggerLimit) * 100);

            //Accelerations
            setAnalogAccelerations();

            //
            if (true)
            {
                RaiseControllerUpdatedEvent?.Invoke(this, null);
            }

        }

        private void setAnalogAccelerations()
        {
            accLeftThumbX=(decimal)((oldLeftThumbX-leftThumbX)/ tickDelta);
            accLeftThumbY= (decimal)((oldLeftThumbY-leftThumbY)/tickDelta);
            accRightThumbX = (decimal)((oldRightThumbX - rightThumbX) / tickDelta);
            accRightThumbY=(decimal)((oldRightThumbY - rightThumbY) / tickDelta);

            accLeftTrigger = (decimal)((oldLeftTrigger - leftTrigger) / tickDelta);
            accRightTrigger = (decimal)((oldRightTrigger - rightTrigger) / tickDelta);

        }

        private bool analogsMoved()
        {
            //El criterio acá debería ser de aceleracion
            return oldLeftThumbX != leftThumbX ||
                oldLeftThumbY != leftThumbY || 
                oldRightThumbX != rightThumbX ||
                oldRightThumbY != rightThumbY ||
                oldLeftTrigger != leftTrigger ||
                oldRightTrigger != rightTrigger;
        }

        private void saveAnalogPositions()
        {
            oldLeftThumbX = leftThumbX;
            oldLeftThumbY = leftThumbY;
            oldRightThumbX = rightThumbX;
            oldRightThumbY = rightThumbY;
            oldLeftTrigger = leftTrigger;
            oldRightTrigger = rightTrigger;
        }

        #endregion

    }
}
