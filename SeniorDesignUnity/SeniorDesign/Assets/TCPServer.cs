using UnityEngine;
using UnityEngine.Networking;

public class TCPServer : MonoBehaviour
{

	public bool isAtStartup = true;

	NetworkClient myClient;

	public void Start()
	{
		SetupServer();
	}
	// Create a server and listen on a port
	public void SetupServer()
	{
		NetworkServer.Listen(1337);
	}

}