using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class MeleeWeapon : NetworkBehaviour
{
    [SerializeField]
    protected Transform weaponParent;

    [SerializeField]
    protected NetworkAnimator networkAnimator;

    [SerializeField]
    protected MeleeWeaponCollision meleeWeaponCollision;

    [SerializeField]
    private int meleeDamage = 2;

    [SerializeField]
    private float meleeSpeed = 1;

    public List<Collider2D> collisions = null;

    [SerializeField]
    private AudioClip meleeAttackSound;

    [SerializeField]
    private AudioSource src;

    bool ReloadCoroutine = false;

    private void Awake()
    {
        collisions = new List<Collider2D>();
        networkAnimator.Animator.speed = meleeSpeed;
    }
    public void Attack()
    {
        if (ReloadCoroutine == false)
        {
            StartAnimationServerRpc();
            StartCoroutine(WaitForReload());
            AttackLogicServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void AttackLogicServerRpc()
    {
        PlayMeleeAttackSoundClientRpc();
        Collider2D[] collionsArray = collisions.ToArray();
        if (collisions != null)
        {
            foreach (Collider2D collider in collionsArray)
            {
                switch (collider.tag)
                {
                    case "Player":
                        collider.GetComponent<Health>().TakeDamageClientRpc(meleeDamage, Health.DamageType.Melee);
                        break;
                    case "Wall":
                        collider.GetComponent<itemDestructionScript>().TakeDamageClientRpc(meleeDamage, itemDestructionScript.DamageType.Melee);
                        break;
                }
            }            
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartAnimationServerRpc()
    {
        networkAnimator.Animator.SetTrigger("Attack");
    }

    [ClientRpc]
    private void PlayMeleeAttackSoundClientRpc()
    {
        src.PlayOneShot(meleeAttackSound);
    }

    private void FixedUpdate()
    {
        if (!ReloadCoroutine) //prevents the animation from moving away from the hitbox during a hit
        {
            gameObject.transform.rotation = weaponParent.transform.rotation;
        }
        
    }

    protected IEnumerator WaitForReload()
    {
        ReloadCoroutine = true;
        yield return new WaitForSeconds(meleeSpeed*0.7f);
        ReloadCoroutine = false;
    }
}
