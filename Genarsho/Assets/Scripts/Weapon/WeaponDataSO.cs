using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/WeaponData")]
public class WeaponDataSO : ScriptableObject
{
    [field: SerializeField]
    public Sprite weaponSprite;

    [field: SerializeField]
    public BulletDataSO BulletData;

    [field: SerializeField]
    public int AmmoCapacity { get; set; } = 100;

    [field: SerializeField]
    public bool AutomaticFire { get; set; } = false;

    [field: SerializeField]
    public float WeaponDelay { get; set; } = .1f;

    [field: SerializeField]
    public int BulletPerShot { get; set; } = 1;

    [field: SerializeField]
    public float BulletSpread { get; set; } = 1f;

    [field: SerializeField]
    public float audioClipVolume;

    [field: SerializeField]
    public AudioClip audioClipShoot;

    [field: SerializeField] 
    public AudioClip audioClipReload;
}