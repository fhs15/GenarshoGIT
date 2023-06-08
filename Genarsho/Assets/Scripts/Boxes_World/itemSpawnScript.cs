using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class itemSpawnScript : NetworkBehaviour
{
    [SerializeField]
    private GameObject blackSquare;

    [SerializeField]
    private int amountOfSquares = 10;

    private List<GameObject> mapSquaresList;

    public static itemSpawnScript Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        mapSquaresList = new List<GameObject>();
    }

    public void GenerateMap()
    {
        if (IsServer)
        {
            for (int i = 0; i < amountOfSquares; i++)
            {
                GameObject tempObj = Instantiate(blackSquare, new Vector3((float)Random.Range(-800, 0) / 100f, (float)Random.Range(-450, 450) / 100f, 0), Quaternion.identity);
                tempObj.transform.localScale = new Vector3((float)Random.Range(10, 300) / 100f, (float)Random.Range(10, 300) / 100f, 1);
                tempObj.transform.Rotate(0, 0, Random.Range(0, 360));
                tempObj.GetComponent<NetworkObject>().Spawn();
                mapSquaresList.Add(tempObj);
            }
            for (int i = 0; i < amountOfSquares; i++)
            {
                GameObject tempObj = Instantiate(blackSquare, new Vector3((float)Random.Range(0, 800) / 100f, (float)Random.Range(-450, 450) / 100f, 0), Quaternion.identity);
                tempObj.transform.localScale = new Vector3((float)Random.Range(10, 300) / 100f, (float)Random.Range(10, 300) / 100f, 1);
                tempObj.transform.Rotate(0, 0, Random.Range(0, 360));
                tempObj.GetComponent<NetworkObject>().Spawn();
                mapSquaresList.Add(tempObj);
            }
            GameObject fixedBox1 = Instantiate(blackSquare, new Vector3(-8.5f,0f,0f), Quaternion.identity);
            fixedBox1.transform.localScale = new Vector3(1f,3f,1f);
            fixedBox1.GetComponent<NetworkObject>().Spawn();
            fixedBox1.GetComponent<itemDestructionScript>().health = 8;
            mapSquaresList.Add(fixedBox1);

            GameObject fixedBox2 = Instantiate(blackSquare, new Vector3(8.5f, 0f, 0f), Quaternion.identity);
            fixedBox2.transform.localScale = new Vector3(1f, 3f, 1f);
            fixedBox2.GetComponent<NetworkObject>().Spawn();
            fixedBox2.GetComponent<itemDestructionScript>().health = 8;
            mapSquaresList.Add(fixedBox2);
        }
    }

    public void ClearMap()
    {
        if (IsServer)
        {
            foreach (var map in mapSquaresList)
            {
                map.GetComponent<NetworkObject>().Despawn();
                Object.Destroy(map);
            }
            mapSquaresList.Clear();
        }
    }
}
