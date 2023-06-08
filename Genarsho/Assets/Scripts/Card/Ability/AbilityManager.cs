using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class AbilityManager : NetworkBehaviour
{
    public static AbilityManager Instance { get; private set; }

    public List<PlayerManager> playerManagers;

    void Awake()
    {
        playerManagers = new List<PlayerManager>();
        Instance = this;
    }

    [ClientRpc]
    public void ClearPlayersClientRpc()
    {
        playerManagers.Clear();
    }

    public PlayerManager GetPlayerManager(bool findLocalPlayer)
    {
        PlayerManager playerManager = null;
        foreach (PlayerManager manager in playerManagers)
        {
            if (manager.GetComponent<NetworkObject>().IsLocalPlayer == findLocalPlayer)
            {
                playerManager = manager;
                break;
            }
        }
        if (playerManager == null)
        {
            throw new NullReferenceException($"Enemy player could not be found");
        }
        return playerManager.GetComponent<PlayerManager>();
    }
}

