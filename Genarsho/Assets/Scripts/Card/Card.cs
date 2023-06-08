using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BaseCardSO;
using static Card;
using static CardsController;

public class Card : MonoBehaviour
{
    [SerializeField]
    Sprite cardBG_blue, cardBG_red, cardBG_grey, cardBG_green, cardBG_yellow;
    [SerializeField]
    Sprite cardFrame_common, cardFrame_rare, cardFrame_epic;
    [SerializeField]
    Sprite cardFooter_common, cardFooter_rare, cardFooter_epic;
    [SerializeField]
    Sprite cardiconContent_placeholder;
    [SerializeField]
    Sprite cardframeContent_placeholder;

    [SerializeField]
    Image cardBG, cardFrame, cardframeContent, cardFooter, cardiconContent;

    [SerializeField]
    TextMeshProUGUI footerContent;

    [SerializeField]
    public BaseCardSO CardData;

    Rarity CardRarity;

    public void init()
    {
        if (CardData.imageIcon == null) 
        {
            cardiconContent.sprite = cardiconContent_placeholder;
        }
        else
        {
            cardiconContent.sprite = CardData.imageIcon;
        }

        if (CardData.imageContent == null)
        {
            cardframeContent.sprite = cardframeContent_placeholder;
        }
        else
        {
            cardframeContent.sprite = CardData.imageContent;
        }

        if (CardData.Target == BaseCardSO.TargetType.Self)
        {
            cardBG.sprite = cardBG_blue;
        }
        else
        {
            cardBG.sprite = cardBG_red;
        }

        footerContent.text = CardData.CardName;
    }

    public void ActivateCard()
    {
        switch (CardData.Target)
        {
            case BaseCardSO.TargetType.Self:
                CardData.Ability.ActivateAbility(AbilityManager.Instance.GetPlayerManager(true), CardRarity);
                break;
            case BaseCardSO.TargetType.Enemy:
                CardData.Ability.ActivateAbility(AbilityManager.Instance.GetPlayerManager(false), CardRarity);
                break;
            default:
                Debug.LogError("ActivateCard broke");
                break;
        }
    }

    public void SetRarity(Rarity rarity)
    {
        CardRarity = rarity;
        switch (rarity)
        {
            case Rarity.Common:
                cardFrame.sprite = cardFrame_common;
                cardFooter.sprite = cardFooter_common;
                break;
            case Rarity.Rare:
                cardFrame.sprite = cardFrame_rare;
                cardFooter.sprite = cardFooter_rare;
                break;
            case Rarity.Epic:
                cardFrame.sprite = cardFrame_epic;
                cardFooter.sprite = cardFooter_epic;
                break;
        }
    }
}
