using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerManager;

public class ScoreController : NetworkBehaviour
{
    public static ScoreController Instance { get; private set; }

    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    private int myScore = 0;
    private int enemyScore = 0;
    void Awake()
    {
        Instance = this;
        gameObject.GetComponent<NetworkObject>().Spawn();
    }

    public void RoundOver(OnRoundOverWinner status)
    {
        if (status == OnRoundOverWinner.Won)
        {
            myScore++;
        }
        if (status == OnRoundOverWinner.Lost)
        {
            enemyScore++;
        }
        GenerateOutput();

        if (enemyScore >= 2)
        {
            Debug.Log("Game over, you lost");
            gameOverController.Instance.GameOver(OnGameOverWinner.Lost);
            return;
        }
        if (myScore >= 2)
        {
            Debug.Log("Game over, you won");
            gameOverController.Instance.GameOver(OnGameOverWinner.Won);
            return;
        }

        if (enemyScore >= 1 || myScore >= 1)
        {
            GameController.Instance.CleanUpPlayersServerRpc();
        }
    }

    private void GenerateOutput()
    {
        textMeshProUGUI.text = $"{myScore} - {enemyScore}";
    }

    public enum OnGameOverWinner
    {
        Won = 0,
        Lost = 1,
    }
}
