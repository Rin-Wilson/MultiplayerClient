using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public enum ServerToClientId : ushort
{
    SpawnPlayer = 1,
    PlayerMovement,
}

public enum ClientToServerId : ushort
{
    PlayerName = 1,
    PlayerInput
}

public class ClientManager : MonoBehaviour
{
    private static ClientManager _singleton;
    public static ClientManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Destroy(value);
            }
        }
    }

    [SerializeField] private string ip;
    [SerializeField] private int port;

    [SerializeField] private GameObject _localPlayerPrefab;
    [SerializeField] private GameObject _remotePlayerPrefab;

    public GameObject LocalPlayerPrefab => _localPlayerPrefab;
    public GameObject RemotePlayerPrefab => _remotePlayerPrefab;

    public Client Client { get; private set; }

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        Client = new Client();
        Client.Connect($"{ip}:{port}");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Client.Update();
    }
}
