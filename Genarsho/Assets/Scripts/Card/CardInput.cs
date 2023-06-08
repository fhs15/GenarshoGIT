using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardInput : MonoBehaviour
{
    public CardsController cardsController;

    void OnCard0()
    {
        cardsController.UseCard(0);
    }
    void OnCard1()
    {
        cardsController.UseCard(1);
    }
    void OnCard2()
    {
        cardsController.UseCard(2);
    }
    void OnCard3()
    {
        cardsController.UseCard(3);
    }
    void OnCard4()
    {
        cardsController.UseCard(4);
    }
}
