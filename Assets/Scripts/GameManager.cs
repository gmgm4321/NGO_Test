using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject NetworkPanel;

    public InputField ipinput;
    public Text stateText;
    private void Awake()
    {
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateHost() {

        NetworkManager.Singleton.StartHost();
        stateText.text = $"Mode: {(NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client")}";
        NetworkPanel.SetActive(false);
    }

    public void JoinServer()
    {

        string ip = ipinput.text;
        if (ipinput.text.Equals(""))
        {

            ip = "127.0.0.1";
            print("No Enter，IP:127.0.0.1");
        }
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData
            (
                 "127.0.0.1",  // The IP address is a string
                 (ushort)7777 // The port number is an unsigned short
            );
         NetworkManager.Singleton.StartClient();
        stateText.text = $"Mode: {(NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client")}";
        NetworkPanel.SetActive(false);
    }
}

