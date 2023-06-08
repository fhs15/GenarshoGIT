using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class SigninAndInit : MonoBehaviour
{
    protected static SigninAndInit signinAndInit;

    void Start()
    {
        if(signinAndInit != null)
        {
            Destroy(this.gameObject);
            return;
        }
        signinAndInit = this;
        DontDestroyOnLoad(gameObject);
        UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
