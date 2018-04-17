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
using Windows.Networking.Sockets;
using Windows.Devices.Gpio;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SocketTest_Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
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
<<<<<<< HEAD
			/*
            _controller = GpioController.GetDefault();
=======
            /*_controller = GpioController.GetDefault();
>>>>>>> 645f86dc6c601e4c8198ad942ef9e9b8cbba2c81
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
			*/
        }


        static string PortNumber = "1338";
        public StreamSocket streamSocket;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //this.StartServer();
            this.StartClient();
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

        public async void StartClient()
        {
            try
            {
                // Create the StreamSocket and establish a connection to the echo server.
                using (streamSocket = new Windows.Networking.Sockets.StreamSocket())
                {
                    // The server hostname that we will be establishing a connection to.
                    // In this example, the server and client are in the same process.
<<<<<<< HEAD
                    var hostName = new Windows.Networking.HostName("192.168.1.4");
=======
                    var hostName = new Windows.Networking.HostName("localhost");
>>>>>>> 645f86dc6c601e4c8198ad942ef9e9b8cbba2c81

                    this.clientListBox.Items.Add("client is trying to connect...");

                    await streamSocket.ConnectAsync(hostName, MainPage.PortNumber);

                    this.clientListBox.Items.Add("client connected");

                    // Read data from the echo server.
                    string response;
                    using (Stream inputStream = streamSocket.InputStream.AsStreamForRead())
                    {
                        using (StreamReader streamReader = new StreamReader(inputStream))
                        {
                            response = await streamReader.ReadLineAsync();
                        }
                    }

                    this.clientListBox.Items.Add(string.Format("client received the response: \"{0}\" ", response));
                    if (response == "Close Socket")
                    {
                        streamSocket.Dispose();
                        this.clientListBox.Items.Add("Client Socket Closed");
                    }
                    else if (response == "On")
                    {
                        _turnOnIgnition();
                        StartClient();
                    }
                    else if (response == "Off")
                    {
                        _turnOffIgnition();
                        StartClient();
                    }
                    else if (response == "Forward")
                    {
                        _forwardMotor();
                        StartClient();
                    }
                    else if (response == "Reverse")
                    {
                        _reverseMotor();
                        StartClient();
                    }
                    else if (response == "Turn Left")
                    {
                        _turnLeft();
                        StartClient();
                    }
                    else if (response == "Turn Right")
                    {
                        _turnRight();
                        StartClient();
                    }
                    else if (response == "Stop")
                    {
                        _stopMotor();
                        StartClient();
                    }
                    else
                    {
                        StartClient();
                    }
                }

                //this.clientListBox.Items.Add("client closed its socket");
            }
            catch (Exception ex)
            {
                Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(ex.GetBaseException().HResult);
                this.clientListBox.Items.Add(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
            }
        }

        // Send a request to the echo server.
        private async void SendHello()
        {
            string request = "Hello, World!";
            using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
            {
                using (var streamWriter = new StreamWriter(outputStream))
                {
                    await streamWriter.WriteLineAsync(request);
                    await streamWriter.FlushAsync();
                }
            }

            this.clientListBox.Items.Add(string.Format("client sent the request: \"{0}\"", request));
            StartClient();
        }

        private void btnSendHello_click(object sender, RoutedEventArgs e)
        {
            SendHello();
        }

    }
}
