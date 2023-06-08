using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Ability/WeaponAbility")]
public class WeaponAbility : AbilityBase
{
    [SerializeField]
    protected float CommonAmmoMultiplier = 1;

    [SerializeField]
    protected float RareAmmoMultiplier = 2;

    [SerializeField]
    protected float EpicAmmoMultiplier = 3;

    public override void ActivateAbility(PlayerManager playerManager, BaseCardSO.Rarity rarity)
    {
        var ammoMultiplier = CommonAmmoMultiplier;
        switch (rarity)
        {
            case BaseCardSO.Rarity.Common:
                ammoMultiplier = CommonAmmoMultiplier;
                break;
            case BaseCardSO.Rarity.Rare:
                ammoMultiplier = RareAmmoMultiplier;
                break;
            case BaseCardSO.Rarity.Epic:
                ammoMultiplier = EpicAmmoMultiplier;
                break;
        }
        var Weapon = Random.Range(0, WeaponDataStorage.Instance.GetLenght());
        playerManager.ChangeWeaponServerRpc(Weapon, ammoMultiplier);
    }
}

