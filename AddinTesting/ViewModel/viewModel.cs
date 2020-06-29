using AddinTesting.model;
using SolidWorks.Interop.sldworks;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;


namespace AddinTesting.ViewModel
{

    public class viewModel : INotifyPropertyChanged
    {
        
        #region private fields
        private string mateName;

        viewManipulator viewMan;
        SldWorks swApp;
        ModelDoc2 swModel;
        SelectionListener sl;


        #endregion

        #region Properties y eventos publicos.

        public xBoxControllerCom xBoxComm = new xBoxControllerCom();

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

        #region Metodos de inicializacion

        public void initVM()
        {
            
            try
            {
                // Obtener Instancia de solidworks
                swApp = (SldWorks)System.Runtime.InteropServices.Marshal.GetActiveObject("SldWorks.Application");
                //Suscribirse al evento de cambio de documento, documento nuevo y cambio de documento.
                swApp.ActiveDocChangeNotify += SwApp_ActiveDocChangeNotify;
                swApp.FileOpenNotify += SwApp_FileOpenNotify;
                swApp.FileNewNotify2 += SwApp_FileNewNotify2;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
            //Modelo abierto en pantalla, en la instancia de solidworks.
        }

        private void documentSwapHandler()
        {
            try
            {
                if ((ModelDoc2)swApp.ActiveDoc != null)
                {
                    swModel = (ModelDoc2)swApp.ActiveDoc;
                    //Iniciar selection listener.
                    sl = new SelectionListener(swModel);
                }
                else
                {
                    MessageBox.Show("Abra un documento");
                    sl = null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        // Handlers de cambio de documento de solidworks. Todos redireccionan a Document Swap Handler
        private int SwApp_FileNewNotify2(object NewDoc, int DocType, string TemplateName)
        {
            documentSwapHandler();
            return 0;
        }
        private int SwApp_FileOpenNotify(string FileName)
        {
            documentSwapHandler();
            return 0;
        }
        private int SwApp_ActiveDocChangeNotify()
        {
            documentSwapHandler();
            return 0;
        }

        // Conectar mando de xbox
        internal bool ConnectController()
        {

            try
            {
                //Instanciar e iniciar controlador del mando
                if (xBoxComm.initXboxControllerComm())
                {
                    // Suscribirse al evento del control. Este evento se dispara cada vez que el usuario usa los elementos del mando.
                    xBoxComm.controllerUpdatedEvent += controllerUpdatedEventHandler;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

        }

        // Desconectar mando de xbox
        internal void DisconnectController()
        {
            try
            {
                if (xBoxComm!=null)
                {
                    //Desuscribirse del evento controllerupdate
                    xBoxComm.controllerUpdatedEvent -= controllerUpdatedEventHandler;
                    //Finalizar conexion con el control
                    xBoxComm.endXboxControllerComm();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion

        #region Listeners

        //Cuando hay foco en la casilla de mate selection, hay que estar pendientes de capturar selecciones
        internal void listenForMateSelection(bool listen)
        {
            if (listen)
            {
                
            }
        }

        private void controllerUpdatedEventHandler(object sender, EventArgs e)
        {
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
