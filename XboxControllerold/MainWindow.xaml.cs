using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using SharpDX.XInput;

namespace XboxController
{

    public partial class MainWindow : INotifyPropertyChanged
    {
        DispatcherTimer _timer = new DispatcherTimer();
        private Controller _controller;


        private string _leftAxis;
        private string _rightAxis;
        private string _buttons;
        private string rightTrigger;
        private string leftTrigger;

        // Thumbstick percentage
        double ltsXPercent;
        double ltsYPercent;
        double rtsXPercent;
        double rtsYPercent;

        //Trigger percentage
        double rtrgPercent;
        double ltrgPercent;


        // Thumbstick + limits. - Limits are symmetrical
        private const int tsRightLimit = 32767;
        private const int tsUpperLimit = 32767;
        private const int triggerLimit = 255;


        public MainWindow()
        {
            DataContext = this;
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(20);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            DisplayControllerInformation();
        }

        void DisplayControllerInformation()
        {
            try
            {
                var state = _controller.GetState();

                // Thumbstick percentage
                ltsXPercent = Math.Round(100 - ((double)(tsRightLimit - state.Gamepad.LeftThumbX) / tsRightLimit) * 100);
                ltsYPercent = Math.Round(100 - ((double)(tsUpperLimit - state.Gamepad.LeftThumbY) / tsUpperLimit) * 100);
                rtsXPercent = Math.Round(100 - ((double)(tsRightLimit - state.Gamepad.RightThumbX) / tsRightLimit) * 100);
                rtsYPercent = Math.Round(100 - ((double)(tsUpperLimit - state.Gamepad.RightThumbY) / tsUpperLimit) * 100);

                // Trigger percentage

                rtrgPercent = Math.Round(100 - ((double)(triggerLimit - state.Gamepad.RightTrigger) / triggerLimit) * 100);
                ltrgPercent = Math.Round(100 - ((double)(triggerLimit - state.Gamepad.LeftTrigger) / triggerLimit) * 100);

                RightTrigger = rtrgPercent.ToString();
                LeftTrigger = ltrgPercent.ToString();
                LeftAxis = string.Format("X: {0} % Y: {1} %", ltsXPercent, ltsYPercent);
                RightAxis = string.Format("X: {0} % Y: {1} %", rtsXPercent, rtsYPercent);
                Buttons = string.Format("{0}", state.Gamepad.Buttons);
            }
            catch (Exception e)
            {
                MessageBox.Show("Excepcion en  DisplayControllerInformation() " + e.Message);
                Environment.Exit(1);
            }
            

        }

        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            _controller = null;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _controller = new Controller(UserIndex.One);
            if (_controller.IsConnected) return;
            MessageBox.Show("Game Controller is not connected ... you know ;)");
            App.Current.Shutdown();
        }

        #region Properties

        public string RightTrigger
        {
            get
            {
                return rightTrigger;
            }
            set
            {
                if (value == rightTrigger) return;
                rightTrigger = value;
                OnPropertyChanged();
            }
        }

        public string LeftTrigger
        {
            get
            {
                return leftTrigger;
            }
            set
            {
                if (value == leftTrigger) return;
                leftTrigger = value;
                OnPropertyChanged();
            }
        }

        public string LeftAxis
        {
            get
            {
                return _leftAxis;
            }
            set
            {
                if (value == _leftAxis) return;
                _leftAxis = value;
                OnPropertyChanged();
            }
        }

        public string RightAxis
        {
            get
            {
                return _rightAxis;
            }
            set
            {
                if (value == _rightAxis) return;
                _rightAxis = value;
                OnPropertyChanged();
            }
        }

        public string Buttons
        {
            get
            {
                return _buttons;
            }
            set
            {
                if (value == _buttons) return;
                _buttons = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
