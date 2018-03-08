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
using SeniorDesign;
using Windows.Networking.Sockets;


namespace SocketTest
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        static string PortNumber = "1337";
        public string request;
        private StreamSocketListener streamSocketListener;
        private StreamSocket streamSocket;
        private StreamWriter streamWriter;
        private StreamReader streamReader;
        private Stream outputStream;
        public string command;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.StartServer();
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
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.serverListBox.Items.Add("Client Connected"));

            using (streamReader = new StreamReader(args.Socket.InputStream.AsStreamForRead()))
            {
                request = await streamReader.ReadLineAsync();
            }
        
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.serverListBox.Items.Add("Client Connected"));
        }
        
        // Echo the request back as the response.
        /*private async void btnSendHey_click(object sender, RoutedEventArgs e)
           {
                using (Stream outputStream = args.Socket.OutputStream.AsStreamForWrite())
                {
                    using (var streamWriter = new StreamWriter(outputStream))
                    {
                        await streamWriter.WriteLineAsync(request);
                        await streamWriter.FlushAsync();
                    }
                }

                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.serverListBox.Items.Add(string.Format("server sent back the response: \"{0}\"", request)));
            }*/

        private async void btnCloseServer_click(object sender, RoutedEventArgs e)
        {
            command = "Close Socket";
            using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
            {
                using (streamWriter = new StreamWriter(outputStream))
                {
                    await streamWriter.WriteLineAsync(command);
                    await streamWriter.FlushAsync();
                }
            }
            streamSocketListener.Dispose();
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.serverListBox.Items.Add("server closed its socket"));
        }

        private async void btnSendHey_click(object sender, RoutedEventArgs e)
        {
            command = "Hey";
            using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
            {
                using (streamWriter = new StreamWriter(outputStream))
                {
                    await streamWriter.WriteLineAsync(command);
                    await streamWriter.FlushAsync();
                }
            }
        }

        private async void btnIgnitionOn_Click(object sender, RoutedEventArgs e)
        {
            command = "On";
            using (outputStream = streamSocket.OutputStream.AsStreamForWrite())
            {
                using (streamWriter)
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
            using (outputStream)
            {
                using (streamWriter)
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
            using (outputStream)
            {
                using (streamWriter)
                {
                    await streamWriter.WriteLineAsync(command);
                    await streamWriter.FlushAsync();
                }
            }
        }
    }
}
