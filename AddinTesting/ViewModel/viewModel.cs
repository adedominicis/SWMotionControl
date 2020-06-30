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
        private SldWorks swApp;
        private viewModelData vmData=new viewModelData();
        private AppControl miApp;
        
        #endregion

        #region UI Bound properties.

        // Eje horizontal del joy izquierdo
        public string LeftThumbXAxis
        {
            get
            {
                return vmData.LeftThumbX.ToString();
            }

        }
        //Eje vertical del joy izquierdo
        public string LeftThumbYAxis
        {
            get
            {
                return vmData.LeftThumbY.ToString();
            }

        }
        // Eje horizontal del joy derecho
        public string RightThumbXAxis
        {
            get
            {
                return vmData.RightThumbX.ToString();
            }

        }
        //Eje vertical del joy derecho
        public string RightThumbYAxis
        {
            get
            {
                return vmData.RightThumbY.ToString();
            }

        }
        //Trigger derecho
        public string RightTrigger
        {
            get
            {
                return vmData.RightTrigger.ToString();
            }

        }
        //Trigger izquierdo
        public string LeftTrigger
        {
            get
            {
                return vmData.LeftTrigger.ToString();
            }

        }

        #endregion

        #region Inicializar

        public void initVM()
        {

            try
            {
                // Obtener Instancia activa de solidworks
                swApp = (SldWorks)System.Runtime.InteropServices.Marshal.GetActiveObject("SldWorks.Application");
                // Generar instancia de app control
                miApp = new AppControl(swApp, this);
                //Suscribirse a eventos de miapp.
                miApp.appControlUiRefresh -= MiApp_appControlUiRefresh;
                miApp.appControlUiRefresh += MiApp_appControlUiRefresh;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error en inicializacion de viewModel: "+e.Message);
            }
        }

        #endregion

        #region Metodos de transferencia desde la UI

        /// <summary>
        /// Commands de la UI
        /// </summary>

        public bool ConnectController()
        {
            return miApp.ConnectController();
        }

        public bool DisconnectController()
        {
            return miApp.DisconnectController();
        }

        #endregion

        #region Listeners

        //Cuando hay foco en la casilla de mate selection, hay que estar pendientes de capturar selecciones

        /// <summary>
        /// Cuando se requiere un refresh de la UI desde miApp, este handler responde al disparo del evento 
        /// AppControl.appControlUiRefresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="vmdata"></param>
        private void MiApp_appControlUiRefresh(object sender, viewModelData vmdata)
        {
            vmData = (viewModelData)vmdata;
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
