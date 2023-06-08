using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WeaponRenderer : MonoBehaviour
{
    // we dont know the starting layer number or index,
    //if we start on zero, then we can go to the layer minus one holding weapon above head 
    // and renderer it behind the player
    // layer with grater ">" index weapon  in front of player
    [SerializeField]
    protected int playerSortingOrder = 0;
    [SerializeField]
    protected SpriteRenderer weaponRenderer;


    private void Awake()
    {
        weaponRenderer = GetComponent<SpriteRenderer>();
    }

    public void FlipSprite(bool val)
    {
        //pointing left to be in position and not upside down
        weaponRenderer.flipY = val;
    }

    public void RendererBehindHead(bool val)
    {
        //insure that we can renderer our weapon in front and behind avater
        if (val)
            weaponRenderer.sortingOrder = playerSortingOrder - 1;
        else
            weaponRenderer.sortingOrder = playerSortingOrder + 1;
    }

}
