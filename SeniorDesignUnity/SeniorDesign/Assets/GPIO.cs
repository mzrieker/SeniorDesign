using System;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SeniorDesign
{
	    /// <summary>
	    /// Basic Bi-Directional Control of a single DC Motor
	    /// </summary>
	    public sealed partial class MainPage : Page
	{
		private int _pinEnable = 25;
		private int _pinRightF = 12;
		private int _pinRightB = 16;
		private int _pinLeftF = 21;
		private int _pinLeftB = 20;

		private GpioController _controller;
		private GpioPin _motorEnable;
		private GpioPin _motorControlRightF;
		private GpioPin _motorControlRightB;
		private GpioPin _motorControlLeftF;
		private GpioPin _motorControlLeftB;

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

		private void _turnOnIgnition()
		{
			System.Threading.Tasks.Task.Delay(300).Wait();
			_motorEnable.Write(GpioPinValue.High);
		}

		private void _forwardMotor()
		{
			System.Threading.Tasks.Task.Delay(300).Wait();
			_motorControlRightF.Write(GpioPinValue.High);
			_motorControlRightB.Write(GpioPinValue.Low);
			_motorControlLeftF.Write(GpioPinValue.High);
			_motorControlLeftB.Write(GpioPinValue.Low);
		}

		private void _turnLeft()
		{
			System.Threading.Tasks.Task.Delay(300).Wait();
			_motorControlRightF.Write(GpioPinValue.High);
			_motorControlRightB.Write(GpioPinValue.Low);
			_motorControlLeftF.Write(GpioPinValue.Low);
			_motorControlLeftB.Write(GpioPinValue.High);
		}

		private void _turnRight()
		{
			System.Threading.Tasks.Task.Delay(300).Wait();
			_motorControlRightF.Write(GpioPinValue.Low);
			_motorControlRightB.Write(GpioPinValue.High);
			_motorControlLeftF.Write(GpioPinValue.High);
			_motorControlLeftB.Write(GpioPinValue.Low);
		}

		private void _reverseMotor()
		{
			System.Threading.Tasks.Task.Delay(300).Wait();
			_motorControlRightF.Write(GpioPinValue.Low);
			_motorControlRightB.Write(GpioPinValue.High);
			_motorControlLeftF.Write(GpioPinValue.Low);
			_motorControlLeftB.Write(GpioPinValue.High);
		}

		private void _stopMotor()
		{
			System.Threading.Tasks.Task.Delay(300).Wait();
			_motorControlRightF.Write(GpioPinValue.Low);
			_motorControlRightB.Write(GpioPinValue.Low);
			_motorControlLeftF.Write(GpioPinValue.Low);
			_motorControlLeftB.Write(GpioPinValue.Low);
		}

		private void _turnOffIgnition()
		{
			System.Threading.Tasks.Task.Delay(300).Wait();
			_motorEnable.Write(GpioPinValue.Low);
			_motorControlRightF.Write(GpioPinValue.Low);
			_motorControlRightB.Write(GpioPinValue.Low);
			_motorControlLeftF.Write(GpioPinValue.Low);
			_motorControlLeftB.Write(GpioPinValue.Low);
		}
	}
}