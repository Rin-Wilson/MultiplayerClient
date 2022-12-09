using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Riptide;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;
    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
                Destroy(value);
        }
    }

    [SerializeField] private TMP_InputField UsernameInput;
    [SerializeField] private GameObject menuScreen;

    private void Awake()
    {
        Singleton = this;
    }

    public void OnConnectClicked()
    {
        UsernameInput.interactable = false;
        menuScreen.SetActive(false);

        ClientManager.Singleton.Connect();
    }

    public void BackToMenu()
    {
        UsernameInput.interactable = true;
        menuScreen.SetActive(true);
    }

    public void SendUsername()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.PlayerName);
        message.AddString(UsernameInput.text);
        ClientManager.Singleton.Client.Send(message);
    }
}
