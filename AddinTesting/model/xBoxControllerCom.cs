using SharpDX.XInput;
using System;
using System.Windows;
using System.Windows.Threading;

namespace AddinTesting.model
{
    public class xBoxControllerCom
    {
        /// <summary>
        /// Expone posiciones y velocidades analogas del control XBOX ONE. Los datos salen en valores decimal de -100 a 100.
        /// Contiene evento "controllerUpdateEvent" para notificar que alguno de los botones o analogos ha sido accionado
        /// Contiene evento "controllerPlugStateChanged" para notificar que el control fue desconectado o conectado.
        /// </summary>

        #region private Fields
        private DispatcherTimer timer;
        private Controller controller;

        // Posiciones analogos, paso N
        private decimal leftThumbX;
        private decimal leftThumbY;
        private decimal rightThumbX;
        private decimal rightThumbY;
        private decimal rightTrigger;
        private decimal leftTrigger;

        //Posición de los analogos en el paso N-1
        private decimal oldLeftThumbX;
        private decimal oldLeftThumbY;
        private decimal oldRightThumbX;
        private decimal oldRightThumbY;
        private decimal oldLeftTrigger;
        private decimal oldRightTrigger;

        // Velocidades análogos, paso N
        private decimal spdLeftThumbX;
        private decimal spdLeftThumbY;
        private decimal spdRightThumbX;
        private decimal spdRightThumbY;
        private decimal spdLeftTrigger;
        private decimal spdRightTrigger;


        // Limites de posicionamiento de los analogos.
        private const int tsRightLimit = 32767;
        private const int tsUpperLimit = 32767;
        private const int triggerLimit = 255;
        //General deadzone
        private int deadZone;

        //Paso del reloj, ms
        private int deltaT;

        //Control conectado?
        private bool isPlugged;

        #endregion

        #region Events.

        public event EventHandler controllerUpdatedEvent;
        public event EventHandler<bool> controllerPlugStateChanged;


        #endregion

        #region Properties

        // Porcentaje de deadzone en los análogos.
        public int DeadZone
        {
            set { deadZone = value; }
        }

        //Paso del reloj, ms
        public int DeltaT
        {
            set { deltaT = value; }
        }

        // Control conectado?
        public bool IsPlugged
        {
            get { return isPlugged; }
        }

        // Analog positions
        public decimal LeftThumbX
        {
            get
            {
                return leftThumbX;
            }

        }
        public decimal LeftThumbY
        {
            get
            {
                return leftThumbY;
            }

        }
        public decimal RightThumbX
        {
            get
            {
                return rightThumbX;
            }

        }
        public decimal RightThumbY
        {
            get
            {
                return rightThumbY;
            }

        }
        public decimal RightTrigger
        {
            get
            {
                return rightTrigger;
            }

        }
        public decimal LeftTrigger
        {
            get
            {
                return leftTrigger;
            }

        }

        //Analog speed

        public decimal SpdLeftThumbX
        {
            get { return spdLeftThumbX; }
        }
        public decimal SpdLeftThumbY
        {
            get { return spdLeftThumbY; }
        }
        public decimal SpdRightThumbX
        {
            get { return spdRightThumbX; }
        }
        public decimal SpdRightThumbY
        {
            get { return spdRightThumbY; }
        }
        public decimal SpdLeftTrigger
        {
            get { return spdLeftTrigger; }
        }
        public decimal SpdRightTrigger
        {
            get { return spdRightTrigger; }
        }

        #endregion

        #region Metodos

        public void initXboxControllerComm()
        {
            controllerPlugStateChanged += changeControllerPlugState;
            controller = new Controller(UserIndex.One);
            if (controller.IsConnected)
            {
                // Notificar a la aplicacion que el control está conectado.
                controllerPlugStateChanged?.Invoke(this, true);
                //Crear un timer y ajustar su paso.
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(deltaT);
                //Suscribirse al evento "tick" del timer. Ocurre cada vez que se cumple un paso.
                timer.Tick += timer_Tick;
                //Arrancar el timer
                timer.Start();
            }
            else
            {
                // Notificar a la aplicacion que el control está desconectado
                endXboxControllerComm();
                controllerPlugStateChanged?.Invoke(this, false);
            }
        }

        //Handler de cambio de estado del control.
        private void changeControllerPlugState(object sender, bool e)
        {
            isPlugged = e;
        }

        //Desconectar control.
        internal void endXboxControllerComm()
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
            //Setear estado de conexión

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

                bool deadZoneTrigger = false;
                //Deadzone correction:
                if (Math.Abs(leftThumbX) < deadZone)
                {
                    leftThumbX = 0;
                }
                if (Math.Abs(leftThumbY) < deadZone)
                {
                    leftThumbY = 0;
                }
                if (Math.Abs(rightThumbX) < deadZone)
                {
                    rightThumbX = 0;
                }
                if (Math.Abs(rightThumbY) < deadZone)
                {
                    rightThumbY = 0;
                }

                //speed
                setAnalogspeed();

                //Si los movimientos son mayores al deadzone, emitir movimiento.
                if (!deadZoneTrigger)
                {
                    controllerUpdatedEvent?.Invoke(this, null);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " Señal del control se ha perdido o desconectado");
                controllerPlugStateChanged?.Invoke(this, false);
                endXboxControllerComm();
            }



        }

        private void setAnalogspeed()
        {
            spdLeftThumbX = (decimal)((oldLeftThumbX - leftThumbX) / deltaT);
            spdLeftThumbY = (decimal)((oldLeftThumbY - leftThumbY) / deltaT);
            spdRightThumbX = (decimal)((oldRightThumbX - rightThumbX) / deltaT);
            spdRightThumbY = (decimal)((oldRightThumbY - rightThumbY) / deltaT);
            spdLeftTrigger = (decimal)((oldLeftTrigger - leftTrigger) / deltaT);
            spdRightTrigger = (decimal)((oldRightTrigger - rightTrigger) / deltaT);

        }

        private bool analogsMoved()
        {
            //Si algun analog se movió
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
