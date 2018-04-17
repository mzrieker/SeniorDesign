using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Networking.Sockets;
using Windows.Devices.Gpio;

namespace SocketTest
{
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
            this.InitializeComponent(); _controller = GpioController.GetDefault();
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
			StartServer();
        }

        static string PortNumber = "1337";
        public string request;
        private StreamSocketListener streamSocketListener;
        private StreamSocket streamSocket;
        //private StreamWriter streamWriter;
        private StreamReader streamReader;
        //private Stream outputStream;
        public string command;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //this.StartServer();
            //this.StartClient();
        }

        private async void StartServer()
        {
            try
            {
                streamSocketListener = new Windows.Networking.Sockets.StreamSocketListener();

                // The ConnectionReceived event is raised when connections are received.
                streamSocketListener.ConnectionReceived += this.StreamSocketListener_ConnectionReceived;

                // Start listening for incoming TCP connections on the specified port. 
                // You can specify any port that's not currently in use. Here I chose port 1337.
                await streamSocketListener.BindServiceNameAsync(MainPage.PortNumber);

                this.serverListBox.Items.Add("server is listening...");
            }
            catch (Exception ex)
            {
                Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(ex.GetBaseException().HResult);
                this.serverListBox.Items.Add(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
            }
        }

        private async void StreamSocketListener_ConnectionReceived(Windows.Networking.Sockets.StreamSocketListener sender, Windows.Networking.Sockets.StreamSocketListenerConnectionReceivedEventArgs args)
        {
            streamSocket = args.Socket;
            //await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.serverListBox.Items.Add("Hololens Connected"));

            using (streamReader = new StreamReader(args.Socket.InputStream.AsStreamForRead()))
            {
                request = await streamReader.ReadLineAsync();
            }
        
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.serverListBox.Items.Add("Hololens Connected"));
			await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.serverListBox.Items.Add("Hololens sent: " + request));

			if (request == "On")
			{
				_turnOnIgnition();
			}

			if (request == "Forwards")
			{
				_forwardMotor();
			}

			else if (request == "Backwards")
			{
				_reverseMotor();
			}

			else if (request == "Left")
			{
				_turnLeft();
			}

			else if (request == "Right")
			{
				_turnRight();
			}

			else if (request == "Stop")
			{
				_stopMotor();
			}

			else if (request == "Off")
			{
				_turnOffIgnition();
			}
		}
		public void _turnOnIgnition()
		{
			System.Threading.Tasks.Task.Delay(300).Wait();
			_motorEnable.Write(GpioPinValue.High);
		}

		public void _forwardMotor()
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

		private async void btnCloseServer_click(object sender, RoutedEventArgs e)
		{
			/*command = "Close Socket";
            using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
            {
                using (var streamWriter = new StreamWriter(outputStream))
                {
                    await streamWriter.WriteLineAsync(command);
                    await streamWriter.FlushAsync();
                }
            }*/
			streamSocketListener.Dispose();
			await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.serverListBox.Items.Add("Server closed the socket"));
		}

		private async void btnOpenServer_click(object sender, RoutedEventArgs e)
		{
			await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.serverListBox.Items.Add("Server is opening..."));
			StartServer();
		}

		private async void btnIgnitionOn_Click(object sender, RoutedEventArgs e)
		{
			command = "On";
			using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
			{
				using (var streamWriter = new StreamWriter(outputStream))
				{
					await streamWriter.WriteLineAsync(command);
					await streamWriter.FlushAsync();
				}
			}
		}

		private async void btnIgnitionOff_Click(object sender, RoutedEventArgs e)
		{
			command = "Off";
			using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
			{
				using (var streamWriter = new StreamWriter(outputStream))
				{
					await streamWriter.WriteLineAsync(command);
					await streamWriter.FlushAsync();
				}
			}
		}

		private async void btnForward_Click(object sender, RoutedEventArgs e)
		{
			command = "Forward";
			using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
			{
				using (var streamWriter = new StreamWriter(outputStream))
				{
					await streamWriter.WriteLineAsync(command);
					await streamWriter.FlushAsync();
				}
			}
		}

		private async void btnReverse_Click(object sender, RoutedEventArgs e)
		{
			command = "Reverse";
			using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
			{
				using (var streamWriter = new StreamWriter(outputStream))
				{
					await streamWriter.WriteLineAsync(command);
					await streamWriter.FlushAsync();
				}
			}
		}
		private async void btnTurnL_Click(object sender, RoutedEventArgs e)
		{
			command = "Turn Left";
			using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
			{
				using (var streamWriter = new StreamWriter(outputStream))
				{
					await streamWriter.WriteLineAsync(command);
					await streamWriter.FlushAsync();
				}
			}
		}
		private async void btnTurnR_Click(object sender, RoutedEventArgs e)
		{
			command = "Turn Right";
			using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
			{
				using (var streamWriter = new StreamWriter(outputStream))
				{
					await streamWriter.WriteLineAsync(command);
					await streamWriter.FlushAsync();
				}
			}
		}

		private async void btnStop_Click(object sender, RoutedEventArgs e)
		{
			command = "Stop";
			using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
			{
				using (var streamWriter = new StreamWriter(outputStream))
				{
					await streamWriter.WriteLineAsync(command);
					await streamWriter.FlushAsync();
				}
			}
		}
	}
}
