using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletObjectPool : NetworkBehaviour
{
    public static BulletObjectPool Instance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    void Awake()
    {
        Instance = this;
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    public void ClearActiveBullets()
    {
        foreach (var bullet in pooledObjects)
        {
            if (bullet.activeInHierarchy)
            {
                bullet.GetComponent<NetworkObject>().Despawn(false);
                bullet.SetActive(false);
            }
        }
    }

    void Start()
    {
        if (IsServer)
        {
            pooledObjects = new List<GameObject>();
            GameObject tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(objectToPool);
                tmp.SetActive(false);
                pooledObjects.Add(tmp);
            }
        }
    }
}