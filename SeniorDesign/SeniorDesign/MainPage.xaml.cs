using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Gpio;

// Initialize the GPIO pins on the Pi
namespace SeniorDesign
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            _controller = GpioController.GetDefault();
            _motorEnable = _controller.OpenPin(_pinEnable);
            _motorControlRightF = _controller.OpenPin(_pinRightF);
            _motorControlRightB = _controller.OpenPin(_pinRightB);
            _motorControlLeftF = _controller.OpenPin(_pinLeftF);
            _motorControlLeftB = _controller.OpenPin(_pinLeftB);
            _motorEnable.SetDriveMode(GpioPinDriveMode.Output);
            _motorControlRightF.SetDriveMode(GpioPinDriveMode.Output);
            _motorControlRightB.SetDriveMode(GpioPinDriveMode.Output);
            _motorControlLeftF.SetDriveMode(GpioPinDriveMode.Output);
            _motorControlLeftB.SetDriveMode(GpioPinDriveMode.Output);
        }
    }
}
