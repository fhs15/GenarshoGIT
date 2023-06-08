using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : NetworkBehaviour
{
    public static NetworkVariable<PlayerInputController> Instance = new();

    protected List<PlayerManagerInput> Players = new List<PlayerManagerInput>();

    protected void Start()
    {
        if (Instance.Value == null)
        {
            Instance.Value = this;
        }
    }

    public void AddPlayer(PlayerManagerInput player)
    {
        Players.Add(player);
    }

    [ClientRpc]
    public void ClearPlayersClientRpc()
    {
        Players.Clear();
    }

    protected void OnFire()
    {
        foreach (var p in Players)
        {
            if(p.IsOwner)
            {
                p.Fire();
            }
        }
    }

    protected void OnStopFire()
    {
        foreach (var p in Players)
        {
            if(p.IsOwner)
            {
                p.StopFire();
            }
        }
    }

    protected void OnMove(InputValue inputvalue)
    {
        foreach (var p in Players)
        {
            if (p.IsOwner)
            {
                p.Move(inputvalue.Get<Vector2>());
            }
        }
    }

    protected void OnStopMove(InputValue inputvalue)
    {
        foreach (var p in Players)
        {
            if (p.IsOwner)
            {
                p.Move(Vector2.zero);
            }
        }
    }

    protected void OnCard0()
    {
        foreach (var p in Players)
        {
            if (p.IsOwner)
            {
                p.UseCard(0);
            }
        }
    }

    protected void OnCard1()
    {
        foreach (var p in Players)
        {
            if (p.IsOwner)
            {
                p.UseCard(1);
            }
        }
    }

    protected void OnCard2()
    {
        foreach (var p in Players)
        {
            if (p.IsOwner)
            {
                p.UseCard(2);
            }
        }
    }

    protected void OnCard3()
    {
        foreach (var p in Players)
        {
            if (p.IsOwner)
            {
                p.UseCard(3);
            }
        }
    }

    protected void OnCard4()
    {
        foreach (var p in Players)
        {
            if (p.IsOwner)
            {
                p.UseCard(4);
            }
        }
    }

    protected void OnMelee()
    {
        foreach (var p in Players)
        {
            if (p.IsOwner)
            {
                p.Melee();
            }
        }
    }
}
