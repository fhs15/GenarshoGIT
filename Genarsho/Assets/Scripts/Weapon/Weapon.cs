using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : NetworkBehaviour
{
    [SerializeField]
    protected GameObject muzzle;

    public int ammo = 10;

    protected WeaponDataSO WeaponData;

    [SerializeField]
    protected WeaponDataSO DefaultWeapon;

    [SerializeField]
    protected SpriteRenderer weaponSpriteRenderer;

    AudioSource audioSource;

    public UnityEvent<int> OnAmmoChange;

    public int Ammo
    {
        get { return ammo; }
        set
        {
            ammo = Mathf.Clamp(value, 0, WeaponData.AmmoCapacity);
            ammo = value;
        }
    }

    //ammo full
    public bool AmmoFull { get => Ammo >= WeaponData.AmmoCapacity; }

    protected bool isShooting = false;

    [SerializeField]
    protected bool ReloadCoroutine = false;

    [field: SerializeField]
    public UnityEvent OnShoot { get; set; }

    [field: SerializeField]
    public UnityEvent OnShootNoAmmo { get; set; }

    protected bool UnlimitedAmmo;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ChangeWeapon(DefaultWeapon);
    }

    public void ChangeWeapon(WeaponDataSO weaponData)
    {
        WeaponData = weaponData;
        weaponSpriteRenderer.sprite = WeaponData.weaponSprite;
        Ammo = WeaponData.AmmoCapacity;
        UnlimitedAmmo = Ammo == 0;
        OnAmmoChange?.Invoke(Ammo);
    }

    [ClientRpc]
    protected void ChangeToDefaultWeaponClientRpc()
    {
        ChangeWeapon(DefaultWeapon);
    }

    [ServerRpc]
    // når man klikker så skyder den
    public void TryShootingServerRpc()
    {
        isShooting = true;
    }

    [ServerRpc]
    //når man slipper knappen så stopper den med at skyde
    public void StopShootingServerRpc()
    {
        isShooting = false;
    }

    //reload ammo
    public void Reload(int ammo)
    {
        PlayOneShotAudioClipReloadClientRpc();
        Ammo = Mathf.Clamp(Ammo + ammo, 0, WeaponData.AmmoCapacity);
        OnAmmoChange?.Invoke(Ammo);
    }

    [ClientRpc]
    private void PlayOneShotAudioClipReloadClientRpc()
    {
        audioSource.PlayOneShot(WeaponData.audioClipReload, WeaponData.audioClipVolume);
    }

    private void Update()
    {
        if(IsOwner)
        {
            UseWeaponServerRpc();
        }
    }

    [ClientRpc]
    protected void InvokeOnShootClientRpc()
    {
        OnShoot?.Invoke();
    }

    [ClientRpc]
    protected void InvokeOnShootNoAmmoClientRpc()
    {
        OnShootNoAmmo?.Invoke();
    }

    [ClientRpc]
    protected void InvokeOnAmmoChangeClientRpc(int ammo)
    {
        OnAmmoChange?.Invoke(ammo);
    }

    [ServerRpc]
    private void UseWeaponServerRpc()
    {
        if (isShooting && ReloadCoroutine == false)
        {
            if (Ammo > 0 || UnlimitedAmmo)
            {
                Ammo--;
                InvokeOnAmmoChangeClientRpc(Ammo);
                InvokeOnShootClientRpc();
                for (int i = 0; i < WeaponData.BulletPerShot; i++)
                {
                    ShootBullet(i);
                }
            }
            if (Ammo <= 0 && !UnlimitedAmmo)
            {
                isShooting = false;
                InvokeOnShootNoAmmoClientRpc();
                ChangeToDefaultWeaponClientRpc();
            }
            FinishShooting();
        }
    }

    private void FinishShooting()
    {
        StartCoroutine(DelayNextShootCoroutine());
        if (WeaponData.AutomaticFire == false)
        {
            isShooting = false;
        }
    }

    protected IEnumerator DelayNextShootCoroutine()
    {
        ReloadCoroutine = true;
        yield return new WaitForSeconds(WeaponData.WeaponDelay);
        ReloadCoroutine = false;
    }

    private void ShootBullet(int bulletCount)
    {
        bool muzzleCollision = 1 <= Physics2D.OverlapBoxAll(muzzle.transform.position, muzzle.transform.localScale / 3, 0, LayerMask.GetMask("Wall")).Length;
        bool weaponCollision = 1 <= Physics2D.OverlapBoxAll(transform.position, transform.localScale / 1.5f, 0, LayerMask.GetMask("Wall")).Length;
                
        //shoot from muzzle
        if (!muzzleCollision && !weaponCollision)
        {
            SpawnBullet(muzzle.transform.position, CalculateAngle(muzzle, bulletCount));
        }
        //shoot from weapon
        if (muzzleCollision && !weaponCollision)
        {
            SpawnBullet(transform.position, CalculateAngle(muzzle, bulletCount));
        }
    }

    private void SpawnBullet(Vector3 position, Quaternion rotation)
    {
        if (IsServer)
        {
            PlayOneShotAudioClipShootClientRpc();
            //var bulletPrefab = Instantiate(WeaponData.BulletData.bulletPrefab, position, rotation);
            var bulletPrefab = BulletObjectPool.Instance.GetPooledObject();     
            bulletPrefab.transform.position = position;
            bulletPrefab.transform.rotation = rotation;
            bulletPrefab.SetActive(true);
            bulletPrefab.GetComponent<NetworkObject>().Spawn();
            bulletPrefab.GetComponent<Bullet>().BulletData = WeaponData.BulletData;
            bulletPrefab.GetComponent<Bullet>().init();
        }
    }

    [ClientRpc]
    private void PlayOneShotAudioClipShootClientRpc()
    {
        audioSource.PlayOneShot(WeaponData.audioClipShoot, WeaponData.audioClipVolume);
    }

    //Quaternion represent the rotation
    private Quaternion CalculateAngle(GameObject muzzle, int bulletCount)
    {
        float spread = GetSpread(bulletCount);
        Quaternion bulletSpreadRotation = Quaternion.Euler(new Vector3(0, 0, spread));
        return muzzle.transform.rotation * bulletSpreadRotation;
    }

    private float GetSpread(int bulletCount)
    {
        if (bulletCount == 0)
        {
            if (WeaponData.BulletSpread % 2 == 1)
            {
                return 0;
            }
            else
            {
                return WeaponData.BulletSpread;
            }
        }
        if (bulletCount % 2 == 0)
        {
            int x = bulletCount / 2;
            return WeaponData.BulletSpread * x;
        }
        else
        {
            int x = Mathf.CeilToInt(bulletCount / 2f);
            return -WeaponData.BulletSpread * x;
        }
    }
}
