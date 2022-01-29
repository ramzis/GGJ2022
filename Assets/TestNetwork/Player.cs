using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{

    [Client]
    void Update()
    {
        if (!hasAuthority) return;
        if (Input.GetKey(KeyCode.Space))
        {
            PlayerAction(1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            PlayerAction(2);
        }
        if (Input.GetKey(KeyCode.D))
        {
            PlayerAction(3);
        }
    }

    [Command]
    void PlayerAction(int a)
    {
        DoAction(a);
    }

    [ClientRpc]

    private void DoAction(int a)
    {
        Debug.Log(a);
    }
}
