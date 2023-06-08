using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameController : NetworkBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField]
    public GameObject playerPrefab;

    bool playersActive = false;

    [SerializeField]
    TextMeshProUGUI textMeshProUGUI;

    [SerializeField]
    GameObject WaitingCanvasGameObject;

    float time = 0f;

    int TextHandlerInteger = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (!IsServer)
        {
            WaitingCanvasGameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (IsServer)
        {
            if (!playersActive)
            {
                if (NetworkManager.ConnectedClientsIds.Count == 2)
                {
                    SpawnPlayersServerRpc();
                }
                else
                {
                    WaitingForClientTextHandler();
                }
            }
            if (playersActive && NetworkManager.ConnectedClientsIds.Count != 2)
            {
                CleanUpPlayersServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void CleanUpPlayersServerRpc()
    {
        //TODO CLEANUP BULLETS PROPERLY
        BulletObjectPool.Instance.ClearActiveBullets();
        itemSpawnScript.Instance.ClearMap();
        AbilityManager.Instance.ClearPlayersClientRpc();
        PlayerInputController.Instance.Value.ClearPlayersClientRpc();
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<NetworkObject>().Despawn();
        }
        playersActive = false;
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnPlayersServerRpc()
    {
        playersActive = true;
        WaitingCanvasGameObject.SetActive(false);
        var PlayerObjectA = Instantiate(playerPrefab, new Vector3(10, 0, 0), new Quaternion(0, 0, 0, 0));
        PlayerObjectA.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.ConnectedClientsIds[0]);

        var PlayerObjectB = Instantiate(playerPrefab, new Vector3(-10, 0, 0), new Quaternion(0, 0, 0, 0));
        PlayerObjectB.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.ConnectedClientsIds[1]);
        itemSpawnScript.Instance.GenerateMap();
    }

    void WaitingForClientTextHandler()
    {
        time += Time.deltaTime;
        if (time > 1f)
        {
            time = 0f;
            switch (TextHandlerInteger)
            {
                case 0:
                    textMeshProUGUI.text = "WAITING FOR A CLIENT  ..";
                    TextHandlerInteger++;
                    break;
                case 1:
                    textMeshProUGUI.text = "WAITING FOR A CLIENT . .";
                    TextHandlerInteger++;
                    break;
                case 2:
                    textMeshProUGUI.text = "WAITING FOR A CLIENT .. ";
                    TextHandlerInteger = 0;
                    break;
                default:
                    TextHandlerInteger = 0;
                    break;
            }
        }
    }
}
