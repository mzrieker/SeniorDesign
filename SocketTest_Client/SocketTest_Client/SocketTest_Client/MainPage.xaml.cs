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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SocketTest_Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        static string PortNumber = "1337";
        public StreamSocket streamSocket;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //this.StartServer();
            this.StartClient();
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
                    var hostName = new Windows.Networking.HostName("192.168.1.2");

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
                    StartClient();
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
        }

        private void btnSendHello_click(object sender, RoutedEventArgs e)
        {
            SendHello();
        }
    }
}
