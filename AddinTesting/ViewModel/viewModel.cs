using AddinTesting.model;
using SharpDX.XInput;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;


namespace AddinTesting.ViewModel
{

    public class viewModel : INotifyPropertyChanged
    {
        xBoxControllerCom xBoxComm = new xBoxControllerCom();

        #region Private fields


        #endregion

        #region Properties

        public string RightTrigger
        {
            get
            {
                return xBoxComm.RtrgPercent;
            }
            set
            {

                OnPropertyChanged();
            }

        }

        #endregion
         
        
        #region Metodos

        public void initVM()
        {
            xBoxComm.initXboxControllerComm();
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
