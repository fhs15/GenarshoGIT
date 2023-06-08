using UnityEngine;

[CreateAssetMenu(menuName = "Card/Ability/IceWalkAbility")]
public class IceWalkAbility : AbilityBase
{
    [SerializeField]
    protected float CommonReducedDrag = 1f;

    [SerializeField]
    protected float RareReducedDrag = 0.5f;

    [SerializeField]
    protected float EpicReducedDrag = 0f;

    public override void ActivateAbility(PlayerManager playerManager, BaseCardSO.Rarity rarity)
    {
        var dragReduction = CommonReducedDrag;
        switch(rarity)
        {
            case BaseCardSO.Rarity.Common:
                dragReduction = CommonReducedDrag;
                break;
            case BaseCardSO.Rarity.Rare:
                dragReduction = RareReducedDrag;
                break;
            case BaseCardSO.Rarity.Epic:
                dragReduction = EpicReducedDrag;
                break;
        }
        playerManager.DragReduceServerRpc(dragReduction);
    }
}
