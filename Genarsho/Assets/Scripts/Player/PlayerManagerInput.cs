using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerManagerInput : NetworkBehaviour
{
    [SerializeField]
    protected movementScript movementScript;

    [SerializeField]
    protected WeaponInput weaponInput;

    [SerializeField]
    protected MeleeInput meleeInput;

    [SerializeField]
    protected CardsController cardsController;

    protected void Start()
    {
        PlayerInputController.Instance.Value.AddPlayer(this);
    }

    public void Fire()
    {
        weaponInput.OnFire();
    }

    public void StopFire()
    {
        weaponInput.OnStopFire();
    }

    public void Move(Vector2 vector)
    {
        movementScript.OnMove(vector);
    }

    public void UseCard(int cardNo)
    {
        cardsController.UseCard(cardNo);
    }

    public void Melee()
    {
        meleeInput.OnMelee();
    }
}
