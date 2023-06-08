using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MeleeInput : NetworkBehaviour
{
    [SerializeField]
    protected MeleeWeapon meleeWeapon;

    public void OnMelee()
    {
        meleeWeapon.Attack();
    }
}
