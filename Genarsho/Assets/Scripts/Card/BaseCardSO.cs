using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Card/CardSO")]
public class BaseCardSO : ScriptableObject
{
    public enum TargetType
    {
        Self,
        Enemy,
    }

    public enum Type
    {
        Heal = 0,
        MoveSpeed = 1,
        DrawCards = 2,
        DamageReduction = 3,
        IceWalk = 4,
        GiveWeapon = 5,
        BonusHealth = 6,
        BuildBox = 7,
    }

    public enum Rarity
    {
        Common = 0,
        Rare = 1,
        Epic = 2,
    }

    [field: SerializeField]
    public string CardName;

    [field: SerializeField]
    [field: TextArea]
    public string CardDescripton;

    [field: SerializeField]
    [Range(0,100)]
    protected int RareChance;

    [field: SerializeField]
    [Range(0, 100)]
    protected int EpicChance;

    [field: SerializeField]
    public TargetType Target;

    [field: SerializeField]
    public Type type;

    [field: SerializeField]
    public AbilityBase Ability;

    [field: SerializeField]
    public bool IsUnique = false;

    [field: SerializeField]
    public Sprite imageContent;

    [field: SerializeField]
    public Sprite imageIcon;

    public int RollForCommon
    {
        get
        {
            return 100-RareChance-EpicChance;
        }
    }

    public int RollForRare
    {
        get
        {
            return 100-EpicChance;
        }
    }

    public int RollForEpic
    {
        get
        {
            return 100-EpicChance;
        }
    }
}
