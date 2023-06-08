using UnityEngine;

[CreateAssetMenu(menuName = "Card/Ability/HealingAbility")]
public class HealingAbility : AbilityBase
{
    [SerializeField]
    protected int CommonHealAmount = 1;

    [SerializeField]
    protected int RareHealAmount = 2;

    [SerializeField]
    protected int EpicHealAmount = 3;

    public override void ActivateAbility(PlayerManager playerManager, BaseCardSO.Rarity rarity)
    {
        var healAmount = CommonHealAmount;
        switch(rarity)
        {
            case BaseCardSO.Rarity.Common:
                healAmount = CommonHealAmount;
                break;
            case BaseCardSO.Rarity.Rare: 
                healAmount = RareHealAmount;
                break;
            case BaseCardSO.Rarity.Epic:
                healAmount = EpicHealAmount;
                break;
        }
        playerManager.Heal(healAmount);
    }
}
