using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : NetworkBehaviour
{
    [SerializeField]
    protected Matchmaker matchmaker;

    public async void FindMatch()
    {
        bool found = await matchmaker.FindMatch();
        if (found)
        {
            NetworkManager.SceneManager.LoadScene("Arena", LoadSceneMode.Single);
        }
    }
    public void StartAsClient()
    {
        NetworkManager.StartClient();
        //TODO FIGURE OUT WHY IT SOMETIMES CRASHES THE GAME
        if (NetworkManager.IsConnectedClient)
        {
            NetworkManager.SceneManager.LoadScene("Arena", LoadSceneMode.Single);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("spil lukket");
    }
}
