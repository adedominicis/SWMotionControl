using System.ComponentModel;
using System.Windows.Controls;
using static AngelSix.SolidDna.SolidWorksEnvironment;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using SharpDX.XInput;
using System;
using AddinTesting.ViewModel;
using AddinTesting.model;


namespace AddinTesting
{
    /// <summary>
    /// Interaction logic for MyAddinControl.xaml
    /// </summary>
    public partial class MyAddinControl : UserControl
    {
        //DATACONTEXT.
        private viewModel vm;
        

        #region Constructor


        public MyAddinControl()
        {

            //El datacontext es el viewmodel, el viewmodel se comunica con todas las otras clases 
            vm = new viewModel();
            this.DataContext = vm;
            vm.initVM();
            InitializeComponent();
            vm.xBoxComm.controllerPlugStateChanged += XBoxComm_controllerPlugStateChanged;


        }

        #endregion

        #region Listeners
        private void XBoxComm_controllerPlugStateChanged(object sender, bool status)
        {
            if (status)
            {
                MessageBox.Show("Mando XBOX ONE conectado!");
            }
            else
            {
                MessageBox.Show("Mando XBOX ONE desconectado!");
            }
            btControlDisconnect.IsEnabled = status;
        }

        //Conectar el control
        private void btControlConnect_Click(object sender, RoutedEventArgs e)
        {
            vm.ConnectController();
        }

        //Desconectar el control
        private void btControlDisconnect_Click(object sender, RoutedEventArgs e)
        {
            vm.DisconnectController();
        }

        //Caja mate ha sido seleccionada, un listener de seleccion en SW debe activarse.

        private void MateNameGotFocus(object sender, RoutedEventArgs e)
        {
            vm.listenForMateSelection(true);
        }
        #endregion
    }
}
