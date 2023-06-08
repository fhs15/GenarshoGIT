using UnityEngine;

[CreateAssetMenu(menuName = "Card/Ability/BuildBoxAbility")]
public class BuildBoxAbility : AbilityBase
{
    [SerializeField]
    protected int BoxHealthCommon = 1;

    [SerializeField]
    protected int BoxHealthRare = 2;

    [SerializeField]
    protected int BoxHealthEpic = 3;

    public override void ActivateAbility(PlayerManager playerManager, BaseCardSO.Rarity rarity)
    {
        var boxHealth = BoxHealthCommon;
        switch(rarity)
        {
            case BaseCardSO.Rarity.Common:
                boxHealth = BoxHealthCommon;
                break;
            case BaseCardSO.Rarity.Rare:
                boxHealth = BoxHealthRare;
                break;
            case BaseCardSO.Rarity.Epic:
                boxHealth = BoxHealthEpic;
                break;
        }
        playerManager.BuildRelativeBox(boxHealth);
    }
}
