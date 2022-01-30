using System;
using System.Collections;
using Mirror;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private bool canClose;
    
    private void Start()
    {
        canClose = false;
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
        canClose = true;
    }

    private void Update()
    {
        if (!canClose) return;
        if (Input.anyKeyDown)
        {
            var manager = FindObjectOfType<NetworkManager>();
            if (manager != null)
            {
                if (manager.mode == NetworkManagerMode.Host)
                {
                    manager.StopHost();
                }

                if (manager.mode == NetworkManagerMode.ClientOnly)
                {
                    manager.StopClient();
                }
            }
        }
    }
}
