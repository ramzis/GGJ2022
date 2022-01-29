using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{

    public int ActionId = 0;

    [Client]
    void Update()
    {
        if (!hasAuthority) return;
        if (Input.GetKeyDown(KeyCode.Q)) // build wall
        {
            CmdAction(5);
        }
        else if (Input.GetKeyDown(KeyCode.E)) // build bomb
        {
            CmdAction(6);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            CmdAction(1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            CmdAction(2);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            CmdAction(3);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            CmdAction(4);
        }
    }
    [Command]
    void CmdAction(int a)
    {
        RpcAction(a);
    }
    [ClientRpc]
    void RpcAction(int a)
    {
        ActionId = a;
    }

}
