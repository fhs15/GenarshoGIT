using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class CardsController : NetworkBehaviour
{
    public GameObject cardPrefab;
    public int luck = 0;
    private int chanceWithLuck {
        get {
            return UnityEngine.Random.Range(0 + luck, 101); ;
        }
    }
    public int cardCount = 5;
    public List<Card> cards = new List<Card>();

    public List<BaseCardSO> cardSOsDefault = new List<BaseCardSO>();

    private List<BaseCardSO> cardSOs = new List<BaseCardSO>();

    private bool DelayCardCoroutine = false;

    public PlayerManager playerManager;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] cardActivateSoundClips;

    [SerializeField]
    private AudioClip failToUseCardClip;

    private void Start()
    {        
        DrawCards();
    }

    public void UseCard(int cardIndex)
    {
        try
        {
            if (cardIndex <= cards.Count - 1 && DelayCardCoroutine == false)
            {
                Card card = cards[cardIndex];
                StartCoroutine(DelayNextCardCoroutine());
                card.ActivateCard();
                //check if card used draws cards as we do not want to delete a card from the new hand
                if (card.CardData.type != BaseCardSO.Type.DrawCards)
                {
                    card.gameObject.SetActive(false);
                    cards.RemoveAt(cardIndex);
                }
                PlayCardSoundServerRpc();
            }
        }
        catch (ArgumentOutOfRangeException e) 
        {
            audioSource.PlayOneShot(failToUseCardClip,0.5f);
            Debug.Log(e.Message);
        }
        catch (NullReferenceException e)
        {
            audioSource.PlayOneShot(failToUseCardClip,0.5f);
            Debug.LogWarning(e.Message);
        }
    }

    [ServerRpc]
    private void PlayCardSoundServerRpc()
    {
        PlayCardSoundClientRpc();
    }

    [ClientRpc]
    private void PlayCardSoundClientRpc()
    {
        int randomIndex = UnityEngine.Random.Range(0, cardActivateSoundClips.Length);
        audioSource.PlayOneShot(cardActivateSoundClips[randomIndex]);
    }

    protected IEnumerator DelayNextCardCoroutine()
    {
        DelayCardCoroutine = true;
        yield return new WaitForSeconds(0.3f);
        DelayCardCoroutine = false;
    }

    public void DrawCards()
    {
        cardSOs = cardSOsDefault;
        ClearCards();

        for (int i = 0; i < cardCount; i++)
        {
            CreateCard();
        }
    }

    private void CreateCard()
    {
        GameObject newCard = Instantiate(cardPrefab);
        newCard.transform.SetParent(transform);
        newCard.transform.localScale = Vector3.one;

        Card card = newCard.GetComponent<Card>();
        card.SetRarity(GenerateRarity());
        card.CardData = GenerateCardSO();
        card.init();
        cards.Add(card);
    }

    private void ClearCards()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        cards = new List<Card>();
    }

    private BaseCardSO.Rarity GenerateRarity()
    {
        switch (chanceWithLuck)
        {
            case < 50:
                return BaseCardSO.Rarity.Common;
            case < 80:
                return BaseCardSO.Rarity.Rare;
            case > 80:
                return BaseCardSO.Rarity.Epic;
            default: 
                return BaseCardSO.Rarity.Common;
        }
    }

    private BaseCardSO GenerateCardSO()
    {
        int index = UnityEngine.Random.Range(0, cardSOs.Count);
        BaseCardSO card = cardSOs[index];
        if (card.IsUnique)
        {
            cardSOs.RemoveAt(index);
        }
        return card;
    }
}
