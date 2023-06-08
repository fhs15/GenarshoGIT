using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static ScoreController;

public class gameOverController : MonoBehaviour
{
    [SerializeField]
    private GameObject vignette;
    [SerializeField]
    private TextMeshProUGUI text;
    public void GameOver(OnGameOverWinner status)
    {
        Time.timeScale = 0f; //stops the game
        GameObject.FindGameObjectWithTag("PlayerController").gameObject.SetActive(false);
        switch (status)
        {
            case OnGameOverWinner.Won:
                text.color = new Color(0, 214, 0, 255);
                text.text = "YOU WON!";
                break;
            case OnGameOverWinner.Lost:
                text.color = new Color(180, 0, 0, 255);
                text.text = "YOU LOST";
                break;
        }
        vignette.SetActive(true);
    }
    public static gameOverController Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
