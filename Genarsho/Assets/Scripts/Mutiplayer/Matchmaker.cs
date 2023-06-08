using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class Matchmaker : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI errorMessage;

    public async Task<bool> FindMatch()
    {
        using (HttpClient httpClient = new())
        {
            var respone = await httpClient.GetAsync("https://genarshomatchmake.azurewebsites.net/api/Matchmaker");
            if (respone.StatusCode == HttpStatusCode.OK)
            {
                string joinCode = respone.Content.ReadAsStringAsync().Result;
                Debug.Log(joinCode);
                try
                {
                    var joinAllocation = await Relay.Instance.JoinAllocationAsync(joinCode);
                    NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
                    return NetworkManager.Singleton.StartClient();
                }
                catch(Exception e)
                {
                    Debug.LogError(e);
                    errorMessage.text = "Failed to join allocation, please try again";
                    return false;
                }
            }
            else if (respone.StatusCode == HttpStatusCode.NoContent)
            {
                var allocation = await Relay.Instance.CreateAllocationAsync(2);
                string joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
                Debug.Log(joinCode);
                await httpClient.PostAsync("https://genarshomatchmake.azurewebsites.net/api/Matchmaker?joinCode=" + joinCode, null);
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
                return NetworkManager.Singleton.StartHost();
            }
            return false;
        }
    }
}
