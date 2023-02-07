using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;
using Riptide.Utils;

public enum ServerToClientId : ushort
{

}

public enum ClientToServerId : ushort
{
    moveInput = 1
}

public class NetworkManager : MonoBehaviour
{
    [SerializeField] private ushort port = 7777;
    [SerializeField] private string ip = "127.0.0.1";

    private Vector2 moveInput;

    [SerializeField] private float tickRate = 60;
    private float tickTimer;

    public float tickInterval { get; private set; }
    public uint currentTick { get; private set; }

    public Client Client { get; private set; }

    void Start()
    {
        tickInterval = 1f / tickRate;

        Client = new Client();
        Client.ClientConnected += OnConnect;

        Client.Connect($"{ip}:{port}");
    }


    void Update()
    {
        tickTimer += Time.deltaTime;

        while (tickTimer >= tickInterval)
        {
            tickTimer -= tickInterval;

            Tick();
            currentTick++;
        }

        moveInput = new()
        {
            x = Input.GetAxis("Horizontal"),
            y = Input.GetAxis("Vertical")
        };
    }

    void Tick()
    {
        if (Client.IsConnected)
        if (currentTick % 2 == 0)
        {
            SendMovement();
        }
        Client.Update();
    }

    void SendMovement()
    {
        Message m = Message.Create(MessageSendMode.Unreliable, ClientToServerId.moveInput);
        m.AddVector2(moveInput);

        Client.Send(m);
    }
    

    private void OnConnect(object sender, ClientConnectedEventArgs e)
    {

    }
}
