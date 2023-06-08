using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponInput : NetworkBehaviour
{
    protected float desiredAngle;

    [SerializeField]
    protected WeaponRenderer weaponRenderer;

    [SerializeField]
    protected Weapon weapon;

    [SerializeField]
    protected Transform weaponParent;

    // awake kaldes når ens script bliver åbnet, eller hvis et objekt 
    private void Awake()
    {
        AssignWeapon();
    }

    private void AssignWeapon()
    {
        weaponRenderer = GetComponentInChildren<WeaponRenderer>();
        weapon = GetComponentInChildren<Weapon>();
    }

    //public void OnLook(InputValue inputValue)
    //{
    //    var pointerPosition = inputValue.Get<Vector2>();
    //    pointerPosition = Camera.main.ScreenToWorldPoint(pointerPosition);
    //    var aimDirection = (Vector3)pointerPosition - weaponParent.position;
    //    desiredAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
    //    AdjustWeaponRendering();
    //    weaponParent.rotation = Quaternion.AngleAxis(desiredAngle, Vector3.forward);
    //}

    public void FixedUpdate()
    {
        if (!IsOwner) return;

        var pointerPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //may need Camera.ScreenToWorldPoint at later date
        pointerPosition = Camera.main.ScreenToWorldPoint(pointerPosition);
        var aimDirection = (Vector3)pointerPosition - weaponParent.position;
        desiredAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        AdjustWeaponRendering();
        weaponParent.rotation = Quaternion.AngleAxis(desiredAngle, Vector3.forward);
    }

    //her adjuster vi ved at flipspirte (gun),
    //og hvis man henter gun bag over hovedet så vender den vores gun og player
    protected void AdjustWeaponRendering()
    {
        if (weaponRenderer != null)
        {
            // "?" to check if its not now
            weaponRenderer.FlipSprite(desiredAngle > 90 || desiredAngle < -90);
            weaponRenderer.RendererBehindHead(desiredAngle < 180 && desiredAngle > 0);
        }
    }

    // skyder
    public void OnFire()
    {
        if (weapon != null)
            weapon.TryShootingServerRpc();
    }

    //stops shooting
    public void OnStopFire()
    {
        if (weapon != null)
            weapon.StopShootingServerRpc();
    }
}
