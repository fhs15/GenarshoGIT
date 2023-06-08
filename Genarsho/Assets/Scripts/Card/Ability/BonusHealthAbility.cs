using UnityEngine;

[CreateAssetMenu(menuName = "Card/Ability/BonusHealthAbility")]
public class BonusHealthAbility : AbilityBase
{
    [SerializeField]
    protected int CommonHealAmount = 1;

    [SerializeField]
    protected int RareHealAmount = 2;

    [SerializeField]
    protected int EpicHealAmount = 2;

    [SerializeField]
    protected int CommonBonusHealth = 1;

    [SerializeField]
    protected int RareBonusHealth = 2;

    [SerializeField]
    protected int EpicBonusHealth = 4;

    public override void ActivateAbility(PlayerManager playerManager, BaseCardSO.Rarity rarity)
    {
        var healAmount = CommonHealAmount;
        var bonusHealth = CommonBonusHealth;
        switch (rarity)
        {
            case BaseCardSO.Rarity.Common:
                healAmount = CommonHealAmount;
                bonusHealth = CommonBonusHealth;
                break;
            case BaseCardSO.Rarity.Rare: 
                healAmount = RareHealAmount;
                bonusHealth = RareBonusHealth;
                break;
            case BaseCardSO.Rarity.Epic:
                healAmount = EpicHealAmount;
                bonusHealth = EpicBonusHealth;
                break;
        }
        playerManager.BonusHealth(bonusHealth);
        playerManager.Heal(healAmount);
    }
}
