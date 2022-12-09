using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;
using Riptide.Utils;
using TMPro;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    [SerializeField] private ushort id;
    [SerializeField] private string username;
    [SerializeField] private TMP_Text usernameText;

    private void OnDestroy()
    {
        list.Remove(id);
    }

    public static void Spawn(ushort id, string username, Vector3 position)
    {
        Player player;
        if (id == ClientManager.Singleton.Client.Id)
            player = Instantiate(ClientManager.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
        else
            player = Instantiate(ClientManager.Singleton.RemotePlayerPrefab, position, Quaternion.identity).GetComponent<Player>();

        player.name = $"{id} : {username}";
        player.username = username;
        player.id = id;
        player.usernameText.text = $"{id} : {username}";
        list.Add(player.id, player);
    }

    public void Move(Vector3 position, Vector3 rotation)
    {
        transform.position = position;

        if (id != ClientManager.Singleton.Client.Id)
            transform.forward = rotation;
    }

    [MessageHandler((ushort)ServerToClientId.SpawnPlayer)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
    }

    [MessageHandler((ushort)ServerToClientId.PlayerMovement)]
    private static void PlayerMovement(Message message)
    {
        ushort id = message.GetUShort();
        if (list.TryGetValue(id, out Player player))
            player.Move(message.GetVector3(), message.GetVector3());
    }
}
