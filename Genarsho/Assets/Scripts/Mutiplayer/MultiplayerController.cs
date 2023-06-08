using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerController : MonoBehaviour
{
    public GameObject Canvas;

    public NetworkObject NetworkObject;

    // Start is called before the first frame update
    void Start()
    {
        if (!NetworkObject.IsOwner)
        {
            Canvas.SetActive(false);
        }
    }
}
