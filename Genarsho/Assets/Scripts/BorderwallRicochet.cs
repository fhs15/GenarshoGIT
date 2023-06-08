using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BorderwallRicochet : NetworkBehaviour
{
    [SerializeField]
    private AudioClip hitSoundBullet;

    [SerializeField]
    private AudioClip hitSoundMelee;

    private AudioSource src;

    private void Start()
    {
        src = gameObject.GetComponent<AudioSource>();
    }

    [ClientRpc]
    public void TakeDamageClientRpc(DamageType type)
    {
        switch (type)
        {
            case DamageType.Bullet:
                src.PlayOneShot(hitSoundBullet);
                break;
            case DamageType.Melee:
                src.PlayOneShot(hitSoundMelee);
                break;
        }
    }

    public enum DamageType
    {
        Bullet = 0,
        Melee = 1,
    }
}
