using AddinTesting.model;
using SharpDX.XInput;
using SolidWorks.Interop.sldworks;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;


namespace AddinTesting.ViewModel
{

    public class viewModel : INotifyPropertyChanged
    {
        xBoxControllerCom xBoxComm = new xBoxControllerCom();
        viewManipulator viewMan;
        SldWorks swApp;

        #region private fields
        private string mateName;
        #endregion

        #region Properties

        // Eje horizontal del joy izquierdo
        public string LeftThumbXAxis
{
            get
            {
                return (xBoxComm.LeftThumbX).ToString();
            }

        }
        //Eje vertical del joy izquierdo
        public string LeftThumbYAxis
        {
            get
            {
                return (xBoxComm.LeftThumbY).ToString();
            }

        }
        // Eje horizontal del joy derecho
        public string RightThumbXAxis
        {
            get
            {
                return (xBoxComm.RightThumbX).ToString();
            }

        }
        //Eje vertical del joy derecho
        public string RightThumbYAxis
        {
            get
            {
                return (xBoxComm.RightThumbY).ToString();
            }

        }

        //Trigger derecho
        public string RightTrigger
        {
            get
            {
                return (xBoxComm.RightTrigger).ToString();
            }

        }
        //Trigger izquierdo
        public string LeftTrigger
        {
            get
            {
                return (xBoxComm.LeftTrigger).ToString();
            }

        }

        // Casilla de seleccion de un mate.
        public string MateName
        {
            get { return mateName; }
            set { mateName = value; }
        }

        #endregion

        #region Metodos

        // Inicializar controlador del mando xbox
        public void initVM()
        {
            //Iniciar controlador del control de xbox
            xBoxComm.initXboxControllerComm();
            // Suscribir el handler XBoxComm_RaiseControllerUpdatedEvent al evento de la clase controladora del mando RaiseControllerUpdatedEvent
            xBoxComm.RaiseControllerUpdatedEvent += XBoxComm_RaiseControllerUpdatedEvent;
            // Instancia de solidworks!
            swApp= (SldWorks)System.Runtime.InteropServices.Marshal.GetActiveObject("SldWorks.Application");
            // Instanciar viewmanipulator y pasarle instancia de solid.
            viewMan = new viewManipulator(swApp,xBoxComm);
            
        }

        private void XBoxComm_RaiseControllerUpdatedEvent(object sender, EventArgs e)
        {
            viewMan.rotateView();
            OnPropertyChanged("LeftThumbXAxis");
            OnPropertyChanged("LeftThumbYAxis");
            OnPropertyChanged("RightThumbXAxis");
            OnPropertyChanged("RightThumbYAxis");
            OnPropertyChanged("RightTrigger");
            OnPropertyChanged("LeftTrigger");
        }

        #endregion


        #region Implementar la interfaz InotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
