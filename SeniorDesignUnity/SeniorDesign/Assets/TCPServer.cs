using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class TCPServer : MonoBehaviour
{
	public static void Main()
	{
		TcpListener server = null;
		try
		{
			// Set the TcpListener on port 13000.
			Int32 port = 1337;
			IPAddress localAddr = IPAddress.Parse("192.168.1.2");

			// TcpListener server = new TcpListener(port);
			server = new TcpListener(localAddr, port);

			// Start listening for client requests.
			server.Start();

			// Buffer for reading data
			Byte[] bytes = new Byte[256];
			String data = null;
			int counter = 0;


			// Enter the listening loop.
			while (true)
			{
				Console.Write("Waiting for a connection... ");

				// Perform a blocking call to accept requests.
				// You could also user server.AcceptSocket() here.
				TcpClient client = server.AcceptTcpClient();
				counter++;
				Console.WriteLine("#" + counter + " Connected!");

				data = null;

				// Get a stream object for reading and writing
				NetworkStream stream = client.GetStream();

				int i;

				// Loop to receive all the data sent by the client.
				while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
				{
					// Translate data bytes to a ASCII string.
					data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
					Console.WriteLine("Received: {0}", data + counter);

					// Process the data sent by the client.
					data = data.ToUpper();

					byte[] msg = System.Text.Encoding.ASCII.GetBytes(data + "Client counter:" + counter);

					// Send back a response.
					stream.Write(msg, 0, msg.Length);
					Console.WriteLine("Sent: {0}", data + "Client counter:" + counter);
				}

				// Shutdown and end connection
				client.Close();
			}
		}
		catch (SocketException e)
		{
			Console.WriteLine("SocketException: {0}", e);
		}
		finally
		{
			// Stop listening for new clients.
			server.Stop();
		}


		Console.WriteLine("\nHit enter to continue...");
		Console.Read();
	}
}