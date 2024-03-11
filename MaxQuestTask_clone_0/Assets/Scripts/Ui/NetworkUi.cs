using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUi : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    private void Start()
    {
        hostButton.onClick.AddListener(() => { NetworkManager.Singleton.StartHost(); });
        clientButton.onClick.AddListener(() => { NetworkManager.Singleton.StartClient();});
    }
}
