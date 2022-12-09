using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;
using System;

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
        Client.Connected += DidConnect;
        Client.ClientDisconnected += PlayerLeft;
        Client.Disconnected += DidDisconnect;
        Client.ConnectionFailed += ConnectionFailed;
    }

    public void Connect()
    {
        Client.Connect($"{ip}:{port}");
    }

    private void FixedUpdate()
    {
        Client.Update();
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        Destroy(Player.list[e.Id].gameObject);
    }

    private void DidDisconnect(object sender, DisconnectedEventArgs e)
    {
        foreach (Player player in Player.list.Values)
            Destroy(player.gameObject);

        UIManager.Singleton.BackToMenu();
    }

    private void DidConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.SendUsername();
    }

    private void ConnectionFailed(object sender, ConnectionFailedEventArgs e) {
        UIManager.Singleton.BackToMenu();
    }
}
