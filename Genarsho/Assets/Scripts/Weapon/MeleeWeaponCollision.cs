using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MeleeWeaponCollision : MonoBehaviour
{
    [SerializeField]
    protected MeleeWeapon meleeWeapon;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        meleeWeapon.collisions.Add(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        meleeWeapon.collisions.Remove(collider);
    }
}
