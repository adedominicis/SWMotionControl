using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Windows;
using AddinTesting.ViewModel;


namespace AddinTesting.model
{
    /// <summary>
    /// Clase app control, tecnicamente deberia concentrar todo el control de la aplicacion acá.
    /// 1- Conexion y desconexión de mandos.
    /// 
    /// </summary>
    /// 


    class AppControl
    {
        #region Public Events

        // A este evento debe suscribirse el viewmodel para obtener su data.
        public event EventHandler<viewModelData> appControlUiRefresh;
        
        #endregion

        #region Private Fields
        private SldWorks swApp;
        private KinematicManipulator kineM;
        private SelectionListener selListener;
        private xBoxControllerCom xBoxConInstance;
        viewModelData vmData;
        #endregion

        #region public properties

        // Exponer selection listener, solo lectura.
        public SelectionListener SelListener
        {
            get { return selListener; }
        }
        // Exponer Controllador de mando Xbox, solo lectura
        public xBoxControllerCom XBoxConInstance
        {
            get { return xBoxConInstance; }
        }
        //Exponer vmData
        public viewModelData VmData
        {
            get { return vmData; }
        }
        #endregion

        #region Constructor
        public AppControl(viewModel vm)
        {
            //instancia actual de solidworks
            swApp = (SldWorks)vm.SwApp;
            //Instancia del viewModelData
            vmData= vm.VmData;
            // Suscribirse a PropertyChanged del viewmodel
            vm.PropertyChanged += Vmodel_PropertyChanged;
            //Inicializar app control
            initAppControl();
        }

        #endregion

        public void initAppControl()
        {
            //Instanciar un listener de selecciones
            selListener = new SelectionListener(swApp);
            //Instanciar el controlador para xbox
            xBoxConInstance = new xBoxControllerCom();
            //Inicializar Mate Manipulator
            kineM = new KinematicManipulator(this);
        }

        #region Connect and disconnect peripherals
        // Conectar mando de xbox
        public bool ConnectController()
        {

            try
            {
                //Instanciar e iniciar controlador del mando
                xBoxConInstance.initXboxControllerComm();
                if (XBoxConInstance.IsPlugged)
                {
                    // Suscribirse al evento del control. Este evento se dispara cada vez que el usuario usa los elementos del mando.
                    xBoxConInstance.controllerUpdatedEvent += controllerUpdatedEventHandler;
                    MessageBox.Show("Mando XBOX ONE conectado!");
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Error al conectar mando XBOX ONE: " + e.Message);
                return false;
            }

        }

        // Desconectar mando de xbox
        public bool DisconnectController()
        {
            try
            {
                if (xBoxConInstance != null)
                {
                    //Desuscribirse del evento controllerupdate
                    xBoxConInstance.controllerUpdatedEvent -= controllerUpdatedEventHandler;
                    //Finalizar conexion con el control
                    xBoxConInstance.endXboxControllerComm();
                    MessageBox.Show("Mando XBOX ONE desconectado!");
                    
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al desconectar mando XBOX ONE: " + e.Message);
            }
            return false;
        }

        // Eventos del modelo actualizan info en el AppController
        private void controllerUpdatedEventHandler(object sender, EventArgs e)
        {
            //Actualizar vmdata para todos los analogos.
            vmData.LeftThumbX = xBoxConInstance.LeftThumbX;
            vmData.LeftThumbY = xBoxConInstance.LeftThumbY;
            vmData.RightThumbX = xBoxConInstance.RightThumbX;
            vmData.RightThumbY = xBoxConInstance.RightThumbY;
            vmData.RightTrigger = xBoxConInstance.RightTrigger;
            vmData.LeftTrigger = xBoxConInstance.LeftTrigger;
            vmData.IsPlugged = XBoxConInstance.IsPlugged;

            //Invocar un refresh de la ui
            appControlUiRefresh?.Invoke(this, vmData);

        }

        //Eventos de la UI actualizan info en el AppController
        private void Vmodel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Pasar datos a controlador del mando
            xBoxConInstance.DeltaT= vmData.DeltaT;
            XBoxConInstance.DeadZone = vmData.DeadZone;

            //Pasar datos a manipulador cinematico.
            kineM.DeltaT = vmData.DeltaT;
            kineM.OmegaMaxZ = VmData.OmegaMaxZ;
            kineM.VmaxZ = VmData.VmaxZ;
            kineM.VmaxR = VmData.VmaxR;

        }

        #endregion

    }
}
