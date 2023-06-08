using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDataStorage : MonoBehaviour
{
    public static WeaponDataStorage Instance;

    [SerializeField]
    protected List<WeaponDataSO> weapons;

    protected void Start()
    {
        Instance = this;
    }

    public int GetLenght()
    {
        return weapons.Count;
    }

    public WeaponDataSO GetByIndex(int index)
    {
        if(index < weapons.Count)
        {
            return Instantiate(weapons[index]);
        }
        return null;
    }
}
