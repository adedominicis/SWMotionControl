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

        #region Public Properties and events.

        public event EventHandler controllerUpdatedEvent;
        public event EventHandler<bool> controllerPlugStateChanged;

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

        public bool initXboxControllerComm()
        {
            controller = new Controller(UserIndex.One);
            if (controller.IsConnected)
            {
                // Notificar a la aplicacion que el control está conectado.
                controllerPlugStateChanged?.Invoke(this, true);
                //Crear un timer y ajustar su paso.
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(tickDelta);
                //Suscribirse al evento "tick" del timer. Ocurre cada vez que se cumple un paso.
                timer.Tick += timer_Tick;
                //Arrancar el timer
                timer.Start();
                return true;
            }
            else
            {
                // Notificar a la aplicacion que el control está desconectado
                controllerPlugStateChanged?.Invoke(this, false);
                return false;
            }

        }

        internal void endXboxControllerComm()
        {
            if (controller.IsConnected)
            {
                //Desconectar control
                timer.Stop();
                //Desuscribirse del timer.
                timer.Tick -= timer_Tick;
                //Destruir objeto timer.
                timer = null;
                //Destruir objeto controller.
                controller = null;
                // Notificar a la aplicación que el control está desconectado
                controllerPlugStateChanged?.Invoke(this, false);
            }


        }

        private void timer_Tick(object sender, EventArgs e)
        {
            updateAnalogs();
        }

        private void updateAnalogs()
        {
            try
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

                //Ajustar esto en funcion de las aceleraciones. Por ahora entra directamente.
                if (true)
                {
                    controllerUpdatedEvent?.Invoke(this, null);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message+" Señal del control se ha perdido o desconectado");
                controllerPlugStateChanged?.Invoke(this,false);
                endXboxControllerComm();
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
