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
        private MateManipulator mateM;
        private SelectionListener selListener;
        private xBoxControllerCom xBoxConInstance;
        private viewModelData vmData=new viewModelData();
        #endregion

        #region public properties

        // Exponer selection listener
        public SelectionListener SelListener
        {
            get { return selListener; }
        }
        public xBoxControllerCom XBoxConInstance
        {
            get { return xBoxConInstance; }
        }

        #endregion

        #region Constructor
        public AppControl(SldWorks swapp, viewModel vmodel)
        {
            //instancia actual de solidworks
            swApp = (SldWorks)swapp;
            //Instanciar un listener de selecciones
            selListener = new SelectionListener(swApp);
            //Instanciar el controlador para xbox
            xBoxConInstance = new xBoxControllerCom();
            //Inicializar Mate Manipulator
            mateM = new MateManipulator(this);
        }
        #endregion

        #region Connect and disconnect peripherals
        // Conectar mando de xbox
        public bool ConnectController()
        {

            try
            {
                //Instanciar e iniciar controlador del mando
                if (xBoxConInstance.initXboxControllerComm())
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

        // 
        private void controllerUpdatedEventHandler(object sender, EventArgs e)
        {
            //Actualizar vmdata para todos los analogos.
            vmData.LeftThumbX = xBoxConInstance.LeftThumbX;
            vmData.LeftThumbY= xBoxConInstance.LeftThumbY;
            vmData.RightThumbX = xBoxConInstance.RightThumbX;
            vmData.RightThumbY = xBoxConInstance.RightThumbY;
            vmData.RightTrigger = xBoxConInstance.RightTrigger;
            vmData.LeftTrigger = xBoxConInstance.LeftTrigger;

            //Invocar un refresh de la ui
            appControlUiRefresh?.Invoke(this, vmData);

        }
        #endregion

    }
}
