using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public Sprite fullHeart, halfHeart, emptyHeart, fullHeartGold, halfHeartGold;
    Image heartImage;

    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public enum HeartStatus
    {
        Empty = 0,
        Half = 1,
        Full = 2,
    }

    public void SetHeartImage(HeartStatus status, bool goldHeart)
    {
        switch (status) 
        {
            case HeartStatus.Empty:
                heartImage.sprite = emptyHeart; 
                break;
            case HeartStatus.Half:
                if (goldHeart)
                {
                    heartImage.sprite = halfHeartGold;
                } 
                else
                {
                    heartImage.sprite = halfHeart;
                }
                break;
            case HeartStatus.Full:
                if (goldHeart)
                {
                    heartImage.sprite = fullHeartGold;
                }
                else
                {
                    heartImage.sprite = fullHeart;
                }
                break;
        }
    }
}
