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
        }

        #endregion

        private void btControlDisconnect_Click(object sender, RoutedEventArgs e)
        {
            if (vm.DisconnectController())
            {
                flipButtons();
            }

        }

        private void btControlConnect_Click(object sender, RoutedEventArgs e)
        {
            if (vm.ConnectController())
            {
                flipButtons();
            }
        }

        private void flipButtons()
        {
            btControlConnect.IsEnabled = !btControlConnect.IsEnabled;
            btControlDisconnect.IsEnabled = !btControlDisconnect.IsEnabled;
        }
    }
}
