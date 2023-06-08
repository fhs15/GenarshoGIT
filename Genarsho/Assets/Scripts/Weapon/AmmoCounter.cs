using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoCounter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ammoCounter;

    public void OnAmmoChange(int ammo)
    {
        if (ammo > 0)
        {
            ammoCounter.text = ammo.ToString();
        }
        else
        {
            ammoCounter.text = "\u221E";
        }
    }
}
