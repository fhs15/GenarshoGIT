using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsController : MonoBehaviour
{
    public GameObject healthHeartPrefab;
    public Health playerObject;
    List<Heart> healthHearts = new List<Heart>();
    private int maxHealthOnStart;
    private bool goldHeart = false;

    private void Start()
    {
        maxHealthOnStart = playerObject.maxHealth;
        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();

        //determine total hearts from max health
        float healthRemainder = playerObject.maxHealth % 2;
        int heartsToMake = (int)((playerObject.maxHealth / 2) + healthRemainder);
        goldHeart = false;

        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        //determine which hearts are which status
        for (int i = 0; i < healthHearts.Count; i++)
        {
            if (i >= maxHealthOnStart/2)
            {
                goldHeart = true;
            }

            int clampedHealthHeart = (int)Mathf.Clamp(playerObject.health - (i * 2), 0, 2);
            healthHearts[i].SetHeartImage((Heart.HeartStatus)clampedHealthHeart, goldHeart);
        }
    }

    private void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(healthHeartPrefab);
        newHeart.transform.SetParent(transform);
        newHeart.transform.localScale = Vector3.one;

        Heart heartComponent = newHeart.GetComponent<Heart>();
        heartComponent.SetHeartImage(Heart.HeartStatus.Empty, goldHeart);
        healthHearts.Add(heartComponent);
    }

    private void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        healthHearts = new List<Heart>();
    }
}
