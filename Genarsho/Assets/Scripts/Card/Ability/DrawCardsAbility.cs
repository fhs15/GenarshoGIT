using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Ability/DrawCardsAbility")]
public class DrawCardsAbility : AbilityBase
{
    [SerializeField]
    protected int CommonLuckAdded = 60;

    [SerializeField]
    protected int RareLuckAdded = 60;

    [SerializeField]
    protected int EpicLuckAdded = 100;

    [SerializeField]
    protected int CommonCardsToDraw = 3;

    [SerializeField]
    protected int RareCardsToDraw = 4;

    [SerializeField]
    protected int EpicCardsToDraw = 4;

    public override void ActivateAbility(PlayerManager playerManager, BaseCardSO.Rarity rarity)
    {
        var luckAdded = CommonLuckAdded;
        var cardsToDraw = CommonCardsToDraw;
        switch (rarity)
        {
            case BaseCardSO.Rarity.Common:
                luckAdded = CommonLuckAdded;
                cardsToDraw = CommonCardsToDraw;
                break;
            case BaseCardSO.Rarity.Rare:
                luckAdded = RareLuckAdded;
                cardsToDraw = RareCardsToDraw;
                break;
            case BaseCardSO.Rarity.Epic:
                luckAdded = EpicLuckAdded;
                cardsToDraw = EpicCardsToDraw;
                break;
        }
        playerManager.ChangeLuck(luckAdded);
        playerManager.ChangeCardHandSize(cardsToDraw);
        playerManager.DrawCards();
    }
}

