using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Health : NetworkBehaviour
{
    public UnityEvent OnPlayerDamaged;
    public UnityEvent OnPlayerHealed;
    public UnityEvent OnPlayerDeath;

    public int health;
    public int maxHealth;

    [SerializeField]
    private AudioClip[] hitSounds;

    [SerializeField]
    private AudioSource src;

    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private AudioClip hitSoundBullet;

    [SerializeField]
    private AudioClip hitSoundMelee;

    private bool DamageTaken = false;

    private float time = 0f;

    private float colorFadeDuration = 1f;

    private void Start()
    {
        health = maxHealth;
    }

    [ServerRpc]
    public void GetHealedServerRpc(int amount)
    {
        GetHealedClientRpc(amount);
    }

    [ClientRpc]
    protected void GetHealedClientRpc(int amount)
    {
        var currHealth = health;
        if ((currHealth += amount) > maxHealth) health = maxHealth; else health += amount;
        OnPlayerHealed?.Invoke();
    }

    [ServerRpc]
    public void AddMaxHealthServerRpc(int amount)
    {
        AddMaxHealthClientRpc(amount);
    }

    [ClientRpc]
    protected void AddMaxHealthClientRpc(int amount)
    {
        maxHealth += amount;
    }

    [ClientRpc]
    public void TakeDamageClientRpc(int amount, DamageType type)
    {
        time = 0f;
        StartCoroutine(TakeDamageCoroutine());
        health -= amount;
        OnPlayerDamaged?.Invoke();

        int randomIndex = UnityEngine.Random.Range(0, hitSounds.Length);
        src.PlayOneShot(hitSounds[randomIndex]);
        switch (type)
        {
            case DamageType.Bullet:
                src.PlayOneShot(hitSoundBullet);
                break;
            case DamageType.Melee:
                src.PlayOneShot(hitSoundMelee);
                break;
        }

        if (health <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        if (DamageTaken)
        {
            time += Time.deltaTime;
            sprite.color = Color.Lerp(Color.red, Color.white, time / colorFadeDuration);
        }
        else
        {
            sprite.color = Color.white;
        }
    }

    protected IEnumerator TakeDamageCoroutine()
    {
        DamageTaken = true;
        yield return new WaitForSeconds(colorFadeDuration);
        DamageTaken = false;
    }

    public enum DamageType
    {
        Bullet = 0,
        Melee = 1,
    }
}
