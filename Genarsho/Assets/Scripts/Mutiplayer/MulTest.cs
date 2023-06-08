using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MulTest : MonoBehaviour
{
    public NetworkManager NetworkManager;

    public void ClientStart()
    {
        NetworkManager.StartClient();
    }

    public void HostStart()
    {
        NetworkManager.StartHost();
    }
}
