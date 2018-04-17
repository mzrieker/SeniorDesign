using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;
using System;
using System.Threading;


public class TCPServer : MonoBehaviour
{
	public TcpListener server;
	public TcpClient client;
	public Thread mThread;
	public NetworkStream stream;
	byte[] msg1;
	string toSend;
	public bool isConnection, senddata1, senddata2;

	void Start()
	{
		isConnection = false;
		senddata1 = false;
		senddata2 = false;
		//print ("StartThread");
		ThreadStart ts = new ThreadStart(Update);
		mThread = new Thread(ts);
		mThread.Start();
	}
	void Update()
	{
		server = null;
		try
		{
			// Set the TcpListener on port 1337.
			Int32 port = 3333;
			//IPAddress localAddr = IPAddress.Parse("192.168.1.4");

			// TcpListener server = new TcpListener(port);
			server = new TcpListener(IPAddress.Any, port);

			// Start listening for client requests.
			server.Start();

			// Buffer for reading data
			Byte[] bytes = new Byte[256];
			String data = null;

			// Enter the listening loop.
			while (true)
			{
				Thread.Sleep(10);

				Debug.Log("Waiting for a connection... ");

				// Perform a blocking call to accept requests.
				// You could also user server.AcceptSocket() here.
				client = server.AcceptTcpClient();
				if (client != null)
				{

					Debug.Log("Connected!");
					//isConnection=true;
					//client.Close();
					//break;

				}
				/*data = null;

				// Get a stream object for reading and writing
				stream = client.GetStream();
				StreamWriter swriter = new StreamWriter(stream);
				int i;

				// Loop to receive all the data sent by the client.
				while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
				{
					//msg1 = System.Text.Encoding.ASCII.GetBytes(prevdata);

					// Send back a response.
					//stream.Write(msg1, 0, msg1.Length);
					// Translate data bytes to a ASCII string.
					data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
					//Debug.Log("Received:"+ data+"data");


					//Debug.Log("Sent:"+ data);
					// Process the data sent by the client.
					bool isTrue = false;
					switch (data)
					{
						case "params":
							//senddata=true;
							string stairways1 = Stairways();
							string rooms1 = Rooms();
							toSend = floors.ToString() + "/" + stairways1 + "/" + rooms1;
							//print (toSend);
							prevdata = toSend;
							msg1 = System.Text.Encoding.ASCII.GetBytes(toSend);
							stream.Write(msg1, 0, msg1.Length);
							//stream.Flush();
							break;

						case "isZooming":
							toSend = zoom.isZooming.ToString() + ",";
							//print (toSend);
							prevdata = toSend;
							msg1 = System.Text.Encoding.ASCII.GetBytes(toSend);
							stream.Write(msg1, 0, msg1.Length);
							//stream.Flush();
							break;
						case "isZoomed":
							toSend = zoom.isZoomed.ToString() + ":";
							prevdata = toSend;
							//print (toSend);
							msg1 = System.Text.Encoding.ASCII.GetBytes(toSend);
							stream.Write(msg1, 0, msg1.Length);
							stream.Flush();
							break;
							break;
						case "Zoompressed1,true":
							senddata1 = true;

							//stream.Flush();
							break;
						case "Zoompressed2,true":
							senddata2 = true;

							break;
						case "Left,true":
							//msg1 = System.Text.Encoding.ASCII.GetBytes(data);
							//stream.Write (msg1, 0, msg1.Length);
							zoom.rotatearound1 = true;
							break;
						case "Left,Stop":
							//msg1 = System.Text.Encoding.ASCII.GetBytes(data);
							//stream.Write (msg1, 0, msg1.Length);
							zoom.rotatearound1 = false;
							break;
						case "Right,true":
							//msg1 = System.Text.Encoding.ASCII.GetBytes(data);
							//stream.Write (msg1, 0, msg1.Length);
							zoom.rotatearound2 = true;
							break;
						case "Right,Stop":
							//msg1 = System.Text.Encoding.ASCII.GetBytes(data);
							//stream.Write (msg1, 0, msg1.Length);
							zoom.rotatearound2 = false;
							break;
						case "Up,true":
							//msg1 = System.Text.Encoding.ASCII.GetBytes(data);
							//stream.Write (msg1, 0, msg1.Length);
							zoom.rotatearound3 = true;
							break;
						case "Up,Stop":
							//msg1 = System.Text.Encoding.ASCII.GetBytes(data);
							//stream.Write (msg1, 0, msg1.Length);
							zoom.rotatearound3 = false;
							break;
						case "Down,true":
							//msg1 = System.Text.Encoding.ASCII.GetBytes(data);
							//stream.Write (msg1, 0, msg1.Length);
							zoom.rotatearound4 = true;
							break;
						case "Down,Stop":
							//msg1 = System.Text.Encoding.ASCII.GetBytes(data);
							//stream.Write (msg1, 0, msg1.Length);
							zoom.rotatearound4 = false;
							break;
						case "ZoomIn,true":
							//msg1 = System.Text.Encoding.ASCII.GetBytes(data);
							//stream.Write (msg1, 0, msg1.Length);
							zoom.distanceChange1 = true;
							break;
						case "ZoomIn,Stop":
							//msg1 = System.Text.Encoding.ASCII.GetBytes(data);
							//stream.Write (msg1, 0, msg1.Length);
							zoom.distanceChange1 = false;
							break;
						case "ZoomOut,true":
							//msg1 = System.Text.Encoding.ASCII.GetBytes(data);
							//stream.Write (msg1, 0, msg1.Length);
							zoom.distanceChange2 = true;
							break;
						case "ZoomOut,Stop":
							//msg1 = System.Text.Encoding.ASCII.GetBytes(data);
							//stream.Write (msg1, 0, msg1.Length);
							zoom.distanceChange2 = false;
							break;
						case "Disconnect":
							goto q;
							client.Close();
							break;
						case "Show":
							show1 = true;
							break;
						case "Hide":
							hide = true;

							break;
						default:
							byte[] msg = System.Text.Encoding.ASCII.GetBytes(prevdata);

							// Send back a response.
							stream.Write(msg, 0, msg.Length);
							//isTrue=true;
							//msg1 = System.Text.Encoding.ASCII.GetBytes(data);
							//stream.Write (msg1, 0, msg1.Length);
							//senddata=true;
							//StartCoroutine(sendString1(stream));
							//sendString1(stream);
							break;
					}
					bool contains1 = data.Contains("Left,Stop");
					if (contains1) zoom.rotatearound1 = false;
					bool contains2 = data.Contains("Right,Stop");
					if (contains2) zoom.rotatearound2 = false;
					bool contains3 = data.Contains("Up,Stop");
					if (contains3) zoom.rotatearound3 = false;
					bool contains4 = data.Contains("Down,Stop");
					if (contains4) zoom.rotatearound4 = false;
					bool contains5 = data.Contains("ZoomIn,Stop");
					if (contains5) zoom.distanceChange1 = false;
					bool contains6 = data.Contains("ZoomOut,Stop");
					if (contains6) zoom.distanceChange2 = false;
					if (data.StartsWith("("))
					{
						//data=subS(data,'(');
						parsedata = true;
						//selection=true;

						byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
						prevdata = data;
						//Debug.Log("SentPrevdata:"+ prevdata);
						// Send back a response.
						stream.Write(msg, 0, msg.Length);
					}
					if (data == " ")
					{
						byte[] msg = System.Text.Encoding.ASCII.GetBytes(prevdata);
						//Debug.Log("SentPrevdata:"+ prevdata);
						// Send back a response.
						stream.Write(msg, 0, msg.Length);
					}
					if (data.StartsWith("!"))
					{
						//Debug.Log(data);
						Vector3 vector;
						try
						{
							vector = parseVector3(data);
							pos = vector;
							endpointChanged = true;
							//print (vector.ToString());
						}
						catch (Exception e)
						{
						}
					}
					if (data.StartsWith("&"))
					{
						//Debug.Log(data);
						float timeOfDay = 0.0f;
						try
						{
							timeOfDay = float.Parse(data.Substring(1, data.Length - 1));
							sun.currentTime = timeOfDay;
						}
						catch (FormatException e)
						{
						}
						//endpointChanged=true;
						//print (timeOfDay.ToString());
					}
					if (data.StartsWith("#"))
					{
						//Debug.Log(data);
						float cloudiness = 0.0f;
						try
						{
							cloudiness = float.Parse(data.Substring(1, data.Length - 1));
							sun.cloudiness = cloudiness;
						}
						catch (FormatException e)
						{
						}
						//endpointChanged=true;
						//print (cloudiness.ToString());
					}
					//Debug.Log("Sent:"+ data);
				}*/
				//q:
				//show1 = true;
				// Shutdown and end connection
				client.Close();
			}
		}
		catch (SocketException e)
		{
			Debug.Log("SocketException:" + e);
		}
		finally
		{
			// Stop listening for new clients.
			server.Stop();
		}

		//yield return null;
	}
}
